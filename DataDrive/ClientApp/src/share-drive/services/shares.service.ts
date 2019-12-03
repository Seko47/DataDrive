import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ShareEveryoneOut } from '../../files-drive/models/share-everyone-out';
import { Observable } from 'rxjs';
import { ShareEveryoneCredentials } from '../models/share-everyone-credentials';
import { ShareForEveryoneIn } from '../../files-drive/models/share-for-everyone-in';

@Injectable({
    providedIn: 'root'
})
export class SharesService {
    
    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public getShareByToken(token: string): Observable<ShareEveryoneOut> {

        return this.httpClient.get<ShareEveryoneOut>(this.baseUrl + 'api/Share/everyone/get/' + token);
    }

    public getShareByTokenAndPassword(shareEveryoneCredentials: ShareEveryoneCredentials): Observable<ShareEveryoneOut> {

        return this.httpClient.post<ShareEveryoneOut>(this.baseUrl + 'api/Share/everyone', {
            'token': shareEveryoneCredentials.token,
            'password': shareEveryoneCredentials.password
        });
    }

    public getShareEveryoneInfo(id: string) {
        return this.httpClient.get<ShareEveryoneOut>(this.baseUrl + 'api/Share/everyone/info/' + id);
    }

    public cancelShareForEveryone(id: string) {
        return this.httpClient.delete<boolean>(this.baseUrl + 'api/Share/everyone/' + id);
    }

    public shareForEveryone(shareForEveryoneIn: ShareForEveryoneIn) {
        return this.httpClient.post<ShareEveryoneOut>(this.baseUrl + 'api/Share/everyone/share', shareForEveryoneIn);
    }
}
