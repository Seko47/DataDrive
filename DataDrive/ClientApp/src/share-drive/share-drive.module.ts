import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ShareEveryoneComponent } from './components/share-everyone/share-everyone.component';
import { FilesDriveModule } from '../files-drive/files-drive.module';
import { PasswordForTokenDialogComponent } from './components/password-for-token-dialog/password-for-token-dialog.component';
import { ShareResourceDialogComponent } from './components/share-resource-dialog/share-resource-dialog.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { SharedFilesComponent } from './components/shared-files/shared-files.component';
import { ContextMenuModule } from 'ngx-contextmenu';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { SharedNotesComponent } from './components/shared-notes/shared-notes.component';

@NgModule({
    declarations: [ShareEveryoneComponent, PasswordForTokenDialogComponent, ShareResourceDialogComponent, SharedFilesComponent, SharedNotesComponent],
    imports: [
        CommonModule,
        SharedModule,
        FilesDriveModule,
        AngularEditorModule,
        ContextMenuModule.forRoot(),
        RouterModule.forChild([
            { path: 'share/:token', component: ShareEveryoneComponent },
            { path: 'shared/files', component: SharedFilesComponent, canActivate: [AuthorizeGuard] },
            { path: 'shared/notes', component: SharedNotesComponent, canActivate: [AuthorizeGuard] },
        ])
    ],
    entryComponents: [PasswordForTokenDialogComponent]
})
export class ShareDriveModule { }
