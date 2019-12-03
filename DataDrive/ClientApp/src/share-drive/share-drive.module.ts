import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ShareEveryoneComponent } from './components/share-everyone/share-everyone.component';
import { FilesDriveModule } from '../files-drive/files-drive.module';
import { PasswordForTokenDialogComponent } from './components/password-for-token-dialog/password-for-token-dialog.component';
import { ShareResourceDialogComponent } from './components/share-resource-dialog/share-resource-dialog.component';
import { AngularEditorModule } from '@kolkov/angular-editor';

@NgModule({
    declarations: [ShareEveryoneComponent, PasswordForTokenDialogComponent, ShareResourceDialogComponent],
    imports: [
        CommonModule,
        SharedModule,
        FilesDriveModule,
        AngularEditorModule,
        RouterModule.forChild([
            { path: 'share/:token', component: ShareEveryoneComponent },
        ])
    ],
    entryComponents: [PasswordForTokenDialogComponent]
})
export class ShareDriveModule { }
