import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent {
    title = 'app';

    constructor(private translate: TranslateService) {
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
}
