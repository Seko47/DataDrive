import { Component, OnInit, Input, Output, EventEmitter, HostListener, AfterViewInit } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { FileOut, ResourceType } from '../../models/file-out';
import { FileMove } from '../../models/file-move';
import { Operation, compare } from 'fast-json-patch';
import { EventService, EventCode } from '../../services/files-event.service';
import { UserDiskSpace } from '../../models/user-disk-space';

@Component({
    selector: 'drive-files-list-content',
    templateUrl: './files-list-content.component.html',
    styleUrls: ['./files-list-content.component.css']
})
export class FilesListContentComponent implements OnInit {

    @Input() actualDirectory: DirectoryOut;
    @Input() userDiskSpace: UserDiskSpace;

    @Output() onFileClick = new EventEmitter<FileOut>();

    @Output() onFileMove = new EventEmitter<FileMove>();

    public parentFile: FileOut;
    public dragFile: FileOut;
    public dropDivElement: HTMLDivElement;

    @HostListener('document:mouseup', ['$event'])
    onMouseUp(event: MouseEvent) {
        if (this.dragFile) {
            this.dragFile = null;
        }

        if (this.dropDivElement) {
            this.makeDirectoryNormal(this.dropDivElement);
            this.dropDivElement = null;
        }
    }

    constructor(private filesEventService: EventService) {
    }

    ngOnInit() {
    }

    public downloadFile(fileId: string) {

        this.filesEventService.emit([EventCode.DOWNLOAD, fileId]);
    }

    public deleteFile(fileId: string) {

        this.filesEventService.emit([EventCode.DELETE, fileId]);
    }

    public changeFileName(fileId: string, oldFileName: string) {

        this.filesEventService.emit([EventCode.RENAME, fileId, oldFileName]);
    }

    public shareFile(fileId: string) {

        this.filesEventService.emit([EventCode.SHARE, fileId]);
    }

    public clickFile(file: FileOut) {
        this.onFileClick.emit(file);
    }

    public drag(divElement: HTMLDivElement, file: FileOut) {
        if (this.actualDirectory) {
            this.parentFile = new FileOut();
            this.parentFile.id = this.actualDirectory.parentDirectoryID;
            this.parentFile.resourceType = ResourceType.DIRECTORY;
            this.parentFile.name = this.actualDirectory.parentDirectoryName;
        }

        this.dragFile = file;
    }

    public drop(file: FileOut) {
        if (this.dragFile && this.dragFile != file) {
            if (file && file.resourceType == ResourceType.DIRECTORY) {

                const movedFile: FileOut = JSON.parse(JSON.stringify(this.dragFile));
                movedFile.parentDirectoryID = file.id;

                const patch: Operation[] = compare(this.dragFile, movedFile);

                const fileMove: FileMove = new FileMove();
                fileMove.fileId = movedFile.id;
                fileMove.patch = patch;

                this.onFileMove.emit(fileMove);
            }
        }

        if (this.dropDivElement) {
            this.makeDirectoryNormal(this.dropDivElement);
        }
        this.dragFile = null;
    }

    public enter(divElement: HTMLDivElement, file: FileOut) {
        if (this.dragFile) {
            if (this.dropDivElement && (!divElement || this.dropDivElement != divElement)) {
                this.makeDirectoryNormal(this.dropDivElement);
            }

            if (divElement && file && file.resourceType == ResourceType.DIRECTORY) {
                this.makeDirectoryDropable(divElement);
                this.dropDivElement = divElement;
            }
        }
    }

    public leave() {
        if (this.dropDivElement) {
            this.makeDirectoryNormal(this.dropDivElement);
        }
    }

    private makeDirectoryDropable(divElement: HTMLDivElement) {
        divElement.classList.add("hoverWhenDraging");
    }

    private makeDirectoryNormal(divElement: HTMLDivElement) {
        divElement.classList.remove("hoverWhenDraging");
    }

    public showMessage(message: string) {
        alert(message);
    }

    public isNotDirectory(item: FileOut): boolean {
        return item.resourceType != 1;
    }
}
