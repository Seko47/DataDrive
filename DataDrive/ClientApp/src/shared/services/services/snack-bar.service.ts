import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';

@Injectable({
    providedIn: 'root'
})
export class SnackBarService {

    constructor(private _snackBar: MatSnackBar) { }

    openSnackBar(message: string, action?: string, duration?: number): MatSnackBarRef<any> {

        return this._snackBar.open(message, action, {
            duration: duration,
        });
    }
}
