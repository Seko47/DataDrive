import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FilesService } from '../../../files-drive/services/files.service';
import { SharesService } from '../../services/shares.service';
import { ShareEveryoneOut } from '../../../files-drive/models/share-everyone-out';
import { HttpErrorResponse, HttpResponseBase } from '@angular/common/http';
import { ShareEveryoneCredentials } from '../../models/share-everyone-credentials';
import { MatDialog } from '@angular/material/dialog';
import { PasswordForTokenDialogComponent, Status } from '../password-for-token-dialog/password-for-token-dialog.component';

@Component({
    selector: 'app-share-everyone',
    templateUrl: './share-everyone.component.html',
    styleUrls: ['./share-everyone.component.css']
})
export class ShareEveryoneComponent implements OnInit {

    private token: string;
    private password: string = "";

    private shareInfo: ShareEveryoneOut;

    constructor(private dialog: MatDialog, private route: ActivatedRoute, private router: Router, private filesService: FilesService, private sharesService: SharesService) {
        this.token = this.route.snapshot.params.token;

        this.getShareInfoByToken();
    }

    ngOnInit() {
    }

    getShareInfoByToken() {
        this.sharesService.getShareByToken(this.token)
            .subscribe(result => {

                this.shareInfo = result;
            }, (err: HttpErrorResponse) => {

                switch (err.status) {
                    case 404: {
                        this.router.navigateByUrl("/");
                        break;
                    }
                    case 401: {

                        this.getShareInfoByTokenAndPassword();
                        break;
                    }
                    default: {
                        this.router.navigateByUrl("/");
                        break;
                    }
                }
            });
    }

    getShareInfoByTokenAndPassword() {
        this.openPasswordDialog().subscribe(result => {

            if (result !== null) {
                this.password = result;

                this.sharesService.getShareByTokenAndPassword(new ShareEveryoneCredentials(this.token, this.password))
                    .subscribe(result => {

                        this.shareInfo = result;
                    }, (err: HttpErrorResponse) => {
                        switch (err.status) {
                            case 404: {

                                this.router.navigateByUrl("/");
                                break;
                            }
                            case 401: {

                                this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
                                    this.router.navigate(['share/' + this.token]);
                                });
                                break;
                            }
                            default: {

                                this.router.navigateByUrl("/");
                                break;
                            }
                        }
                    });
            }
        });
    }

    openPasswordDialog() {
        const dialogRef = this.dialog.open(PasswordForTokenDialogComponent, {
            disableClose: true,
            data: {
                token: this.token,
                password: "",
                status: Status.UNDEFINED
            }
        });

        return dialogRef.afterClosed();
    }

}
