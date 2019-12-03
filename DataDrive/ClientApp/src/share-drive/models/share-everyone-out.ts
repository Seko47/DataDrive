import { ResourceType } from "../../files-drive/models/file-out";

export class ShareEveryoneOut {
    public id: string;
    public createdDateTime: Date;
    public lastModifiedDateTime: Date;
    public token: string;
    public expirationDateTime: Date;
    public downloadLimit: number;
    public resourceID: string;
    public resourceName: string;
    public resourceType: ResourceType;
    public ownerUsername: string;
}
