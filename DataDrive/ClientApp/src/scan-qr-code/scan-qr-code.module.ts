import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { QrCodeScannerComponent } from './qr-code-scanner/qr-code-scanner.component';
import { ZXingScannerModule } from '@zxing/ngx-scanner';

@NgModule({
    declarations: [QrCodeScannerComponent],
    imports: [
        CommonModule,
        SharedModule,
        ZXingScannerModule,
        RouterModule.forChild([
            { path: 'scan', component: QrCodeScannerComponent },
        ])
    ]
})
export class ScanQrCodeModule { }
