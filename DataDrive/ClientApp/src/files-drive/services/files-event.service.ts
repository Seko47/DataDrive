import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class EventService {

    private event = new BehaviorSubject<[EventCode, string, string?]>([EventCode.NONE, '']);

    public emit(message: [EventCode, string, string?]) {
        this.event.next(message);
    }

    public asObservable() {

        return this.event.asObservable();
    }

    public unsubscribe() {
        this.event.unsubscribe();
        this.event = new BehaviorSubject<[EventCode, string, string?]>([EventCode.NONE, '']);
    }
}

export enum EventCode {
    NONE = 0,
    RENAME = 1,
    DOWNLOAD = 2,
    DELETE = 3,
    SHARE = 4,
    EDIT = 5
}
