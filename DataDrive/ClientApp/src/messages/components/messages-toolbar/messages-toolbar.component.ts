import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-messages-toolbar',
    templateUrl: './messages-toolbar.component.html',
    styleUrls: ['./messages-toolbar.component.css']
})
export class MessagesToolbarComponent implements OnInit {

    constructor(private router: Router) { }

    ngOnInit() {
    }

    public sendMessage(): void {
        this.router.navigate(["/messages/chat"], { queryParams: { mode: "new" } });
    }
}
