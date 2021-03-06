import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ShareEveryoneCredentials } from '../models/share-everyone-credentials';
import { ShareEveryoneOut } from '../models/share-everyone-out';
import { ShareForEveryoneIn } from '../models/share-for-everyone-in';
import { ShareForUserOut } from '../models/share-for-user-out';
import { ShareForUserIn } from '../models/share-for-user-in';
import { ShareFilter } from '../models/share-filter';

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





    public getShareForUsersByUser(shareFilter: ShareFilter) {
        return this.httpClient.get<ShareForUserOut[]>(this.baseUrl + 'api/Share/forUser', {
            params: new HttpParams()
                .set('ResourceType',
                    shareFilter.resourceType
                        .toString())
        });
    }

    public getShareForUsersInfo(id: string) {
        return this.httpClient.get<ShareForUserOut[]>(this.baseUrl + 'api/Share/forUser/info/' + id);
    }

    public shareForUser(shareForUserIn: ShareForUserIn) {
        return this.httpClient.post<ShareForUserOut>(this.baseUrl + 'api/Share/forUser', shareForUserIn);
    }

    public cancelShareForUser(id: string) {
        return this.httpClient.delete<boolean>(this.baseUrl + 'api/Share/forUser/' + id);
    }
}
