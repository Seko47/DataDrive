import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FilesEventService {

    private event = new BehaviorSubject<[FilesEventCode, string]>([FilesEventCode.NONE, '']);

    public emit(message: [FilesEventCode, string]) {
        this.event.next(message);
    }

    public asObservable() {
        return this.event.asObservable();
    }
}

export enum FilesEventCode {
    NONE,
    RENAME,
    DOWNLOAD,
    DELETE
}
