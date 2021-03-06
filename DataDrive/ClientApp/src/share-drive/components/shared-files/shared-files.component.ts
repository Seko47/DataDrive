import { Component, OnInit, ViewChild } from '@angular/core';
import { FileOut, ResourceType } from '../../../files-drive/models/file-out';
import { ShareForUserOut } from '../../models/share-for-user-out';
import { SharesService } from '../../services/shares.service';
import { ShareFilter } from '../../models/share-filter';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FilesService } from '../../../files-drive/services/files.service';
import { MatSidenav } from '@angular/material/sidenav';
import { saveAs } from 'file-saver';
import { ReportService } from '../../../shared/services/services/report.service';
import { Unit } from '../../../files-drive/models/user-disk-space';

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

    constructor(private reportService: ReportService, private sharesService: SharesService, private filesService: FilesService) {

        this.loadFiles();
    }
    //TODO refresh when new shared
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
                this.calcualateSize();
                this.fileinfosidenav.toggle();
            }, (err: HttpErrorResponse) => alert(err.error));
    }

    public downloadFile(resourceId: string) {

        this.filesService.downloadFile(resourceId)
            .subscribe((result: HttpResponse<Blob>) => {
                if (this.fileinfosidenav) {
                    this.fileinfosidenav.close();
                }

                let fileName = "download";
                if (result.headers.has("content-disposition")) {

                    let contentDisposition = result.headers.get("content-disposition");
                    const startIndex = contentDisposition.indexOf("filename=") + 9;
                    contentDisposition = contentDisposition.substr(startIndex);

                    const endIndex = contentDisposition.indexOf(';');
                    let rawFileName = contentDisposition.substring(0, endIndex);

                    if (rawFileName.startsWith('"') && rawFileName.endsWith('"')) {
                        rawFileName = rawFileName.substring(1, rawFileName.length - 1);
                    }

                    fileName = rawFileName;
                }

                saveAs(result.body, fileName);
            }, err => console.log(err.error));
    }

    reportResource(resourceId: string) {

        this.reportService.report(resourceId);
    }

    calcualateSize() {

        if (this.actualFile) {
            if (this.actualFile.fileSizeBytes > Unit.TB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.TB) + " TB";
            }
            else if (this.actualFile.fileSizeBytes > Unit.GB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.GB) + " GB";
            }
            else if (this.actualFile.fileSizeBytes > Unit.MB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.MB) + " MB";
            }
            else if (this.actualFile.fileSizeBytes > Unit.kB) {

                this.actualFile.fileSizeString = (this.actualFile.fileSizeBytes / Unit.kB) + " kB";
            }
            else {
                this.actualFile.fileSizeString = this.actualFile.fileSizeBytes + " byte";
            }
        }
    }
}
