import { Component, OnInit, ViewChild, EventEmitter } from '@angular/core';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
import { Router } from '@angular/router';

@Component({
    selector: 'app-qr-code-scanner',
    templateUrl: './qr-code-scanner.component.html',
    styleUrls: ['./qr-code-scanner.component.css']
})
export class QrCodeScannerComponent {

    public systemURL: string = window.location.origin;

    @ViewChild('qrcodescanner', null) qrCodeScanner: ZXingScannerComponent;

    private scannerEnabled: boolean;
    private qrCodeText: string = null;

    constructor(private router: Router) { }

    public camerasFoundHandler(event: MediaDeviceInfo[]) {

        this.scannerEnabled = true;
    }

    public camerasNotFoundHandler(event) {

        this.scannerEnabled = false;
        this.router.navigateByUrl("/");
    }

    public scanSuccessHandler(event) {

        this.scannerEnabled = false;
        this.qrCodeText = event;

        if (this.isSystemURL()) {

            this.router.navigateByUrl(this.qrCodeText.substring(this.systemURL.length));
        }
        else {


        }
    }

    public scanErrorHandler(event) {

    }

    public scanFailureHandler(event) {

    }

    public scanCompleteHandler(event) {

    }

    private isSystemURL() {

        return this.qrCodeText.startsWith(this.systemURL);
    }
}
