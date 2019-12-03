import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FileOut } from '../models/file-out';
import { Observable } from 'rxjs';
import { DirectoryOut } from '../models/directory-out';
import { CreateDirectoryPost } from '../models/create-directory-post';
import { FileUploadResult } from '../models/file-upload-result';
import { Operation } from 'fast-json-patch';
import { ShareForEveryoneIn } from '../models/share-for-everyone-in';
import { ShareEveryoneOut } from '../models/share-everyone-out';


@Injectable({
    providedIn: 'root'
})
export class FilesService {

    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public deleteFile(id: string) {
        return this.httpClient.delete<FileOut>(this.baseUrl + 'api/Files/' + id);
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

    public updateFile(fileId: string, patch: Operation[]) {
        return this.httpClient.patch<FileOut>(this.baseUrl + 'api/Files/' + fileId, patch);
    }

    public downloadFile(id: string) {
        return this.httpClient.get(this.baseUrl + 'api/Files/download/' + id,
            { observe: "response", responseType: 'blob' });
    }

    public shareFileForEveryone(shareForEveryoneIn: ShareForEveryoneIn) {
        return this.httpClient.post<ShareEveryoneOut>(this.baseUrl + 'api/Share/everyone/share', shareForEveryoneIn);
    }

    public cancelShareFileForEveryone(fileId: string) {
        return this.httpClient.delete<boolean>(this.baseUrl + 'api/Share/everyone/' + fileId);
    }
}
