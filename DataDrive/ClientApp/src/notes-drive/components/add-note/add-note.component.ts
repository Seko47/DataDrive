import { Component, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Router } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { NotePost } from '../../models/note-post';

@Component({
    selector: 'app-add-note',
    templateUrl: './add-note.component.html',
    styleUrls: ['./add-note.component.css']
})
export class AddNoteComponent implements OnInit {

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
        defaultFontName: '',
        defaultFontSize: '',
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

    constructor(private router: Router, private notesService: NotesService) {
        this.newNote = new NotePost();
    }

    ngOnInit() {
    }

    public saveNote(): void {
        this.notesService.add(this.newNote);
        this.getBackToList();
    }

    public getBackToList(): void {
        this.router.navigateByUrl('/drive/notes');
    }
}
