import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ShareEveryoneOut } from '../../files-drive/models/share-everyone-out';
import { Observable } from 'rxjs';
import { ShareEveryoneCredentials } from '../models/share-everyone-credentials';

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
}
