import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';

export interface DialogData {
    token: string;
    password: string;
}

@Component({
    selector: 'app-password-for-token-dialog',
    templateUrl: './password-for-token-dialog.component.html',
    styleUrls: ['./password-for-token-dialog.component.css']
})
export class PasswordForTokenDialogComponent {

    constructor(public dialogRef: MatDialogRef<PasswordForTokenDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private router: Router) { }

    onNoClick(): void {
        this.data.password = "";
        this.dialogRef.close(null);
        this.router.navigateByUrl('/');
    }
}
