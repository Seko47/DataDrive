import { Component, OnInit, Inject } from '@angular/core';
import { FilesService } from '../../../files-drive/services/files.service';
import { SharesService } from '../../services/shares.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotesService } from '../../../notes-drive/services/notes.service';
import { FileOut, ResourceType } from '../../../files-drive/models/file-out';
import { ShareForEveryoneIn } from '../../models/share-for-everyone-in';
import { ShareEveryoneOut } from '../../models/share-everyone-out';
import { NoteOut } from '../../../notes-drive/models/note-out';
import { ShareForUserIn } from '../../models/share-for-user-in';
import { ShareForUserOut } from '../../models/share-for-user-out';

export interface DialogData {
    file: FileOut;
    note: NoteOut,
    resourceType: ResourceType
}

@Component({
    selector: 'app-share-resource-dialog',
    templateUrl: './share-resource-dialog.component.html',
    styleUrls: ['./share-resource-dialog.component.css']
})
export class ShareResourceDialogComponent {

    private resourceId: string;
    private isShared: boolean;
    private isSharedForEveryone: boolean;
    private isSharedForUsers: boolean;

    public shareForEveryoneIn: ShareForEveryoneIn;
    public shareEveryoneOut: ShareEveryoneOut;

    public shareForUserIn: ShareForUserIn;
    public shareForUsersOut: ShareForUserOut[];

    public urlToShareEveryone: string = window.location.origin + "/share/";

    public shareEveryoneSliderChecked: boolean;

    constructor(
        public dialogRef: MatDialogRef<ShareResourceDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private sharesService: SharesService
    ) {

        if (this.data.resourceType == ResourceType.FILE) {

            this.resourceId = this.data.file.id;
            this.isShared = this.data.file.isShared;
            this.isSharedForEveryone = this.data.file.isSharedForEveryone;
            this.isSharedForUsers = this.data.file.isSharedForUsers;
        } else if (this.data.resourceType == ResourceType.NOTE) {

            this.resourceId = this.data.note.id;
            this.isShared = this.data.note.isShared;
            this.isSharedForEveryone = this.data.note.isSharedForEveryone;
            this.isSharedForUsers = this.data.note.isSharedForUsers;
        }
        else {
            this.closeDialog();
            return;
        }

        this.shareForEveryoneIn = new ShareForEveryoneIn();
        this.shareForEveryoneIn.resourceId = this.resourceId;

        this.shareForUserIn = new ShareForUserIn();
        this.shareForUserIn.resourceID = this.resourceId;

        this.shareEveryoneSliderChecked = this.isSharedForEveryone;

        if (this.isShared && this.isSharedForEveryone) {

            this.sharesService.getShareEveryoneInfo(this.resourceId)
                .subscribe(result => {

                    result.token = this.urlToShareEveryone + result.token;

                    this.shareEveryoneOut = result;
                    this.shareForEveryoneIn.downloadLimit = this.shareEveryoneOut.downloadLimit;
                    this.shareForEveryoneIn.expirationDateTime = this.shareEveryoneOut.expirationDateTime;
                }, err => alert(err.error));
        }

        if (this.isShared && this.isSharedForUsers) {

            this.sharesService.getShareForUsersInfo(this.resourceId)
                .subscribe(result => {

                    this.shareForUsersOut = result;
                    this.shareForUserIn.expirationDateTime = this.shareForUsersOut[this.shareForUsersOut.length - 1].expirationDateTime;
                }, err => alert(err.error));
        }
    }

    closeDialog(): void {
        this.dialogRef.close();
    }

    toggleResourceSharingByToken(event) {

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
            this.sharesService.cancelShareForEveryone(this.shareForEveryoneIn.resourceId)
                .subscribe(() => {
                    this.shareEveryoneOut = null;

                    this.shareForEveryoneIn = new ShareForEveryoneIn();
                    this.shareForEveryoneIn.resourceId = this.resourceId;
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
