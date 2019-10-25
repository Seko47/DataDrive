import { Operation } from "fast-json-patch";

export class FileMove {
    public fileId: string;
    public patch: Operation[];
}
