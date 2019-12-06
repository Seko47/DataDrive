import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { ThreadOut } from '../models/thread-out';
import { MessageFilter } from '../models/message-filter';
import { MessagePost } from '../models/message-post';
import { MessageOut } from '../models/message-out';

@Injectable({
    providedIn: 'root'
})
export class MessagesService {

    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public getAllThreads() {

        return this.httpClient.get<ThreadOut[]>(this.baseUrl + 'api/Messages/threads');
    }

    public getMessagesFromThread(threadId: string, messageFilter: MessageFilter) {
        return this.httpClient.get<ThreadOut>(this.baseUrl + 'api/Messages/threads/' + threadId,
            {
                params: new HttpParams()
                    .set('NumberOfLastMessage',
                        messageFilter.numberOfLastMessage
                            .toString())
            });
    }

    public sendMessage(messagePost: MessagePost) {
        return this.httpClient.post<MessageOut>(this.baseUrl + 'api/Messages', messagePost);
    }
}
