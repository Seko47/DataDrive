import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-create-directory-dialog',
    templateUrl: './create-directory-dialog.component.html',
    styleUrls: ['./create-directory-dialog.component.css']
})
export class CreateDirectoryDialogComponent implements OnInit, AfterViewInit {

    @ViewChild('inputName', null) inputName: ElementRef;

    public form: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private dialogRef: MatDialogRef<CreateDirectoryDialogComponent>,
        private translate: TranslateService) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            directoryName: this.translate.instant('FILES.CREATE_DIRECTORY_DIALOG.NAME_INPUT')
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
