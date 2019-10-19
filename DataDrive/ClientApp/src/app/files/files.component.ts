import { Component, OnInit } from '@angular/core';
import { DirectoryOut } from './models/directory-out';
import { FilesService } from './files.service';
import { CreateDirectoryPost } from './models/create-directory-post';
import { Observable } from 'rxjs';
import { FileType } from './models/file-out';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

    public actualDirectory: DirectoryOut;
    public newDirectory: CreateDirectoryPost;

    constructor(private filesService: FilesService) {
        this.actualDirectory = null;

        this.newDirectory = new CreateDirectoryPost();
    }

    ngOnInit() {
        this.getFromDirectory();
    }

    public getFromDirectory() {
        let getFromDirectoryId: string = null;
        if (this.actualDirectory != null) {
            getFromDirectoryId = this.actualDirectory.id;
        }

        console.log("files.component.ts: getFromDirectoryId == " + getFromDirectoryId)

        this.filesService
            .getFilesFromDirectory(getFromDirectoryId)
            .subscribe(result => {
                console.log("files.component.ts: dirName == " + result.name);
                console.log("files.component.ts: filesInDir == " + result.files.length);
                this.actualDirectory = result;
            }, err => alert(err.error));
    }

    public createDirectory() {
        this.filesService.createDirectory(this.newDirectory)
            .subscribe(result => {
                console.log("files.component.ts: new directory created")

                this.newDirectory = new CreateDirectoryPost();
            }, err => alert(err.error));
    }
}
