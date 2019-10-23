import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DirectoryOut } from '../../models/directory-out';
import { FileOut } from '../../models/file-out';

@Component({
    selector: 'drive-files-list-content',
    templateUrl: './files-list-content.component.html',
    styleUrls: ['./files-list-content.component.css']
})
export class FilesListContentComponent implements OnInit {

    @Input() actualDirectory: DirectoryOut;

    @Output() onFileClick = new EventEmitter<FileOut>();

    constructor() { }

    ngOnInit() {
    }

    public clickFile(file: FileOut) {
        this.onFileClick.emit(file);
    }
}
