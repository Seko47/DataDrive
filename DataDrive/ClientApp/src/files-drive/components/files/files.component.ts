import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { FileOut, FileType } from '../../models/file-out';
import { FilesService } from '../../services/files.service';
import { CreateDirectoryPost } from '../../models/create-directory-post';
import { MatSidenav } from '@angular/material/sidenav';
import { Operation, compare } from 'fast-json-patch';
import { FileMove } from '../../models/file-move';
import { saveAs } from 'file-saver';
import { FilesEventService, FilesEventCode } from '../../services/files-event.service';
import { HttpResponse } from '@angular/common/http';

@Component({
    selector: 'drive-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit, OnDestroy {

    public actualDirectory: DirectoryOut;
    public actualFile: FileOut;

    @ViewChild('fileinfosidenav', null) fileinfosidenav: MatSidenav;


    constructor(private filesService: FilesService, private filesEventService: FilesEventService) {

        this.actualDirectory = new DirectoryOut();
        this.actualDirectory.id = null;
        this.actualDirectory.name = "Root";

        this.actualFile = new FileOut();
    }

    ngOnInit() {
        this.getFromDirectory(null);

        this.filesEventService.asObservable().subscribe((message: [FilesEventCode, string, string?]) => {

            const eventCode = message[0];
            const fileId = message[1];

            switch (eventCode) {

                case FilesEventCode.RENAME: {

                    if (message[2] && message[2].length > 0) {

                        const newFileName = message[2];

                        this.changeFileName(fileId, newFileName);
                    }
                    break;
                }
                case FilesEventCode.DELETE: {

                    this.deleteFile(fileId);
                    break;
                }
                case FilesEventCode.DOWNLOAD: {

                    this.downloadFile(fileId);
                    break;
                }
            }
        });
    }

    ngOnDestroy(): void {
        this.filesEventService.unsubscribe();
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
            .subscribe((result: HttpResponse<Blob>) => {
                if (this.fileinfosidenav) {
                    this.fileinfosidenav.close();
                }

                let fileName = "download";
                if (result.headers.has("content-disposition")) {

                    let contentDisposition = result.headers.get("content-disposition");
                    const startIndex = contentDisposition.indexOf("filename=") + 9;
                    contentDisposition = contentDisposition.substr(startIndex);

                    const endIndex = contentDisposition.indexOf(';');
                    let rawFileName = contentDisposition.substring(0, endIndex);

                    if (rawFileName.startsWith('"') && rawFileName.endsWith('"')) {
                        rawFileName = rawFileName.substring(1, rawFileName.length - 1);
                    }

                    fileName = rawFileName;
                }

                saveAs(result.body, fileName);
            }, err => console.log(err.error));
    }

    public changeFileName(fileId: string, newName: string) {

        this.getFileInfo(fileId);
        const modifiedFile: FileOut = JSON.parse(JSON.stringify(this.actualFile));
        modifiedFile.name = newName;
        const patch: Operation[] = compare(this.actualFile, modifiedFile);

        this.filesService.updateFile(this.actualFile.id, patch)
            .subscribe(result => {
                let fileId = result.parentDirectoryID;

                if (result.fileType == FileType.DIRECTORY) {

                    fileId = result.id;
                }

                this.getFromDirectory(fileId);
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
