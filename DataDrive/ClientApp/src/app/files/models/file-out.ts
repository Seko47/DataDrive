export class FileOut {
    public id: string;
    public createdDateTime: string;
    public lastModifiedDateTime: string;
    public parentDirectoryID: string;
    public parentDirectoryName: string;
    public name: string;
    public fileType: FileType;
}

export enum FileType {
    FILE,
    DIRECTORY,
    NOTE
}
