import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FilesEventService {

    private event = new BehaviorSubject<[FilesEventCode, string, string?]>([FilesEventCode.NONE, '']);

    public emit(message: [FilesEventCode, string, string?]) {
        this.event.next(message);
    }

    public asObservable() {

        return this.event.asObservable();
    }

    public unsubscribe() {
        this.event.unsubscribe();
        this.event = new BehaviorSubject<[FilesEventCode, string, string?]>([FilesEventCode.NONE, '']);
    }
}

export enum FilesEventCode {
    NONE = 0,
    RENAME = 1,
    DOWNLOAD = 2,
    DELETE = 3,
    SHARE = 4
}
