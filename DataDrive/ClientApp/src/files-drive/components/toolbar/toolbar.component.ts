import { Component, OnInit, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { DirectoryOut } from '../../models/directory-out';
import { CreateDirectoryPost } from '../../models/create-directory-post';
import { FilesService } from '../../services/files.service';
import { Form } from '@angular/forms';
import { MatMenu } from '@angular/material/menu';

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

    public newDirectory: CreateDirectoryPost;

    public progress: number;
    public message: string;

    constructor(private filesService: FilesService) {
        this.newDirectory = new CreateDirectoryPost();
    }

    ngOnInit() {
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

    public createDirectory() {

        if (this.newDirectory && this.newDirectory.name) {

            this.newDirectory.name = this.newDirectory.name.trim();

            if (this.newDirectory.name.length < 1) {

                console.log("3");
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
