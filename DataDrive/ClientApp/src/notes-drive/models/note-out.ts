import { ResourceType } from "../../files-drive/models/file-out";

export class NoteOut {
    public id: string;
    public createdDateTime: Date;
    public lastModifiedDateTime: Date;

    public isShared: boolean;
    public isSharedForEveryone: boolean;
    public isSharedForUsers: boolean;

    public title: string;
    public content: string;
    public resourceType: ResourceType;

    constructor(title: string, content: string) {
        this.title = title;
        this.content = content;
        this.createdDateTime = new Date();
        this.resourceType = ResourceType.NOTE;
    }
}
