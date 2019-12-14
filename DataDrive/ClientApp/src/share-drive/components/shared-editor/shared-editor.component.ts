import { Component, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { HttpErrorResponse } from '@angular/common/http';
import { NoteOut } from '../../../notes-drive/models/note-out';
import { Router, ActivatedRoute } from '@angular/router';
import { NotesService } from '../../../notes-drive/services/notes.service';

@Component({
    selector: 'app-shared-editor',
    templateUrl: './shared-editor.component.html',
    styleUrls: ['./shared-editor.component.css']
})
export class SharedEditorComponent implements OnInit {

    editorConfig: AngularEditorConfig = {
        editable: false,
        spellcheck: false,
        height: '40vh',
        minHeight: '30vh',
        maxHeight: 'auto',
        width: 'auto',
        minWidth: '0',
        translate: 'yes',
        enableToolbar: false,
        showToolbar: false,
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

    public note: NoteOut;

    constructor(private router: Router, private notesService: NotesService, private activatedRoute: ActivatedRoute) {

        this.note = new NoteOut("", "");

        this.activatedRoute.queryParams
            .subscribe(params => {

                var noteId = params.note;

                this.notesService.getNoteById(noteId)
                    .subscribe((result: NoteOut) => {

                        this.note = result;
                    });
            });
    }

    ngOnInit() {
    }

    public getBackToList(): void {
        this.router.navigateByUrl('/shared/notes');
    }
}
