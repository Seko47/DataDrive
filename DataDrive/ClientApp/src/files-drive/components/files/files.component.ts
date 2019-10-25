import { Component, OnInit, ViewChild } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { FileOut, FileType } from '../../models/file-out';
import { FilesService } from '../../services/files.service';
import { CreateDirectoryPost } from '../../models/create-directory-post';
import { MatSidenav } from '@angular/material/sidenav';
import { Operation } from 'fast-json-patch';
import { FileMove } from '../../models/file-move';
import { saveAs } from 'file-saver';

@Component({
    selector: 'drive-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

    public actualDirectory: DirectoryOut;
    public actualFile: FileOut;

    @ViewChild('fileinfosidenav', null) fileinfosidenav: MatSidenav;


    constructor(private filesService: FilesService) {
        this.actualDirectory = new DirectoryOut();
        this.actualDirectory.id = null;
        this.actualDirectory.name = "Root";

        this.actualFile = new FileOut();
    }

    ngOnInit() {
        this.getFromDirectory(null);
    }

    public getFromDirectory(id: string) {

        this.filesService
            .getFilesFromDirectory(id)
            .subscribe(result => {
                if (this.fileinfosidenav) {
                    this.fileinfosidenav.close();
                }
                this.actualDirectory = result;
            }, err => alert(err.error));
    }

    public uploadFiles(files: File[]) {
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

    public createDirectory(newDirectory: CreateDirectoryPost) {
        this.filesService.createDirectory(newDirectory)
            .subscribe(result => {
                this.getFromDirectory(result);
            }, err => alert(err.error));
    }

    public getFileInfo(id: string) {
        if (id == null) {
            this.actualFile = new FileOut();
            this.actualFile.name = "Root";
            this.actualFile.createdDateTime = new Date();
            this.actualFile.lastModifiedDateTime = new Date();
            this.actualFile.fileType = FileType.DIRECTORY;

            this.fileinfosidenav.close();
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

    public deleteFile(id: string) {
        this.filesService.deleteFile(id)
            .subscribe(result => {
                this.getFromDirectory(result.id);
            }, err => alert(err.error));
    }

    public downloadFile(id: string) {
        this.filesService.downloadFile(id)
            .subscribe(result => {
                if (this.fileinfosidenav) {
                    this.fileinfosidenav.close();
                }

                saveAs(result, this.actualFile.name);
            }, err => console.log(err.error));
    }

    public changeFileName(patch: Operation[]) {
        this.filesService.updateFile(this.actualFile.id, patch)
            .subscribe(result => {
                this.getFromDirectory(this.actualDirectory.id);
            }, err => alert(err.error));
    }

    public onFileClick(clickedFile: FileOut) {
        if (clickedFile.fileType == FileType.DIRECTORY) {
            this.getFromDirectory(clickedFile.id);
        }
        else if (clickedFile.fileType == FileType.FILE) {
            this.getFileInfo(clickedFile.id);
        }
    }

    public onFileMove(movedFile: FileMove) {
        if (movedFile.fileId && movedFile.patch) {
            this.filesService.updateFile(movedFile.fileId, movedFile.patch)
                .subscribe(result => {
                    this.getFromDirectory(this.actualDirectory.id);
                }, err => alert(err.error));
        }
    }
}
