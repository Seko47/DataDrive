import { Component, OnInit, ViewChild } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { FileOut, FileType } from '../../models/file-out';
import { FilesService } from '../../services/files.service';

@Component({
    selector: 'drive-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

    public actualDirectory: DirectoryOut;
    public actualFile: FileOut;

    @ViewChild('fileinfosidenav', null) fileinfosidenav;


    constructor(private filesService: FilesService) {
        this.actualDirectory = new DirectoryOut();
        this.actualDirectory.id = null;
        this.actualDirectory.name = "Root";

        this.actualFile = new FileOut();
    }

    ngOnInit() {
        this.getFromDirectory(null);
    }

    public onGetParentDirectory(parentDirectoryId: string) {
        this.getFromDirectory(parentDirectoryId);
    }


    public getFromDirectory(id: string) {

        console.log("files.component.ts: getFromDirectoryId == " + id)

        this.filesService
            .getFilesFromDirectory(id)
            .subscribe(result => {
                console.log("files.component.ts: dirName == " + result.name);
                console.log("files.component.ts: filesInDir == " + result.files.length);
                this.actualDirectory = result;
            }, err => alert(err.error));
    }

    public onFilesUpload(files: File[]) {
        if (files.length === 0) {
            return;
        }

        const formData = new FormData();
        if (this.actualDirectory && this.actualDirectory.id) {
            formData.append("parentDirectoryId", this.actualDirectory.id);
        }

        if (files.length) {
            for (let i = 0; i < files.length; i++)
                formData.append('files[]', files[i], files[i].name);
        }

        let loading = true;

        this.filesService.uploadFiles(formData).
            subscribe(result => {
                loading = false;
                this.getFromDirectory(this.actualDirectory.id);
            },
                err => {
                    alert("err: " + err.error);
                    loading = false;
                });
    }















    public deleteFile() {
        this.filesService.deleteFile(this.actualFile.id)
            .subscribe(result => {
                this.onFileClick(result);
            }, err => alert(err.error));
    }

    public onFileClick(clickedFile: FileOut) {
        if (clickedFile.fileType == FileType.DIRECTORY) {
            console.log("files.component.ts:onFileClick(clickedFile: FileOut) | clicked directory")
            this.getFromDirectory(clickedFile.id);
        }
        else if (clickedFile.fileType == FileType.FILE) {
            console.log("files.component.ts:onFileClick(clickedFile: FileOut) | clicked file")
            this.getFileInfo(clickedFile.id);
        }
    }

    public getFileInfo(id: string) {
        if (id == null) {
            this.actualFile = new FileOut();
            this.actualFile.name = "Root";
            this.actualFile.createdDateTime = new Date();
            this.actualFile.lastModifiedDateTime = new Date();
            this.actualFile.fileType = FileType.DIRECTORY;

            this.fileinfosidenav.toggle();
        }
        else {
            this.filesService.getFileInfo(id)
                .subscribe(result => {
                    this.actualFile = result;
                    this.fileinfosidenav.toggle();
                }, err => {
                    alert(err.error);
                });
        }
    }

    public upload(files) {
        if (files.length === 0) {
            return;
        }

        let filesToUpload = <File[]>files;

        const formData = new FormData();
        if (this.actualDirectory && this.actualDirectory.id) {
            formData.append("parentDirectoryId", this.actualDirectory.id);
        }

        if (filesToUpload.length) {
            for (let i = 0; i < filesToUpload.length; i++)
                formData.append('files[]', filesToUpload[i], filesToUpload[i].name);
        }

        let loading = true;

        this.filesService.uploadFiles(formData).
            subscribe(result => {
                loading = false;
                this.getFromDirectory(this.actualDirectory.id);
            },
                err => {
                    alert("err: " + err.error);
                    loading = false;
                });
    }

    stopPropagation(event) {
        event.stopPropagation();
    }
}