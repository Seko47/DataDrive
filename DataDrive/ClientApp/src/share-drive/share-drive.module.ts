import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ShareEveryoneComponent } from './components/share-everyone/share-everyone.component';
import { FilesDriveModule } from '../files-drive/files-drive.module';

@NgModule({
    declarations: [ShareEveryoneComponent],
    imports: [
        CommonModule,
        SharedModule,
        FilesDriveModule,
        RouterModule.forChild([
            { path: 'share/:token', component: ShareEveryoneComponent },
        ])
    ]
})
export class ShareDriveModule { }
