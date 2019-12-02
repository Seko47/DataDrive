import { Component, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
    selector: 'app-add-note',
    templateUrl: './add-note.component.html',
    styleUrls: ['./add-note.component.css']
})
export class AddNoteComponent implements OnInit {

    editorConfig: AngularEditorConfig = {
        editable: true,
        spellcheck: true,
        height: '70vh',
        minHeight: '0',
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

    constructor() { }

    ngOnInit() {
    }

}
