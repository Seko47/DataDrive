import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FilesService } from '../../../files-drive/services/files.service';
import { SharesService } from '../../services/shares.service';
import { ShareEveryoneOut } from '../../../files-drive/models/share-everyone-out';
import { HttpErrorResponse, HttpResponseBase, HttpResponse } from '@angular/common/http';
import { ShareEveryoneCredentials } from '../../models/share-everyone-credentials';
import { MatDialog } from '@angular/material/dialog';
import { PasswordForTokenDialogComponent } from '../password-for-token-dialog/password-for-token-dialog.component';
import { FileOut } from '../../../files-drive/models/file-out';
import { EventService, EventCode } from '../../../files-drive/services/files-event.service';
import { saveAs } from 'file-saver';

@Component({
    selector: 'app-share-everyone',
    templateUrl: './share-everyone.component.html',
    styleUrls: ['./share-everyone.component.css']
})
export class ShareEveryoneComponent implements OnInit {

    private token: string;
    private password: string = "";

    public actualFile: FileOut;
    public shareInfo: ShareEveryoneOut;

    public urlToShareEveryone: string = window.location.origin + "/share/";

    constructor(private dialog: MatDialog, private route: ActivatedRoute, private router: Router, private filesService: FilesService, private sharesService: SharesService, private filesEventService: EventService) {

        this.token = this.route.snapshot.params.token;

        this.getShareInfoByToken();
    }

    ngOnInit() {
    }

    getFileOutByFileId(fileId: string) {
        this.filesService.getFileInfo(fileId)
            .subscribe(result => {

                this.actualFile = result;
            }, (err: HttpErrorResponse) => {
                switch (err.status) {
                    case 404: {
                        this.filesService.cancelShareFileForEveryone(fileId);

                        this.router.navigateByUrl("/");
                        break;
                    }
                }
            });
    }

    getShareInfoByToken() {
        this.sharesService.getShareByToken(this.token)
            .subscribe(result => {

                result.token = this.urlToShareEveryone + result.token;

                this.shareInfo = result;

                this.getFileOutByFileId(this.shareInfo.fileID);
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

                        result.token = this.urlToShareEveryone + result.token;

                        this.shareInfo = result;

                        this.getFileOutByFileId(this.shareInfo.fileID);
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

    downloadFile() {
        this.filesService.downloadFile(this.actualFile.id)
            .subscribe((result: HttpResponse<Blob>) => {

                let fileName = "download";
                if (result.headers.has("content-disposition")) {

                    let contentDisposition = result.headers.get("content-disposition");
                    const startIndex = contentDisposition.indexOf("filename=") + 9;
                    contentDisposition = contentDisposition.substr(startIndex);

                    const endIndex = contentDisposition.indexOf(';');
                    let rawFileName = contentDisposition.substring(0, endIndex);

                    if (rawFileName.startsWith('"') && rawFileName.endsWith('"')) {
                        rawFileName = rawFileName.substring(1, rawFileName.length - 1);
                    }

                    fileName = rawFileName;
                }

                saveAs(result.body, fileName);
            }, (err: HttpErrorResponse) => {
                if (err.status == 404) {
                    alert("Resource is no longer available");

                    this.router.navigateByUrl("/");
                }
            });
    }

    openPasswordDialog() {
        const dialogRef = this.dialog.open(PasswordForTokenDialogComponent, {
            disableClose: true,
            data: {
                token: this.token,
                password: ""
            }
        });

        return dialogRef.afterClosed();
    }

}
