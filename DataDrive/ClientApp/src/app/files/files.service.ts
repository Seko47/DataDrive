import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FileOut } from './models/file-out';
import { Observable } from 'rxjs';
import { getBaseUrl } from '../../main';
import { DirectoryOut } from './models/directory-out';

@Injectable({
    providedIn: 'root'
})
export class FilesService {
    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public getFilesFromDirectory(directoryID: string) {
        if (directoryID == null || directoryID.length == 0) {
            return this.httpClient.get<DirectoryOut>(this.baseUrl + "api/Files/fromRoot");
        }
        else {
            return this.httpClient.get<DirectoryOut>(this.baseUrl + 'api/Files/fromDirectory/' + directoryID);
        }
    }
}
