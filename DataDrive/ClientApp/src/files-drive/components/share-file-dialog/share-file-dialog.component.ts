import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FilesService } from '../../services/files.service';
import { FileOut } from '../../models/file-out';

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

    constructor(
        public dialogRef: MatDialogRef<ShareFileDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData) {

        this.file = this.data.file;

        if (this.file.isShared) {
            if (this.file.isSharedForEveryone) {

            }
        }
    }

    onNoClick(): void {
        this.dialogRef.close();
    }
}
