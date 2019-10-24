import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileOut } from '../../models/file-out';

@Component({
    selector: 'drive-files-list-sidenav',
    templateUrl: './files-list-sidenav.component.html',
    styleUrls: ['./files-list-sidenav.component.css']
})
export class FilesListSidenavComponent implements OnInit {

    @Input() actualFile: FileOut;

    @Output() onFileDelete = new EventEmitter<string>();

    constructor() { }

    ngOnInit() {
    }

    public deleteFile() {
        this.onFileDelete.emit(this.actualFile.id);
    }
}
