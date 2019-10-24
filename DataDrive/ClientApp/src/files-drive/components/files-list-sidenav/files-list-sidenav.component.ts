import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileOut } from '../../models/file-out';
import { ChangeFileNameDialogComponent } from '../change-file-name-dialog/change-file-name-dialog.component';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs/operators';
import { compare, Operation } from 'fast-json-patch';


@Component({
    selector: 'drive-files-list-sidenav',
    templateUrl: './files-list-sidenav.component.html',
    styleUrls: ['./files-list-sidenav.component.css']
})
export class FilesListSidenavComponent implements OnInit {

    @Input() actualFile: FileOut;

    @Output() onFileDelete = new EventEmitter<string>();
    @Output() onFileNameChanged = new EventEmitter<Operation[]>();

    public changeFileNameDialogRef: MatDialogRef<ChangeFileNameDialogComponent>;

    constructor(private createDirectoryDialog: MatDialog) {
    }

    ngOnInit() {
    }

    public openChangeFileNameDialog() {

        this.changeFileNameDialogRef = this.createDirectoryDialog.open(ChangeFileNameDialogComponent, {
            hasBackdrop: true,
            data: {
                filename: this.actualFile.name
            }
        });

        this.changeFileNameDialogRef
            .afterClosed()
            .pipe(filter(name => name))
            .subscribe((name: string) => {
                this.changeFileName(name);
            }, err => alert(err.error));
    }

    public deleteFile() {
        this.onFileDelete.emit(this.actualFile.id);
    }

    public changeFileName(name: string) {

        const modifiedFile: FileOut = JSON.parse(JSON.stringify(this.actualFile));
        modifiedFile.name = name;
        const patch: Operation[] = compare(this.actualFile, modifiedFile);

        this.onFileNameChanged.emit(patch);
    }
}
