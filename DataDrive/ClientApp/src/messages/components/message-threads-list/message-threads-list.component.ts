import { Component, OnInit, Input } from '@angular/core';
import { ThreadOut } from '../../models/thread-out';
import { Router } from '@angular/router';

@Component({
    selector: 'app-message-threads-list',
    templateUrl: './message-threads-list.component.html',
    styleUrls: ['./message-threads-list.component.css']
})
export class MessageThreadsListComponent implements OnInit {

    @Input('threads') threads: ThreadOut[];

    constructor(private router: Router) {

    }

    ngOnInit() {
    }

    public showThread(threadId: string) {

        this.router.navigateByUrl('/messages/chat?mode=read&thread=' + threadId);
    }
}
//Wysyłanie wiadomości, żeby móc zobaczyć listę wątków i listę wiadomości
//po kliknięciu wątku
