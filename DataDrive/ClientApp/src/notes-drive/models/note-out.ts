import { FileType } from "../../files-drive/models/file-out";

export class NoteOut {
    public id: string;
    public createdDateTime: string;
    public lastModifiedDateTime: Date;

    public isShared: boolean;
    public isSharedForEveryone: boolean;
    public isSharedForUsers: boolean;

    public title: string;
    public content: string;
    public fileType: FileType;
}
