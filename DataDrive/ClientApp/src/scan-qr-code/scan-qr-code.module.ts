import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { QrCodeScannerComponent } from './qr-code-scanner/qr-code-scanner.component';

@NgModule({
    declarations: [QrCodeScannerComponent],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'scan', component: QrCodeScannerComponent },
        ])
    ]
})
export class ScanQrCodeModule { }
