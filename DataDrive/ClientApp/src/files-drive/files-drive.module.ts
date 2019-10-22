import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilesComponent } from './files/files.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';


@NgModule({
    declarations: [FilesComponent],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'drive/files', component: FilesComponent, canActivate: [AuthorizeGuard] },
        ])
    ]
})
export class FilesDriveModule { }
