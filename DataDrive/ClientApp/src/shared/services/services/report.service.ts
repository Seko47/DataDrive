import { Injectable, Inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SnackBarService } from './snack-bar.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class ReportService {

    private baseUrl: string;

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string, private translate: TranslateService, private snackBarService: SnackBarService) {

        this.baseUrl = baseUrl;
    }

    public report(resourceId: string) {

        if (localStorage.getItem("reported_" + resourceId)) {

            this.snackBarService.openSnackBar(this.translate.instant('REPORT.RESOURCE_ALREADY_REPORTED_BY_YOU'), "X", 2500);
            return;
        }

        this.httpClient.get<boolean>(this.baseUrl + "api/Share/report/" + resourceId)
            .subscribe(result => {

                if (result) {

                    localStorage.setItem("reported_" + resourceId, "true");

                    this.snackBarService.openSnackBar(this.translate.instant('REPORT.RESOURCE_REPORTED'), "X", 2500);
                }
                else {

                    this.snackBarService.openSnackBar(this.translate.instant('FILES.UPLOAD_FILE.SOMETHING_WENT_WRONG'), "X", 2500);
                }
            }, (err: HttpErrorResponse) => alert(JSON.stringify(err)));
    }
}
