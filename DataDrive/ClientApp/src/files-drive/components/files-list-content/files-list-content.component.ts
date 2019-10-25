import { Component, OnInit, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { FileOut, FileType } from '../../models/file-out';
import { MatGridTile } from '@angular/material/grid-list';

@Component({
    selector: 'drive-files-list-content',
    templateUrl: './files-list-content.component.html',
    styleUrls: ['./files-list-content.component.css']
})
export class FilesListContentComponent implements OnInit {

    @Input() actualDirectory: DirectoryOut;

    @Output() onFileClick = new EventEmitter<FileOut>();

    public dragFile: FileOut;
    public overMatGridTile: HTMLDivElement;

    @HostListener('document:mouseup', ['$event'])
    onMouseUp(event: MouseEvent) {
        console.log("MouseEvent");
        if (this.dragFile) {
            this.dragFile = null;
        }

        if (this.overMatGridTile) {
            this.makeDirectoryNormal(this.overMatGridTile);
            this.overMatGridTile = null;
        }
    }

    constructor() { }

    ngOnInit() {
    }

    public clickFile(file: FileOut) {
        this.onFileClick.emit(file);
    }

    public drag(matGridTile: HTMLDivElement, file: FileOut) {
        this.dragFile = file;
    }

    public drop(file: FileOut) {
        if (this.dragFile) {
            if (file && file.fileType == FileType.DIRECTORY) {

                console.log("drop: " + file.id);
            }
        }

        if (this.overMatGridTile) {
            this.makeDirectoryNormal(this.overMatGridTile);
        }
        this.dragFile = null;
    }

    public enter(matGridTile: HTMLDivElement, file: FileOut) {
        if (this.dragFile) {
            if (this.overMatGridTile && (!matGridTile || this.overMatGridTile != matGridTile)) {
                this.makeDirectoryNormal(this.overMatGridTile);
            }

            if (matGridTile && file && file.fileType == FileType.DIRECTORY) {
                this.makeDirectoryDropable(matGridTile);
                this.overMatGridTile = matGridTile;
            }
        }
    }

    public leave() {
        if (this.overMatGridTile) {
            this.makeDirectoryNormal(this.overMatGridTile);
        }
    }

    private makeDirectoryDropable(matGridTile: HTMLDivElement) {
        matGridTile.classList.add("hoverWhenDraging");
        //matGridTile._setStyle("border", "3px outset coral");
    }

    private makeDirectoryNormal(matGridTile: HTMLDivElement) {
        matGridTile.classList.remove("hoverWhenDraging");
        //matGridTile._setStyle("border", "3px solid gray");
    }
}
