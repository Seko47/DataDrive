import { Component, OnInit, OnDestroy } from '@angular/core';
import { EventService, EventCode } from '../../../files-drive/services/files-event.service';
import { NotesService } from '../../services/notes.service';
import { HttpErrorResponse } from '@angular/common/http';
import { NoteOut } from '../../models/note-out';
import { MatDialog } from '@angular/material/dialog';
import { ShareResourceDialogComponent } from '../../../share-drive/components/share-resource-dialog/share-resource-dialog.component';
import { ResourceType } from '../../../files-drive/models/file-out';

@Component({
    selector: 'app-notes',
    templateUrl: './notes.component.html',
    styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit, OnDestroy {

    public notes: NoteOut[];

    constructor(private dialog: MatDialog, private notesEventService: EventService, private notesService: NotesService) {
        this.loadNotes();
    }

    ngOnInit() {

        this.notesEventService.asObservable().subscribe((message: [EventCode, string, string?]) => {

            const eventCode = message[0];
            const noteId = message[1];

            switch (eventCode) {

                case EventCode.DELETE: {

                    this.deleteNote(noteId);
                    break;
                }
                case EventCode.SHARE: {

                    this.openShareNoteDialog(noteId);

                    break;
                }
            }
        });
    }

    ngOnDestroy(): void {
        this.notesEventService.unsubscribe();
    }

    public openShareNoteDialog(noteId: string) {

        this.notesService.getNoteById(noteId)
            .subscribe(result => {

                const note: NoteOut = result;

                if (note) {
                    const dialogRef = this.dialog.open(ShareResourceDialogComponent, {
                        hasBackdrop: true,
                        data: {
                            note: note,
                            resourceType: ResourceType.NOTE
                        }
                    });

                    dialogRef.afterClosed().subscribe(result => {
                        this.loadNotes();
                    }, err => alert(err.error));
                }
            });
    }

    public loadNotes() {
        this.notes = [];

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

            console.log(error.error);
        });
    }

    public deleteNote(id: string) {

        this.notesService.deleteNote(id)
            .subscribe(result => {

                console.log("Note " + result + " deleted");
                this.loadNotes();
            }, (err: HttpErrorResponse) => alert(err.message));
    }
}
