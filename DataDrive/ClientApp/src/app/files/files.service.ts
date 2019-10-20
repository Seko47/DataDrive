import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FileOut } from './models/file-out';
import { Observable } from 'rxjs';
import { getBaseUrl } from '../../main';
import { DirectoryOut } from './models/directory-out';
import { CreateDirectoryPost } from './models/create-directory-post';
import { FilePost } from './models/file-post';
import { FileUploadResult } from './models/file-upload-result';

@Injectable({
    providedIn: 'root'
})
export class FilesService {
    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public getFileInfo(fileId: string) {
        return this.httpClient.get<FileOut>(this.baseUrl + 'api/Files/' + fileId);
    }

    public getFilesFromDirectory(directoryID: string): Observable<DirectoryOut> {
        if (directoryID == null || directoryID.length == 0) {
            console.log("files.service.ts: directoryID == null or empty");
            return this.httpClient.get<DirectoryOut>(this.baseUrl + "api/Files/fromRoot");
        }
        else {
            console.log("files.service.ts: directoryID == " + directoryID);
            return this.httpClient.get<DirectoryOut>(this.baseUrl + 'api/Files/fromDirectory/' + directoryID);
        }
    }

    public createDirectory(directory: CreateDirectoryPost) {
        return this.httpClient.post<string>(this.baseUrl + 'api/Files/createDirectory', directory);
    }

    public uploadFiles(formData: FormData) {
        return this.httpClient.post<FileUploadResult[]>(this.baseUrl + 'api/Files', formData,
            {
                reportProgress: true,
                observe: 'events',
                withCredentials: true
            });
    }
}
