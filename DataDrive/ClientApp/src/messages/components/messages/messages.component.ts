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

            if (this.threads && this.threads[0] && this.threads[0].messages && this.threads[0].messages[0]) {
                if (this.threads.length === result.length
                    && this.threads[0].id === result[0].id
                    && this.threads[0].messages[0].id === result[0].messages[0].id) {

                    return;
                }
            }
            alert(JSON.stringify(result[0].messages[0]));
            for (let i = 0; i < result.length; ++i) {
                console.log("i = " + i)
                if (result[i].messages[0].messageReadStates.length > 1) {

                    result[i].messages[0].showReaded = true;
                }
                else {
                    result[i].messages[0].showReaded = false;
                }

                console.log("readed =" + result[i].messages[0].showReaded);


                for (let j = 0; j < result[i].messageThreadParticipants.length; ++j) {

                    if (result[i].messageThreadParticipants[j].userUsername !== this.loggedUsername) {

                        result[i].caller = result[i].messageThreadParticipants[j].userUsername;
                        console.log(result[i].caller);
                        console.log("caller =" + result[i].caller);

                        break;
                    }
                }
            }

            this.threads = result;

        }, (error: HttpErrorResponse) => {

            console.log(error.error);
        });
    }
}
