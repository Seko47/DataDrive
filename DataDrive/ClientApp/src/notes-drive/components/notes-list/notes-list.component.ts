import { Component, OnInit } from '@angular/core';
import { NoteOut } from '../../models/note-out';
import { NotesService } from '../../services/notes.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-notes-list',
    templateUrl: './notes-list.component.html',
    styleUrls: ['./notes-list.component.css']
})
export class NotesListComponent implements OnInit {

    public notes: NoteOut[];

    constructor(private notesService: NotesService) {

        this.notesService.getAll().subscribe(result => {

            this.notes = result;
            this.notesService.getOffline().forEach(note => {

                this.notes.push(note);
            });
        }, (error: HttpErrorResponse) => {

            this.notesService.getOffline().forEach(note => {

                this.notes.push(note);
            });
        });
    }

    ngOnInit() {
    }

}
