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
    public overMatGridTile: MatGridTile;

    @HostListener('document:mouseup', ['$event'])
    onMouseUp(event: MouseEvent) {
        console.log("MouseEvent");
        if (this.dragFile) {
            this.dragFile = null;
        }

        if (this.overMatGridTile) {
            this.overMatGridTile._setStyle("border", "0");
            this.overMatGridTile = null;
        }
    }

    constructor() { }

    ngOnInit() {
    }

    public clickFile(file: FileOut) {
        this.onFileClick.emit(file);
    }

    public drag(matGridTile: MatGridTile, file: FileOut) {
        this.dragFile = file;
        console.log("drag: " + file.id);
    }

    public drop(file: FileOut) {
        if (this.dragFile) {
            if (file && file.fileType == FileType.DIRECTORY) {

                console.log("drop: " + file.id);
            }
        }

        if (this.overMatGridTile) {
            this.overMatGridTile._setStyle("border", "0");
        }
        this.dragFile = null;
    }

    public over(matGridTile: MatGridTile, file: FileOut) {
        if (this.dragFile) {
            if (this.overMatGridTile && (!matGridTile || this.overMatGridTile != matGridTile)) {
                this.overMatGridTile._setStyle("border", "0");
            }

            if (matGridTile && file && file.fileType == FileType.DIRECTORY) {
                console.log("over: " + file.id);
                matGridTile._setStyle("border", "5px solid blue");
                this.overMatGridTile = matGridTile;
            }
        }
    }
}
