import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { FilesComponent } from './components/files/files.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { FilesListContentComponent } from './components/files-list-content/files-list-content.component';
import { FilesListSidenavComponent } from './components/files-list-sidenav/files-list-sidenav.component';
import { CreateDirectoryDialogComponent } from './components/create-directory-dialog/create-directory-dialog.component';


@NgModule({
    declarations: [FilesComponent, ToolbarComponent, FilesListContentComponent, FilesListSidenavComponent, CreateDirectoryDialogComponent],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'drive/files', component: FilesComponent, canActivate: [AuthorizeGuard] },
        ])
    ],
    entryComponents: [CreateDirectoryDialogComponent]
})
export class FilesDriveModule { }
