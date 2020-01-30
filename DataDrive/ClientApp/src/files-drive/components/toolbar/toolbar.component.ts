import { Component, OnInit, Input, ViewChild, Output, EventEmitter, ElementRef } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { CreateDirectoryPost } from '../../models/create-directory-post';
import { MatMenu } from '@angular/material/menu';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CreateDirectoryDialogComponent } from '../create-directory-dialog/create-directory-dialog.component';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'drive-files-toolbar',
    templateUrl: './toolbar.component.html',
    styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent implements OnInit {

    @Input() actualDirectory: DirectoryOut;

    @Output() onGetParentDirectory = new EventEmitter<string>();
    @Output() onFilesUpload = new EventEmitter<File[]>();
    @Output() onDirectoryCreated = new EventEmitter<CreateDirectoryPost>();
    @Output() onGetFileInfo = new EventEmitter<string>();

    @ViewChild("createDirectoryMenu", null) createDirectoryMenu: MatMenu;
    @ViewChild("file", null) fileInputElement: ElementRef;

    public newDirectory: CreateDirectoryPost;
    public createDirectoryDialogRef: MatDialogRef<CreateDirectoryDialogComponent>;

    constructor(private createDirectoryDialog: MatDialog) {

        this.newDirectory = new CreateDirectoryPost();
    }

    ngOnInit() {
    }

    public openCreateDirectoryDialog() {

        this.createDirectoryDialogRef = this.createDirectoryDialog.open(CreateDirectoryDialogComponent, {
            hasBackdrop: true
        });

        this.createDirectoryDialogRef
            .afterClosed()
            .pipe(filter(name => name))
            .subscribe(name => {
                this.newDirectory.name = name;
                this.createDirectory();
            }, err => alert(err.error));
    }

    public getBackToParentDirectory() {

        this.onGetParentDirectory.emit(this.actualDirectory.parentDirectoryID);
    }

    public uploadFiles(files: File[]) {

        if (files.length === 0) {
            return;
        }

        this.onFilesUpload.emit(files);
    }

    public resetFileInput() {

        this.fileInputElement.nativeElement.value = "";
    }

    public createDirectory() {

        if (this.newDirectory && this.newDirectory.name) {

            this.newDirectory.name = this.newDirectory.name.trim();

            if (this.newDirectory.name.length < 1) {

                return;
            }

            this.newDirectory.parentDirectoryID = this.actualDirectory.id
            this.onDirectoryCreated.emit(this.newDirectory);
            this.newDirectory = new CreateDirectoryPost();
        }
    }

    public getFileInfo() {
        this.onGetFileInfo.emit(this.actualDirectory.id);
    }



    stopPropagation(event) {
        event.stopPropagation();
    }
}
