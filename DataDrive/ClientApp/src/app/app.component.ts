import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PwaService } from './services/pwa.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    providers: [PwaService]
})
export class AppComponent {
    title = 'DataDrive';

    constructor(private translate: TranslateService, public pwaService: PwaService) {
        let language = 'en';
        if (localStorage.getItem("application_language")) {
            language = localStorage.getItem("application_language");
        }
        this.translate.setDefaultLang(language);
    }

    public useLanguage(language: string) {
        localStorage.setItem("application_language", language);
        this.translate.use(language);
    }

    public installPwa(): void {
        this.pwaService.promptInstallEvent.prompt();
    }
}
