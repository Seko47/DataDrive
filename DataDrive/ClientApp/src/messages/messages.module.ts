import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { ThreadsComponent } from './components/threads/threads.component';


@NgModule({
    declarations: [ThreadsComponent],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'threads', component: ThreadsComponent, canActivate: [AuthorizeGuard] }
        ])
    ]
})
export class MessagesModule { }
