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

    private localStorageLocalNotesList: string = "localNotesListCache";
    private localStorageLocalNotesListOffline: string = "offlineNotesList";

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public getAllOnlineNotes() {

        return this.httpClient.get<NoteOut[]>(this.baseUrl + 'api/Notes');
    }

    public addOnlineNote(newNote: NotePost): Observable<NoteOut> {

        return this.httpClient.post<NoteOut>(this.baseUrl + 'api/Notes', newNote);
    }

    public addOfflineNote(newNote: NotePost) {
        const value: string = localStorage.getItem(this.localStorageLocalNotesListOffline)
        var offlineList: NotePost[];

        if (value) {
            offlineList = JSON.parse(value);
        }
        else {
            offlineList = [];
        }

        offlineList.push(newNote);

        localStorage.setItem(this.localStorageLocalNotesListOffline, JSON.stringify(offlineList));

        console.log("Note add to offline")
    }

    public getOffline(): NoteOut[] {
        const value: string = localStorage.getItem(this.localStorageLocalNotesListOffline)
        var offlineList: NotePost[];

        if (value) {
            offlineList = JSON.parse(value);
        }
        else {
            offlineList = [];
        }

        var notes: NoteOut[];

        for (var i = 0; i < offlineList.length; ++i) {
            notes.push(new NoteOut(offlineList[i].title, offlineList[i].content));
        }

        return notes;
    }


    public sync() {
        const value: string = localStorage.getItem(this.localStorageLocalNotesListOffline)

        if (value) {
            var offlineList: NotePost[] = JSON.parse(value);

            localStorage.removeItem(this.localStorageLocalNotesListOffline);

            for (var i = 0; i < offlineList.length; ++i) {
                this.addOnlineNote(offlineList[i]);
            }
            console.log("Note sync");
        }
    }
}
