import { FileOut } from "./file-out";
import { Observable } from "rxjs";

export class DirectoryOut extends FileOut {
    public Files: Observable<FileOut[]>;
}
