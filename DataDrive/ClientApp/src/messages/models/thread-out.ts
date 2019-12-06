import { MessageOut } from "./message-out";
import { MessageThreadParticipantOut } from "./message-thread-participant-out";

export class ThreadOut {

    public id: string;

    public messages: MessageOut[];
    public messageThreadParticipants: MessageThreadParticipantOut[]
}
