import { Component, OnInit, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { DirectoryOut } from '../../models/directory-out';
import { CreateDirectoryPost } from '../../models/create-directory-post';
import { FilesService } from '../../services/files.service';

@Component({
    selector: 'drive-files-toolbar',
    templateUrl: './toolbar.component.html',
    styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent implements OnInit {

    @Input() actualDirectory: DirectoryOut;

    @Output() onGetParentDirectory = new EventEmitter<string>();
    @Output() onFilesUpload = new EventEmitter<File[]>();

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

    public uploadFiles(files) {

        if (files.length === 0) {
            return;
        }

        let filesToUpload = <File[]>files;

        this.onFilesUpload.emit(filesToUpload);
    }
}
