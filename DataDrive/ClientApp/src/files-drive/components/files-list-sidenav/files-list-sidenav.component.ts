import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileOut } from '../../models/file-out';
import { ChangeFileNameDialogComponent } from '../change-file-name-dialog/change-file-name-dialog.component';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs/operators';
import { compare, Operation } from 'fast-json-patch';
import { FilesEventService, FilesEventCode } from '../../services/files-event.service';


@Component({
    selector: 'drive-files-list-sidenav',
    templateUrl: './files-list-sidenav.component.html',
    styleUrls: ['./files-list-sidenav.component.css']
})
export class FilesListSidenavComponent implements OnInit {

    @Input() actualFile: FileOut;

    constructor(private filesEventService: FilesEventService) {
    }

    ngOnInit() {
    }

    public deleteFile() {

        this.filesEventService.emit([FilesEventCode.DELETE, this.actualFile.id]);
    }

    public downloadFile() {

        this.filesEventService.emit([FilesEventCode.DOWNLOAD, this.actualFile.id]);
    }

    public changeFileName() {

        this.filesEventService.emit([FilesEventCode.RENAME, this.actualFile.id, this.actualFile.name]);
    }

    public shareFile() {

        this.filesEventService.emit([FilesEventCode.SHARE, this.actualFile.id]);
    }
}
