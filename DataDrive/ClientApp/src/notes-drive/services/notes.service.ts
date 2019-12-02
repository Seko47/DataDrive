import { Injectable, Inject } from '@angular/core';
import { NotePost } from '../models/note-post';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { NoteOut } from '../models/note-out';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class NotesService {

    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public getAllNotes() {

        return this.httpClient.get<NoteOut[]>(this.baseUrl + 'api/Notes');
    }

    public addNote(newNote: NotePost): Observable<NoteOut> {

        return this.httpClient.post<NoteOut>(this.baseUrl + 'api/Notes', newNote);
    }

    public deleteNote(noteId: string) {
        return this.httpClient.delete<string>(this.baseUrl + 'api/Notes/' + noteId);
    }
}
