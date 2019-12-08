import { MessageReadStateOut } from "./message-read-state-out";

export class MessageOut {

    public id: string;

    public content: string;
    public sentDate: Date;
    public sendingUserID: string;
    public sendingUserUsername: string;
    public threadID: string;

    public showDate: boolean;
    public isReaded: boolean;

    public messageReadStates: MessageReadStateOut[];
}
