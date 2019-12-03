import { Component, OnInit, Inject } from '@angular/core';
import { FilesService } from '../../../files-drive/services/files.service';
import { SharesService } from '../../services/shares.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotesService } from '../../../notes-drive/services/notes.service';
import { FileOut } from '../../../files-drive/models/file-out';
import { ShareForEveryoneIn } from '../../models/share-for-everyone-in';
import { ShareEveryoneOut } from '../../models/share-everyone-out';

export interface DialogData {
    file: FileOut;
}

@Component({
    selector: 'app-share-resource-dialog',
    templateUrl: './share-resource-dialog.component.html',
    styleUrls: ['./share-resource-dialog.component.css']
})
export class ShareResourceDialogComponent {

    public file: FileOut;
    public shareForEveryoneIn: ShareForEveryoneIn;
    public shareEveryoneOut: ShareEveryoneOut;

    public urlToShareEveryone: string = window.location.origin + "/share/";

    public shareEveryoneSliderChecked: boolean;

    constructor(
        public dialogRef: MatDialogRef<ShareResourceDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fileService: FilesService,
        private notesService: NotesService,
        private sharesService: SharesService
    ) {

        this.file = this.data.file;
        this.shareForEveryoneIn = new ShareForEveryoneIn();
        this.shareForEveryoneIn.fileId = this.file.id;
        this.shareEveryoneSliderChecked = this.file.isSharedForEveryone;

        if (this.file.isShared) {
            if (this.file.isSharedForEveryone) {

                this.sharesService.getShareEveryoneInfo(this.file.id)
                    .subscribe(result => {

                        result.token = this.urlToShareEveryone + result.token;

                        this.shareEveryoneOut = result;
                        this.shareForEveryoneIn.downloadLimit = this.shareEveryoneOut.downloadLimit;
                        this.shareForEveryoneIn.expirationDateTime = this.shareEveryoneOut.expirationDateTime;
                    }, err => alert(err.error));
            }
        }
    }

    closeDialog(): void {
        this.dialogRef.close();
    }

    toggleFileSharingByToken(event) {

        this.shareEveryoneSliderChecked = event.checked;
        if (this.shareEveryoneSliderChecked) {

            this.sharesService.shareForEveryone(this.shareForEveryoneIn)
                .subscribe(result => {

                    result.token = this.urlToShareEveryone + result.token;

                    this.shareEveryoneOut = result;
                    this.shareForEveryoneIn.downloadLimit = this.shareEveryoneOut.downloadLimit;
                    this.shareForEveryoneIn.expirationDateTime = this.shareEveryoneOut.expirationDateTime;
                }, err => alert(err.error));
        }
        else {
            this.sharesService.cancelShareForEveryone(this.shareForEveryoneIn.fileId)
                .subscribe(() => {
                    this.shareEveryoneOut = null;

                    this.shareForEveryoneIn = new ShareForEveryoneIn();
                    this.shareForEveryoneIn.fileId = this.file.id;
                }, err => alert(err.error));
        }
    }

    saveShareForEveryone() {
        this.sharesService.shareForEveryone(this.shareForEveryoneIn)
            .subscribe(result => {

                result.token = this.urlToShareEveryone + result.token;

                this.shareEveryoneOut = result;
                this.shareForEveryoneIn.downloadLimit = this.shareEveryoneOut.downloadLimit;
                this.shareForEveryoneIn.expirationDateTime = this.shareEveryoneOut.expirationDateTime;
                //TODO przy zapisie data zapisuje się o dzień mniejsza niż wybrana
                this.closeDialog();
            }, err => alert(err.error));
    }
}
