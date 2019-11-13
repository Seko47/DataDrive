import { FileType } from "./file-out";

export class ShareEveryoneOut {
    public id: string;
    public createdDateTime: Date;
    public lastModifiedDateTime: Date;
    public token: string;
    public expirationDateTime: Date;
    public downloadLimit: number;
    public fileID: string;
    public fileName: string;
    public fileType: FileType;
    public ownerUsername: string;
}
