import { Component, OnInit, AfterViewInit, ViewChild, ElementRef, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-change-file-name-dialog',
    templateUrl: './change-file-name-dialog.component.html',
    styleUrls: ['./change-file-name-dialog.component.css']
})
export class ChangeFileNameDialogComponent implements OnInit, AfterViewInit {

    @ViewChild('inputName', null) inputName: ElementRef;

    public form: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private dialogRef: MatDialogRef<ChangeFileNameDialogComponent>,
        @Inject(MAT_DIALOG_DATA) private data) { }

    ngOnInit() {

        this.form = this.formBuilder.group({
            fileName: this.data.filename
        });
    }

    ngAfterViewInit(): void {

        this.inputName.nativeElement.setSelectionRange(0, this.inputName.nativeElement.value.lastIndexOf('.'));
    }

    public onSubmit(form: FormGroup) {

        form.value.fileName = form.value.fileName.trim();

        if (form.value.fileName.length < 1) {
            return;
        }
        this.dialogRef.close(`${form.value.fileName}`);
    }
}
