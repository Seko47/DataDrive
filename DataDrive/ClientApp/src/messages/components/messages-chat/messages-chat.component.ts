import { Component, OnInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
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
import { EventService, EventCode } from '../../../files-drive/services/files-event.service';
import { MessageOut } from '../../models/message-out';

@Component({
    selector: 'app-messages-chat',
    templateUrl: './messages-chat.component.html',
    styleUrls: ['./messages-chat.component.css']
})
export class MessagesChatComponent implements OnInit, OnDestroy {

    private mode: string;
    private thread: ThreadOut;
    private messageFilter: MessageFilter;
    private messagePost: MessagePost;
    private loggedUsername: Observable<string>;
    private isMoore: boolean = true;

    @ViewChild("messageListContainer", null) messageListContainer: HTMLDivElement;
    private loadMessagesInterval;


    constructor(private cdref: ChangeDetectorRef, private authorizeService: AuthorizeService, private router: Router, private activatedRoute: ActivatedRoute, private messagesService: MessagesService, private messageEventService: EventService) {

        this.loggedUsername = this.authorizeService.getUser().pipe(map(u => u.name));

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

        this.messageEventService.asObservable().subscribe((message: [EventCode, string, string?]) => {

            const eventCode = message[0];
            const value = message[1];
            if (value == 'to_chat')
                switch (eventCode) {

                    case EventCode.RELOAD: {

                        this.getMessagesFromThread(this.thread.id);
                        break;
                    }
                }
        });

        this.loadMessagesInterval = setInterval(() => {

            this.getMessagesFromThread(this.thread.id);
        }, 1000);
    }

    ngOnDestroy(): void {
        this.messageEventService.unsubscribe();
        clearInterval(this.loadMessagesInterval);
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

                if (this.thread && this.thread.messages && result && result.messages) {
                    if (result.messages.length < 1
                        || (result.messages.length == this.thread.messages.length)
                        && result.messages[result.messages.length - 1].id === this.thread.messages[this.thread.messages.length - 1].id
                        && result.messages[0].id === this.thread.messages[0].id) {

                        return;
                    }
                }

                if (result.messages.length < this.messageFilter.numberOfLastMessage) {
                    this.isMoore = false;
                }
                else {
                    this.isMoore = true;
                }

                this.thread = result;
                this.cdref.detectChanges();

                let loggedUser: Observable<string> = this.authorizeService.getUser().pipe(map(u => u && u.name));

                this.messageEventService.emit([EventCode.RELOAD, "from_chat"]);

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

    public loadMore() {
        this.messageFilter.numberOfLastMessage += 10;

        this.getMessagesFromThread(this.thread.id);
    }

    public getBackToList() {
        this.router.navigateByUrl('/messages');
    }
}
