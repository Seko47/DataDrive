export class FileOut {
    public id: string;
    public createdDateTime: Date;
    public lastModifiedDateTime: Date;
    public parentDirectoryID: string;
    public parentDirectoryName: string;
    public name: string;
    public fileType: FileType;
    public isShared: boolean;
    public isSharedForEveryone: boolean;
    public isSharedForUsers: boolean;
}

export enum FileType {
    FILE,
    DIRECTORY,
    NOTE
}
