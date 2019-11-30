import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { NotesComponent } from './components/notes/notes.component';

@NgModule({
  declarations: [NotesComponent],
  imports: [
      CommonModule,
      SharedModule,
      RouterModule.forChild([
          { path: 'drive/notes', component: NotesComponent, canActivate: [AuthorizeGuard] },
      ])
  ]
})
export class NotesDriveModule { }
