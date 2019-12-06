import { Component, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NotePost } from '../../models/note-post';
import { Router, ActivatedRoute } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { HttpErrorResponse } from '@angular/common/http';
import { NoteOut } from '../../models/note-out';
import { Operation, compare } from 'fast-json-patch';

@Component({
    selector: 'app-editor',
    templateUrl: './editor.component.html',
    styleUrls: ['./editor.component.css']
})
export class EditorComponent implements OnInit {

    editorConfig: AngularEditorConfig = {
        editable: true,
        spellcheck: true,
        height: '40vh',
        minHeight: '30vh',
        maxHeight: 'auto',
        width: 'auto',
        minWidth: '0',
        translate: 'yes',
        enableToolbar: true,
        showToolbar: true,
        placeholder: 'Enter text here...',
        defaultParagraphSeparator: '',
        defaultFontName: 'times-new-roman',
        defaultFontSize: '3',
        fonts: [
            { class: 'arial', name: 'Arial' },
            { class: 'times-new-roman', name: 'Times New Roman' },
            { class: 'calibri', name: 'Calibri' },
            { class: 'comic-sans-ms', name: 'Comic Sans MS' }
        ],
        uploadUrl: '',
        sanitize: true,
        toolbarPosition: 'bottom',
        toolbarHiddenButtons: [
            ['insertImage', 'insertVideo']
        ]
    };

    public newNote: NotePost;
    public oldNote: NotePost;
    public noteId: string;
    public mode: string;

    constructor(private router: Router, private notesService: NotesService, private activatedRoute: ActivatedRoute) {

        this.newNote = new NotePost();
        this.oldNote = new NotePost();

        this.activatedRoute.queryParams
            .subscribe(params => {
                this.mode = params.mode;
                if (this.mode === "new") {

                    console.log("New note");
                } else if (this.mode === "edit") {

                    var noteId = params.note;

                    this.notesService.getNoteById(noteId)
                        .subscribe((result: NoteOut) => {
                            this.newNote = new NotePost();

                            this.oldNote.title = result.title;
                            this.oldNote.content = result.content;
                            this.noteId = result.id;

                            this.newNote = JSON.parse(JSON.stringify(this.oldNote));
                        });
                } else if (this.mode === "read") {

                    var noteId = params.note;

                    this.notesService.getNoteById(noteId)
                        .subscribe((result: NoteOut) => {
                            this.newNote = new NotePost();

                            this.oldNote.title = result.title;
                            this.oldNote.content = result.content;
                            this.noteId = result.id;

                            this.newNote = JSON.parse(JSON.stringify(this.oldNote));

                            this.makeNotEditable();
                        });
                }
            });
    }

    ngOnInit() {
    }

    public saveNote(): void {

        if (!this.newNote.title && !this.newNote.content) {
            alert("Note is empty");
            return;
        }

        if (!this.noteId) {

            this.notesService.addNote(this.newNote).subscribe(result => {

                console.log("Note added");
                this.getBackToList();
            }, (error: HttpErrorResponse) => {

                alert(error.message);
                this.getBackToList();
            });
        }
        else {

            const patch: Operation[] = compare(this.oldNote, this.newNote);

            this.oldNote = new NotePost();

            this.notesService.editNote(this.noteId, patch).subscribe(result => {

                console.log("Note edited");

                if (this.mode === "read") {

                    this.makeNotEditable();
                } else {
                    this.getBackToList();
                }
            }, (error: HttpErrorResponse) => {

                alert(error.message);
                this.getBackToList();
            });
        }
    }

    public getBackToList(): void {
        this.router.navigateByUrl('/drive/notes');
    }

    public makeEditable(): void {

        this.editorConfig.editable = true;
        this.editorConfig.showToolbar = true;
        this.editorConfig.height = "40vh";
    }

    public makeNotEditable(): void {

        this.editorConfig.editable = false;
        this.editorConfig.showToolbar = false;
        this.editorConfig.height = "60vh";
    }
}
