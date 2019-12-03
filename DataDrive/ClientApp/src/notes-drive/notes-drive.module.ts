import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { NotesComponent } from './components/notes/notes.component';
import { NotesToolbarComponent } from './components/notes-toolbar/notes-toolbar.component';
import { NotesListComponent } from './components/notes-list/notes-list.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { ContextMenuModule } from 'ngx-contextmenu';
import { EditorComponent } from './components/editor/editor.component';
import { ShareResourceDialogComponent } from '../share-drive/components/share-resource-dialog/share-resource-dialog.component';

@NgModule({
    declarations: [NotesComponent, NotesToolbarComponent, NotesListComponent, EditorComponent],
    imports: [
        CommonModule,
        SharedModule,
        ContextMenuModule.forRoot(),
        AngularEditorModule,
        RouterModule.forChild([
            { path: 'drive/notes', component: NotesComponent, canActivate: [AuthorizeGuard] },
            { path: 'drive/notes/editor', component: EditorComponent, canActivate: [AuthorizeGuard] },
        ])
    ],
    entryComponents: [ShareResourceDialogComponent]
})
export class NotesDriveModule { }
