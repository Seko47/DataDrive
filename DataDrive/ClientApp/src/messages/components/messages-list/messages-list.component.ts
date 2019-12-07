import { Component, OnInit, Input, ViewChild, OnDestroy } from '@angular/core';
import { MessageOut } from '../../models/message-out';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { EventService, EventCode } from '../../../files-drive/services/files-event.service';

@Component({
    selector: 'app-messages-list',
    templateUrl: './messages-list.component.html',
    styleUrls: ['./messages-list.component.css']
})
export class MessagesListComponent implements OnInit, OnDestroy {

    @Input("messages") messages: MessageOut[];
    @ViewChild("messageListContainer", null) messageListContainer: HTMLDivElement;

    private loadMessagesInterval: NodeJS.Timeout;
    private updateScrollInterval: NodeJS.Timeout;

    private loggedUsername: Observable<string>;


    constructor(private authorizeService: AuthorizeService, private messageEventService: EventService) {

        this.loggedUsername = this.authorizeService.getUser().pipe(map(u => u.name));
    }

    ngOnInit() {

        this.messageListContainer.scrollTop = this.messageListContainer.scrollHeight;

        this.messageEventService.asObservable().subscribe((message: [EventCode, string, string?]) => {

            const eventCode = message[0];
            const value = message[1];

            if (value === 'from_chat')
                switch (eventCode) {

                    case EventCode.RELOAD: {

                        this.updateScroll();
                        break;
                    }
                }
        });

        this.loadMessagesInterval = setInterval(() => {

            this.messageEventService.emit([EventCode.RELOAD, "to_chat"]);

        }, 1500);

        this.updateScrollInterval = setInterval(() => {

            this.updateScroll();
        }, 1000);
    }

    ngOnDestroy(): void {

        clearInterval(this.loadMessagesInterval);
        clearInterval(this.updateScrollInterval);
    }

    public updateScroll() {
        const isScrolledToBottom = (this.messageListContainer.scrollHeight - this.messageListContainer.clientHeight) <= this.messageListContainer.scrollTop + 1

        if (isScrolledToBottom) {
            this.messageListContainer.scrollTop = this.messageListContainer.scrollHeight - this.messageListContainer.clientHeight;
        }
    }

}
