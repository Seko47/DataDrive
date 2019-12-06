import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ThreadOut } from '../models/thread-out';

@Injectable({
    providedIn: 'root'
})
export class MessagesService {

    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public getAllThreads() {

        return this.httpClient.get<ThreadOut[]>(this.baseUrl + 'threads');
    }

}
