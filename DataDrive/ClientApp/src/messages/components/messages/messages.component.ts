import { Component, OnInit, OnDestroy } from '@angular/core';
import { ThreadOut } from '../../models/thread-out';
import { EventService, EventCode } from '../../../files-drive/services/files-event.service';
import { MessagesService } from '../../services/messages.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { map } from 'rxjs/operators';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit, OnDestroy {

    public threads: ThreadOut[];
    private loadthreadsInterval;
    private loggedUsername: string;


    constructor(private messagesService: MessagesService, private authorizeService: AuthorizeService) {
        this.threads = [];
        this.authorizeService.getUser()
            .pipe(map(u => u.name))
            .subscribe((username: string) => {

                this.loggedUsername = username;
            });

        this.loadThreads();
    }

    ngOnInit() {

        this.loadthreadsInterval = setInterval(() => {

            this.loadThreads();
        }, 1000);
    }

    ngOnDestroy(): void {

        clearInterval(this.loadthreadsInterval);
    }

    public loadThreads() {

        this.messagesService.getAllThreads().subscribe(result => {

            for (let i = 0; i < result.length; ++i) {

                for (let j = 0; j < result[i].messageThreadParticipants.length; ++j) {

                    if (result[i].messageThreadParticipants[j].userUsername !== this.loggedUsername) {

                        result[i].caller = result[i].messageThreadParticipants[j].userUsername;

                        break;
                    }
                }

                result[i].messages[0].isReaded = false;

                for (let j = 0; j < result[i].messages[0].messageReadStates.length; ++j) {

                    if (result[i].messages[0].messageReadStates[j].userUsername === this.loggedUsername) {

                        result[i].messages[0].isReaded = true;
                        break;
                    }
                }

            }

            if (JSON.stringify(this.threads) !== JSON.stringify(result)) {

                this.threads = result;
            }

        }, (error: HttpErrorResponse) => {

            console.log(error.error);
        });
    }
}
