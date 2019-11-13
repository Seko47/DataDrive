import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FilesService } from '../../services/files.service';
import { FileOut } from '../../models/file-out';
import { ShareForEveryoneIn } from '../../models/share-for-everyone-in';
import { ShareEveryoneOut } from '../../models/share-everyone-out';
import { MatInput } from '@angular/material/input';
import { Router } from '@angular/router';

export interface DialogData {
    file: FileOut;
}

@Component({
    selector: 'drive-share-file-dialog',
    templateUrl: './share-file-dialog.component.html',
    styleUrls: ['./share-file-dialog.component.css']
})
export class ShareFileDialogComponent {

    @ViewChild("tokenInput", null) tokenInput: MatInput;

    public file: FileOut;
    public shareForEveryoneIn: ShareForEveryoneIn;
    public shareEveryoneOut: ShareEveryoneOut;

    public urlToShareEveryone: string = "drive/share/";

    public shareEveryoneSliderChecked: boolean;

    constructor(
        public dialogRef: MatDialogRef<ShareFileDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fileService: FilesService,
        private router: Router) {

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
}
