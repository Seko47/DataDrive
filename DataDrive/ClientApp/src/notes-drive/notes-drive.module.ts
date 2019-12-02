import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { NotesComponent } from './components/notes/notes.component';
import { AddNoteComponent } from './components/add-note/add-note.component';
import { NotesToolbarComponent } from './components/notes-toolbar/notes-toolbar.component';
import { NotesListComponent } from './components/notes-list/notes-list.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { ContextMenuModule } from 'ngx-contextmenu';

@NgModule({
  declarations: [NotesComponent, AddNoteComponent, NotesToolbarComponent, NotesListComponent],
  imports: [
      CommonModule,
      SharedModule,
      ContextMenuModule.forRoot(),
      AngularEditorModule,
      RouterModule.forChild([
          { path: 'drive/notes', component: NotesComponent, canActivate: [AuthorizeGuard] },
          { path: 'drive/notes/add', component: AddNoteComponent, canActivate: [AuthorizeGuard] },
      ])
  ]
})
export class NotesDriveModule { }
