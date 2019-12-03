import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { FileOut, ResourceType } from '../../models/file-out';
import { FilesService } from '../../services/files.service';
import { CreateDirectoryPost } from '../../models/create-directory-post';
import { MatSidenav } from '@angular/material/sidenav';
import { Operation, compare } from 'fast-json-patch';
import { FileMove } from '../../models/file-move';
import { saveAs } from 'file-saver';
import { EventService, EventCode } from '../../services/files-event.service';
import { HttpResponse } from '@angular/common/http';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { ChangeFileNameDialogComponent } from '../change-file-name-dialog/change-file-name-dialog.component';
import { filter } from 'rxjs/operators';
import { ShareFileDialogComponent } from '../share-file-dialog/share-file-dialog.component';

@Component({
    selector: 'drive-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit, OnDestroy {

    public actualDirectory: DirectoryOut;
    public actualFile: FileOut;

    @ViewChild('fileinfosidenav', null) fileinfosidenav: MatSidenav;

    public changeFileNameDialogRef: MatDialogRef<ChangeFileNameDialogComponent>;


    constructor(private dialog: MatDialog, private filesService: FilesService, private filesEventService: EventService) {

        this.actualDirectory = new DirectoryOut();
        this.actualDirectory.id = null;
        this.actualDirectory.name = "Root";

        this.actualFile = new FileOut();
    }

    ngOnInit() {
        this.getFromDirectory(null);

        this.filesEventService.asObservable().subscribe((message: [EventCode, string, string?]) => {

            const eventCode = message[0];
            const fileId = message[1];

            switch (eventCode) {

                case EventCode.RENAME: {

                    if (message[2] && message[2].length > 0) {

                        const oldFileName = message[2];

                        this.openChangeFileNameDialog(fileId, oldFileName);
                    }
                    break;
                }
                case EventCode.DELETE: {

                    this.deleteFile(fileId);
                    break;
                }
                case EventCode.DOWNLOAD: {

                    this.downloadFile(fileId);
                    break;
                }
                case EventCode.SHARE: {

                    this.openShareFileDialog(fileId);

                    break;
                }
            }
        });
    }

    ngOnDestroy(): void {
        this.filesEventService.unsubscribe();
    }

    public openChangeFileNameDialog(fileId: string, oldFileName: string) {

        this.changeFileNameDialogRef = this.dialog.open(ChangeFileNameDialogComponent, {
            hasBackdrop: true,
            data: {
                filename: oldFileName
            }
        });

        this.changeFileNameDialogRef
            .afterClosed()
            .pipe(filter(name => name))
            .subscribe((newFileName: string) => {

                this.changeFileName(fileId, newFileName);
            }, err => alert(err.error));
    }

    public openShareFileDialog(fileId: string) {

        this.filesService.getFileInfo(fileId)
            .subscribe(result => {

                const file: FileOut = result;

                if (file) {
                    const dialogRef = this.dialog.open(ShareFileDialogComponent, {
                        hasBackdrop: true,
                        data: {
                            file: file
                        }
                    });

                    dialogRef.afterClosed().subscribe(result => {
                        this.getFromDirectory(this.actualDirectory.id);
                    }, err => alert(err.error));
                }
            });
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
        this.actualFile = null;
        if (id == null) {
            this.actualFile = new FileOut();
            this.actualFile.name = "Root";
            this.actualFile.createdDateTime = new Date();
            this.actualFile.lastModifiedDateTime = new Date();
            this.actualFile.resourceType = ResourceType.DIRECTORY;

            this.fileinfosidenav.close();
        }
        else {
            this.filesService.getFileInfo(id)
                .subscribe(result => {

                    this.actualFile = result;
                }, err => {
                    alert(err.error);
                });
            this.fileinfosidenav.toggle();
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

    private getFileInfoObserve(fileId: string) {
        return this.filesService.getFileInfo(fileId);
    }

    public changeFileName(fileId: string, newName: string) {

        this.getFileInfoObserve(fileId)
            .subscribe(result => {

                this.actualFile = result;

                const modifiedFile: FileOut = JSON.parse(JSON.stringify(this.actualFile));
                modifiedFile.name = newName;
                const patch: Operation[] = compare(this.actualFile, modifiedFile);

                this.filesService.updateFile(this.actualFile.id, patch)
                    .subscribe((result: FileOut) => {
                        let fileId = result.parentDirectoryID;

                        if (result.resourceType == ResourceType.DIRECTORY) {

                            fileId = result.id;
                        }

                        if (result.resourceType == ResourceType.DIRECTORY && this.actualDirectory.id == result.id) {

                            this.getFromDirectory(result.id);
                        }
                        else {

                            this.getFromDirectory(result.parentDirectoryID);
                        }
                    }, err => alert(err.error));
            }, err => {
                alert(err.error);
            });;
    }

    public onFileClick(clickedFile: FileOut) {
        if (clickedFile.resourceType == ResourceType.DIRECTORY) {
            this.getFromDirectory(clickedFile.id);
        }
        else if (clickedFile.resourceType == ResourceType.FILE) {
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
