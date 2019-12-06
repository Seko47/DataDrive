import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ThreadOut } from '../../models/thread-out';
import { MessagesService } from '../../services/messages.service';
import { MessageFilter } from '../../models/message-filter';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageThreadParticipantOut } from '../../models/message-thread-participant-out';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MessagePost } from '../../models/message-post';

@Component({
    selector: 'app-messages-chat',
    templateUrl: './messages-chat.component.html',
    styleUrls: ['./messages-chat.component.css']
})
export class MessagesChatComponent implements OnInit {

    private mode: string;
    private thread: ThreadOut;
    private messageFilter: MessageFilter;
    private messagePost: MessagePost;

    constructor(private authorizeService: AuthorizeService, private router: Router, private activatedRoute: ActivatedRoute, private messagesService: MessagesService) {

        this.thread = new ThreadOut();
        this.messageFilter = new MessageFilter(10);
        this.messagePost = new MessagePost();

        this.activatedRoute.queryParams
            .subscribe(params => {

                this.mode = params.mode;
                if (this.mode && this.mode === "new") {

                    console.log("New message");
                } else if (this.mode === "read") {

                    let threadId = params.thread;

                    if (!threadId) {
                        this.getBackToList();
                        return;
                    }

                    this.getMessagesFromThread(threadId);
                }
            }, error => {

                this.getBackToList();
            });
    }

    ngOnInit() {
    }

    public sendMessage() {

        this.messagePost.toUserUsername = this.messagePost.toUserUsername.trim();
        this.messagePost.content = this.messagePost.content.trim();

        if (!this.messagePost.toUserUsername || this.messagePost.toUserUsername.length < 1
            || !this.messagePost.content || this.messagePost.content.length < 1) {

            return;
        }

        this.messagesService.sendMessage(this.messagePost)
            .subscribe(result => {

                ++this.messageFilter.numberOfLastMessage;
                this.messagePost.content = '';
                if (this.mode === 'new') {

                    this.mode = 'read';
                }

                this.getMessagesFromThread(result.threadID);
            }, (error: HttpErrorResponse) => {

                alert(error.error);
            });
    }

    public getMessagesFromThread(threadId: string) {

        this.messagesService.getMessagesFromThread(threadId, this.messageFilter)
            .subscribe((result: ThreadOut) => {

                this.thread = result;
                let loggedUser: Observable<string> = this.authorizeService.getUser().pipe(map(u => u && u.name));

                loggedUser.subscribe(username => {

                    let messageThreadParticipant: MessageThreadParticipantOut = this.thread.messageThreadParticipants
                        .find(participant => participant.userUsername !== username);

                    this.messagePost.toUserUsername = messageThreadParticipant.userUsername;
                });
            }, (error: HttpErrorResponse) => {

                console.log(error.error);
                if (error.status === 415) {
                    this.getBackToList();
                }
            });
    }

    public getBackToList() {
        this.router.navigateByUrl('/messages');
    }
}
