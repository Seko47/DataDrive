import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-notes-toolbar',
    templateUrl: './notes-toolbar.component.html',
    styleUrls: ['./notes-toolbar.component.css']
})
export class NotesToolbarComponent implements OnInit {

    constructor(private router: Router) { }

    ngOnInit() {
    }

    public addNote(): void {
        this.router.navigate(["/drive/notes/editor"], { queryParams: { mode: "new" } });
    }
}
