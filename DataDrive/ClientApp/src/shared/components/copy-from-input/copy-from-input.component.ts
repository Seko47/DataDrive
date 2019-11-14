import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-copy-from-input',
  templateUrl: './copy-from-input.component.html',
  styleUrls: ['./copy-from-input.component.css']
})
export class CopyFromInputComponent {

    @Input("inputField") inputField: HTMLInputElement;

  constructor() { }

    copyText() {

        this.inputField.select();
        document.execCommand('copy');
    }

}
