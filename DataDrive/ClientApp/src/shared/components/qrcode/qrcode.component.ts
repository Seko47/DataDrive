import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'qrcode',
  templateUrl: './qrcode.component.html',
  styleUrls: ['./qrcode.component.css']
})
export class QrcodeComponent implements OnInit {

    @Input("text") qrCodeText: string;

  constructor() { }

  ngOnInit() {
  }

}
