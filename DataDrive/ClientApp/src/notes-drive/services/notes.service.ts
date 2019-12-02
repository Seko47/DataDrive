import { Injectable, Inject } from '@angular/core';
import { NotePost } from '../models/note-post';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { NoteOut } from '../models/note-out';

@Injectable({
    providedIn: 'root'
})
export class NotesService {

    private baseUrl: string;

    private localStorageLocalNotesList: string = "localNotesList";
    private localStorageLocalNotesListOffline: string = "offlineNotesList";

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    public add(newNote: NotePost) {
        this.sync();

        this.post(newNote).subscribe(result => {

            console.log("Note added");
        }, (error: HttpErrorResponse) => {

            this.addOffline(newNote);
        });
    }

    private post(newNote: NotePost) {

        return this.httpClient.post<NoteOut>(this.baseUrl + 'api/Notes', newNote);
    }

    private addOffline(newNote: NotePost) {
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

    public sync() {
        const value: string = localStorage.getItem(this.localStorageLocalNotesListOffline)

        if (value) {
            var offlineList: NotePost[] = JSON.parse(value);

            localStorage.removeItem(this.localStorageLocalNotesListOffline);

            for (var i = 0; i < offlineList.length; ++i) {
                this.add(offlineList[i]);
            }
            console.log("Note sync");
        }
    }
}
