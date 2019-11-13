import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FilesService } from '../../services/files.service';
import { FileOut } from '../../models/file-out';
import { ShareForEveryoneIn } from '../../models/share-for-everyone-in';
import { ShareEveryoneOut } from '../../models/share-everyone-out';

export interface DialogData {
    file: FileOut;
}

@Component({
    selector: 'drive-share-file-dialog',
    templateUrl: './share-file-dialog.component.html',
    styleUrls: ['./share-file-dialog.component.css']
})
export class ShareFileDialogComponent {

    public file: FileOut;
    public shareForEveryoneIn: ShareForEveryoneIn;
    public shareEveryoneOut: ShareEveryoneOut;

    public urlToShareEveryone: string = window.location.origin + "/share/";

    public shareEveryoneSliderChecked: boolean;

    constructor(
        public dialogRef: MatDialogRef<ShareFileDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fileService: FilesService) {

        this.file = this.data.file;
        this.shareForEveryoneIn = new ShareForEveryoneIn();
        this.shareForEveryoneIn.fileId = this.file.id;
        this.shareEveryoneSliderChecked = this.file.isSharedForEveryone;

        if (this.file.isShared) {
            if (this.file.isSharedForEveryone) {

            }
        }
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    toggleFileSharingByToken(event) {

        this.shareEveryoneSliderChecked = event.checked;
        if (this.shareEveryoneSliderChecked) {

            this.fileService.shareFileForEveryone(this.shareForEveryoneIn)
                .subscribe(result => {

                    result.token = this.urlToShareEveryone + result.token;

                    this.shareEveryoneOut = result;
                    this.shareForEveryoneIn.downloadLimit = this.shareEveryoneOut.downloadLimit;
                    this.shareForEveryoneIn.expirationDateTime = this.shareEveryoneOut.expirationDateTime;
                }, err => alert(err.error));
        }
        else {
            this.fileService.cancelShareFileForEveryone(this.shareForEveryoneIn.fileId)
                .subscribe(() => {
                    this.shareEveryoneOut = null;
                }, err => alert(err.error));
        }
    }

    saveShareForEveryone() {
        this.fileService.shareFileForEveryone(this.shareForEveryoneIn)
            .subscribe(result => {

                result.token = this.urlToShareEveryone + result.token;

                this.shareEveryoneOut = result;
                this.shareForEveryoneIn.downloadLimit = this.shareEveryoneOut.downloadLimit;
                this.shareForEveryoneIn.expirationDateTime = this.shareEveryoneOut.expirationDateTime;
            }, err => alert(err.error));
    }
}
