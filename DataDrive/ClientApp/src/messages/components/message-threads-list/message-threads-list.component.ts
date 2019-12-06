import { Component, OnInit, Input } from '@angular/core';
import { ThreadOut } from '../../models/thread-out';

@Component({
    selector: 'app-message-threads-list',
    templateUrl: './message-threads-list.component.html',
    styleUrls: ['./message-threads-list.component.css']
})
export class MessageThreadsListComponent implements OnInit {

    @Input('threads') threads: ThreadOut[];

    constructor() { }

    ngOnInit() {
    }

}
