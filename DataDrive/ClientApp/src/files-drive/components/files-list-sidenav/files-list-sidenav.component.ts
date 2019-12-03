import { Component, OnInit, Input, KeyValueDiffer, KeyValueDiffers, KeyValueChanges } from '@angular/core';
import { FileOut } from '../../models/file-out';
import { EventService, EventCode } from '../../services/files-event.service';
import { SharesService } from '../../../share-drive/services/shares.service';
import { ShareEveryoneOut } from '../../../share-drive/models/share-everyone-out';


@Component({
    selector: 'drive-files-list-sidenav',
    templateUrl: './files-list-sidenav.component.html',
    styleUrls: ['./files-list-sidenav.component.css']
})
export class FilesListSidenavComponent implements OnInit {

    @Input() actualFile: FileOut;
    private customerDiffer: KeyValueDiffer<string, any>;
    public shareEveryoneInfo: ShareEveryoneOut;

    public urlToShareEveryone: string = window.location.origin + "/share/";

    constructor(private differs: KeyValueDiffers, private filesEventService: EventService, private sharesService: SharesService) {
        this.actualFile = new FileOut();
        this.customerDiffer = this.differs.find(this.actualFile).create();
    }

    ngOnInit() {

        this.getShareInfo();
    }

    actualFileChanged(changes: KeyValueChanges<string, any>) {
        this.getShareInfo();
    }

    ngDoCheck(): void {
        const changes = this.customerDiffer.diff(this.actualFile);
        if (changes) {
            this.actualFileChanged(changes);
        }
    }


    public deleteFile() {

        this.filesEventService.emit([EventCode.DELETE, this.actualFile.id]);
    }

    public downloadFile() {

        this.filesEventService.emit([EventCode.DOWNLOAD, this.actualFile.id]);
    }

    public changeFileName() {

        this.filesEventService.emit([EventCode.RENAME, this.actualFile.id, this.actualFile.name]);
    }

    public shareFile() {

        this.filesEventService.emit([EventCode.SHARE, this.actualFile.id]);
    }

    public getShareInfo() {

        if (this.actualFile && this.actualFile.isShared) {

            this.getShareEveryoneInfo();
        }
    }

    public getShareEveryoneInfo() {

        if (this.actualFile && this.actualFile.isSharedForEveryone) {
            this.sharesService.getShareEveryoneInfo(this.actualFile.id)
                .subscribe((result: ShareEveryoneOut) => {
                    result.token = this.urlToShareEveryone + result.token;

                    this.shareEveryoneInfo = result;
                });
        }
    }
}
