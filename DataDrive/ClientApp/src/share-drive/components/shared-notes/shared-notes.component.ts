import { Component, OnInit } from '@angular/core';
import { ShareForUserOut } from '../../models/share-for-user-out';
import { SharesService } from '../../services/shares.service';
import { ShareFilter } from '../../models/share-filter';
import { ResourceType } from '../../../files-drive/models/file-out';
import { HttpErrorResponse } from '@angular/common/http';
import { NotesService } from '../../../notes-drive/services/notes.service';
import { NoteOut } from '../../../notes-drive/models/note-out';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
    selector: 'app-shared-notes',
    templateUrl: './shared-notes.component.html',
    styleUrls: ['./shared-notes.component.css']
})
export class SharedNotesComponent implements OnInit {

    public shareForUserOuts: [ShareForUserOut, NoteOut][];

    constructor(private router: Router, private sharesService: SharesService, private notesService: NotesService) {

        this.loadNotes();
    }

    ngOnInit() {
    }

    public loadNotes() {

        this.sharesService.getShareForUsersByUser(new ShareFilter(ResourceType.NOTE))
            .subscribe(result => {

                if (this.shareForUserOuts && result) {

                    if (JSON.stringify(this.shareForUserOuts.map(s => s[0])) === JSON.stringify(result)) {

                        return;
                    }
                }


                this.shareForUserOuts = [];
                let temp: [ShareForUserOut, NoteOut][] = [];
                var div = document.createElement("div");
                result.forEach(share => {

                    this.notesService.getNoteById(share.resourceID)
                        .subscribe(note => {

                            if (note.content) {
                                note.content = note.content.replace("<div>", "\n");
                                note.content = note.content.replace("</div>", "\n");
                            }
                            div.innerHTML = note.content;
                            note.content = div.textContent;

                            temp.push([share, note]);

                            this.shareForUserOuts = temp;
                        });
                });

            }, (err: HttpErrorResponse) => console.log(err.error));
    }

    public showNote(resourceId: string) {

        this.router.navigate(["/shared/notes/editor"], { queryParams: { mode: "read", note: resourceId } });
    }
}
