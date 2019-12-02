import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
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

        this.notesService.getAllNotes().subscribe(result => {

            this.notes = result;
            if (!this.notes) {
                this.notes = [];
            }

            this.notes.forEach(note => {
                var div = document.createElement("div");
                if (note.content) {
                    note.content = note.content.replace("<div>", "\n");
                    note.content = note.content.replace("</div>", "\n");
                }
                div.innerHTML = note.content;
                note.content = div.textContent;
            });
        }, (error: HttpErrorResponse) => {

            alert(error.message);
        });
    }

    ngOnInit() {
    }

}
