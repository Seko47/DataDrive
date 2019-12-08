import { ResourceType } from "../../files-drive/models/file-out";

export class ShareForUserOut {

    public id: string;
    public createdDateTime: Date;
    public lastModifiedDateTime: Date;

    public sharedForUserID: string;
    public sharedForUserUsername: string;

    public expirationDateTime: Date;
    public resourceID: string;
    public resourceName: string;
    public resourceType: ResourceType;

    public ownerUsername: string;
}
