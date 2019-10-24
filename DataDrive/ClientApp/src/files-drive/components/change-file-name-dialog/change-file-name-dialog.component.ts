import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

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
        private dialogRef: MatDialogRef<ChangeFileNameDialogComponent>) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            directoryName: ''
        });
    }

    ngAfterViewInit(): void {
        this.inputName.nativeElement.select();
    }

    public onSubmit(form: FormGroup) {
        if (form.value.directoryName.length < 1) {
            return;
        }
        this.dialogRef.close(`${form.value.directoryName}`);
    }
}
