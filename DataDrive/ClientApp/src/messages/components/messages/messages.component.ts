import { Component, OnInit, OnDestroy } from '@angular/core';
import { ThreadOut } from '../../models/thread-out';
import { EventService, EventCode } from '../../../files-drive/services/files-event.service';
import { MessagesService } from '../../services/messages.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

    public threads: ThreadOut[];

    constructor(private messagesService: MessagesService) {

        this.loadThreads();
    }

    ngOnInit() {
    }

    public loadThreads() {

        this.messagesService.getAllThreads().subscribe(result => {

            this.threads = result;

            if (!this.threads) {

                this.threads = [];
            }

        }, (error: HttpErrorResponse) => {

                console.log(error.error);
        });
    }
}
