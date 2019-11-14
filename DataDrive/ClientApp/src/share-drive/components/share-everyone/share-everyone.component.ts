import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FilesService } from '../../../files-drive/services/files.service';
import { SharesService } from '../../services/shares.service';
import { ShareEveryoneOut } from '../../../files-drive/models/share-everyone-out';
import { HttpErrorResponse, HttpResponseBase } from '@angular/common/http';
import { ShareEveryoneCredentials } from '../../models/share-everyone-credentials';

@Component({
    selector: 'app-share-everyone',
    templateUrl: './share-everyone.component.html',
    styleUrls: ['./share-everyone.component.css']
})
export class ShareEveryoneComponent implements OnInit {

    private token: string;

    private shareInfo: ShareEveryoneOut;

    constructor(private route: ActivatedRoute, private router: Router, private filesService: FilesService, private sharesService: SharesService) {
        this.token = this.route.snapshot.params.token;

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

                        //TODO dialog do wpisania hasła
                        this.sharesService.getShareByTokenAndPassword(new ShareEveryoneCredentials(this.token, '1234'))
                            .subscribe(result => {

                                this.shareInfo = result;
                            }, (err: HttpErrorResponse) => {

                                alert(err.error);
                            })
                        break;
                    }
                    default: {
                        this.router.navigateByUrl("/");
                        break;
                    }
                }
            });
    }

    ngOnInit() {
    }



}
