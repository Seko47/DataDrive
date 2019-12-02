import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core';
import { NoteOut } from '../../models/note-out';
import { NotesService } from '../../services/notes.service';
import { HttpErrorResponse } from '@angular/common/http';
import { EventService, EventCode } from '../../../files-drive/services/files-event.service';

@Component({
    selector: 'app-notes-list',
    templateUrl: './notes-list.component.html',
    styleUrls: ['./notes-list.component.css']
})
export class NotesListComponent implements OnInit {

    @Input("notes") notes: NoteOut[];

    constructor(private notesEventService: EventService) {
    }

    ngOnInit() {
    }

    public deleteNote(noteId: string) {

        this.notesEventService.emit([EventCode.DELETE, noteId]);
    }
}
