export class FileOut {
    public ID: string;
    public CreatedDateTime: string;
    public LastModifiedDateTime: string;
    public ParentDirectoryID: string;
    public ParentDirectoryName: string;
    public Name: string;
    public FileType: FileType;
}

export enum FileType {
    FILE,
    DIRECTORY,
    NOTE
}
