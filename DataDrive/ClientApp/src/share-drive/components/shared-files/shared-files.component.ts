import { Component, OnInit, ViewChild } from '@angular/core';
import { FileOut, ResourceType } from '../../../files-drive/models/file-out';
import { ShareForUserOut } from '../../models/share-for-user-out';
import { SharesService } from '../../services/shares.service';
import { ShareFilter } from '../../models/share-filter';
import { HttpErrorResponse } from '@angular/common/http';
import { FilesService } from '../../../files-drive/services/files.service';
import { MatSidenav } from '@angular/material/sidenav';

@Component({
    selector: 'app-shared-files',
    templateUrl: './shared-files.component.html',
    styleUrls: ['./shared-files.component.css']
})
export class SharedFilesComponent implements OnInit {

    @ViewChild('fileinfosidenav', null) fileinfosidenav: MatSidenav;

    public shareForUserOuts: ShareForUserOut[];
    public actualFile: FileOut;
    public actualFileShareInfo: ShareForUserOut;

    constructor(private sharesService: SharesService, private filesService: FilesService) {

        this.loadFiles();
    }

    ngOnInit() {
    }

    public loadFiles() {

        this.sharesService.getShareForUsersByUser(new ShareFilter(ResourceType.FILE))
            .subscribe(result => {

                if (this.shareForUserOuts && result) {

                    if (JSON.stringify(this.shareForUserOuts) === JSON.stringify(result)) {

                        return;
                    }
                }

                this.shareForUserOuts = result

            }, (err: HttpErrorResponse) => console.log(err.error));
    }

    public clickFile(resource: ShareForUserOut) {

        this.actualFileShareInfo = resource;

        this.filesService.getFileInfo(resource.resourceID)
            .subscribe(result => {

                this.actualFile = result;
                this.fileinfosidenav.toggle();
            }, (err: HttpErrorResponse) => alert(err.error));
    }

    public downloadFile(resourceId: string) {

        //TODO pobierz plik
    }
}
