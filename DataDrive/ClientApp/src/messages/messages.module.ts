import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { MessagesComponent } from './components/messages/messages.component';
import { MessagesToolbarComponent } from './components/messages-toolbar/messages-toolbar.component';
import { MessageThreadsListComponent } from './components/message-threads-list/message-threads-list.component';
import { MessagesChatComponent } from './components/messages-chat/messages-chat.component';


@NgModule({
    declarations: [MessagesComponent, MessagesToolbarComponent, MessageThreadsListComponent, MessagesChatComponent],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'messages', component: MessagesComponent, canActivate: [AuthorizeGuard] },
            { path: 'messages/chat', component: MessagesChatComponent, canActivate: [AuthorizeGuard] }
        ])
    ]
})
export class MessagesModule { }
