import { Component, OnInit } from '@angular/core';
import { DirectoryOut } from './models/directory-out';
import { FilesService } from './files.service';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

    public actualDirectory: DirectoryOut;

    constructor(private filesService: FilesService) {
        this.actualDirectory = null;
    }

    ngOnInit() {
        this.filesService
            .getFilesFromDirectory(this.actualDirectory != null ? this.actualDirectory.ID : null)
            .subscribe(result => {
                this.actualDirectory = result;
            });
    }

}
