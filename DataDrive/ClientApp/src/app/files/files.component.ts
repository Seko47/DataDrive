import { Component, OnInit } from '@angular/core';
import { DirectoryOut } from './models/directory-out';
import { FilesService } from './files.service';
import { CreateDirectoryPost } from './models/create-directory-post';
import { Observable } from 'rxjs';
import { FileType } from './models/file-out';
import { FilePost } from './models/file-post';

@Component({
    selector: 'app-files',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

    public actualDirectory: DirectoryOut;
    public newDirectory: CreateDirectoryPost;

    public progress: number;
    public message: string;

    constructor(private filesService: FilesService) {
        this.actualDirectory = new DirectoryOut();
        this.actualDirectory.id = null;
        this.actualDirectory.name = "Root";

        this.newDirectory = new CreateDirectoryPost();
    }

    ngOnInit() {
        this.getFromDirectory(null);
    }

    public getFromDirectory(id: string) {

        console.log("files.component.ts: getFromDirectoryId == " + id)

        this.filesService
            .getFilesFromDirectory(id)
            .subscribe(result => {
                console.log("files.component.ts: dirName == " + result.name);
                console.log("files.component.ts: filesInDir == " + result.files.length);
                this.actualDirectory = result;
            }, err => alert(err.error));
    }

    public getBack() {

        console.log("files.component.ts: getBackTo " + this.actualDirectory.parentDirectoryName);

        this.getFromDirectory(this.actualDirectory.parentDirectoryID);
    }

    public createDirectory() {
        this.newDirectory.parentDirectoryID = this.actualDirectory.id
        this.filesService.createDirectory(this.newDirectory)
            .subscribe(result => {
                console.log("files.component.ts: new directory created")
                this.newDirectory = new CreateDirectoryPost();
                this.getFromDirectory(result);
            }, err => alert(err.error));
    }

    public upload(files) {
        if (files.length === 0) {
            return;
        }

        const formData = new FormData();
        
        formData.append("parentDirectoryID", this.actualDirectory.id);

        formData.append("files", files);

        console.log("files.component.ts:upload(files) | files="+files);
        console.log("files.component.ts:upload(files) | files.length="+files.length);

        this.filesService.uploadFiles(formData)
            .subscribe(result => {
                result.forEach(r => alert("suc"+r.name));
                this.getFromDirectory(this.actualDirectory.id);
            }, err => alert(err.error));

        /*const formData = new FormData();

        for (let file of files) {
            formData.append(file.name, file);
        }

        const uploadReq = new HttpRequest('POST', `api/upload`, formData, {
            reportProgress: true,
        });

        this.http.request(uploadReq).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress)
                this.progress = Math.round(100 * event.loaded / event.total);
            else if (event.type === HttpEventType.Response)
                this.message = event.body.toString();
        });*/
    }

    stopPropagation(event) {
        event.stopPropagation();
    }
}
