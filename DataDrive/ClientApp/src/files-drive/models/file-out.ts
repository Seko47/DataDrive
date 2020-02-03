export class FileOut {
    public id: string;
    public createdDateTime: Date;
    public lastModifiedDateTime: Date;
    public parentDirectoryID: string;
    public parentDirectoryName: string;
    public name: string;
    public resourceType: ResourceType;
    public isShared: boolean;
    public isSharedForEveryone: boolean;
    public isSharedForUsers: boolean;
    public fileSizeBytes: number;
    public fileSizeString: string;
}

export enum ResourceType {
    FILE,
    DIRECTORY,
    NOTE
}
