import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { FilesComponent } from './components/files/files.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';


@NgModule({
    declarations: [FilesComponent, ToolbarComponent],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'drive/files', component: FilesComponent, canActivate: [AuthorizeGuard] },
        ])
    ]
})
export class FilesDriveModule { }
