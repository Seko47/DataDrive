import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FilesService } from '../../../files-drive/services/files.service';
import { SharesService } from '../../services/shares.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { ShareEveryoneCredentials } from '../../models/share-everyone-credentials';
import { MatDialog } from '@angular/material/dialog';
import { PasswordForTokenDialogComponent } from '../password-for-token-dialog/password-for-token-dialog.component';
import { FileOut, ResourceType } from '../../../files-drive/models/file-out';
import { saveAs } from 'file-saver';
import { ShareEveryoneOut } from '../../models/share-everyone-out';
import { NotesService } from '../../../notes-drive/services/notes.service';
import { NoteOut } from '../../../notes-drive/models/note-out';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ReportService } from '../../../shared/services/services/report.service';
import { Unit } from '../../../files-drive/models/user-disk-space';

@Component({
    selector: 'app-share-everyone',
    templateUrl: './share-everyone.component.html',
    styleUrls: ['./share-everyone.component.css']
})
export class ShareEveryoneComponent implements OnInit {

    private token: string;
    private password: string = "";

    public actualFile: FileOut;
    public actualNote: NoteOut;

    public shareInfo: ShareEveryoneOut;

    public urlToShareEveryone: string = window.location.origin + "/share/";

    editorConfig: AngularEditorConfig = {
        editable: false,
        spellcheck: true,
        height: '40vh',
        minHeight: '30vh',
        maxHeight: 'auto',
        width: 'auto',
        minWidth: '0',
        translate: 'yes',
        enableToolbar: false,
        showToolbar: false,
        placeholder: '',
        defaultParagraphSeparator: '',
        defaultFontName: '',
        defaultFontSize: '',
        fonts: [
            { class: 'arial', name: 'Arial' },
            { class: 'times-new-roman', name: 'Times New Roman' },
            { class: 'calibri', name: 'Calibri' },
            { class: 'comic-sans-ms', name: 'Comic Sans MS' }
        ],
        uploadUrl: '',
        sanitize: true,
        toolbarPosition: 'bottom',
        toolbarHiddenButtons: [
            ['insertImage', 'insertVideo']
        ]
    };

    constructor(private reportService: ReportService, private dialog: MatDialog, private route: ActivatedRoute, private router: Router, private filesService: FilesService, private notesService: NotesService, private sharesService: SharesService) {

        this.token = this.route.snapshot.params.token;

        this.getShareInfoByToken();
    }

    ngOnInit() {
    }

    getFileOutById(fileId: string) {
        this.filesService.getFileInfo(fileId)
            .subscribe(result => {
                this.actualFile = result;
                this.calculateSize();
            }, (err: HttpErrorResponse) => {
                switch (err.status) {
                    case 404: {
                        this.sharesService.cancelShareForEveryone(fileId);
                        this.router.navigateByUrl("/");
                        break;
                    }
                }
            });
    }

    getNoteOutById(noteId: string) {
        this.notesService.getNoteById(noteId)
            .subscribe(result => {
                this.actualNote = result;
            }, (err: HttpErrorResponse) => {
                switch (err.status) {
                    case 404: {
                        this.sharesService.cancelShareForEveryone(noteId);
                        this.router.navigateByUrl("/");
                        break;
                    }
                }
            });
    }

    getShareInfoByToken() {
        this.sharesService.getShareByToken(this.token)
            .subscribe(result => {

                result.token = this.urlToShareEveryone + result.token;
                this.shareInfo = result;

                if (this.shareInfo.resourceType == ResourceType.FILE) {
                    this.getFileOutById(this.shareInfo.resourceID);
                }
                else if (this.shareInfo.resourceType == ResourceType.NOTE) {
                    this.getNoteOutById(this.shareInfo.resourceID);
                }
            }, (err: HttpErrorResponse) => {

                switch (err.status) {
                    case 404: {
                        this.router.navigateByUrl("/");
                        break;
                    }
                    case 401: {

                        this.getShareInfoByTokenAndPassword();
                        break;
                    }
                    default: {
                        this.router.navigateByUrl("/");
                        break;
                    }
                }
            });
    }

    getShareInfoByTokenAndPassword() {
        this.openPasswordDialog().subscribe(result => {

            if (result !== null) {
                this.password = result;

                this.sharesService.getShareByTokenAndPassword(new ShareEveryoneCredentials(this.token, this.password))
                    .subscribe(result => {

                        result.token = this.urlToShareEveryone + result.token;

                        this.shareInfo = result;

                        if (this.shareInfo.resourceType == ResourceType.FILE) {
                            this.getFileOutById(this.shareInfo.resourceID);
                        }
                        else if (this.shareInfo.resourceType == ResourceType.NOTE) {
                            this.getNoteOutById(this.shareInfo.resourceID);
                        }
                    }, (err: HttpErrorResponse) => {
                        switch (err.status) {
                            case 404: {

                                this.router.navigateByUrl("/");
                                break;
                            }
                            case 401: {

                                this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
                                    this.router.navigate(['share/' + this.token]);
                                });
                                break;
                            }
                            default: {

                                this.router.navigateByUrl("/");
                                break;
                            }
                        }
                    });
            }
        });
    }

    downloadFile() {
        this.filesService.downloadFile(this.actualFile.id)
            .subscribe((result: HttpResponse<Blob>) => {

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
            }, (err: HttpErrorResponse) => {
                if (err.status == 404) {
                    alert("Resource is no longer available");

                    this.router.navigateByUrl("/");
                }
            });
    }

    openPasswordDialog() {
        const dialogRef = this.dialog.open(PasswordForTokenDialogComponent, {
            disableClose: true,
            data: {
                token: this.token,
                password: ""
            }
        });

        return dialogRef.afterClosed();
    }

    reportResource(resourceId: string) {

        this.reportService.report(resourceId);
    }

    calculateSize() {

        if (this.actualFile) {
            if (this.actualFile.fileSizeBytes > Unit.TB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.TB) + " TB";
            }
            else if (this.actualFile.fileSizeBytes > Unit.GB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.GB) + " GB";
            }
            else if (this.actualFile.fileSizeBytes > Unit.MB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.MB) + " MB";
            }
            else if (this.actualFile.fileSizeBytes > Unit.kB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.kB) + " kB";
            }
            else {
                this.actualFile.fileSizeString = this.actualFile.fileSizeBytes + " byte";
            }
        }
    }
}
