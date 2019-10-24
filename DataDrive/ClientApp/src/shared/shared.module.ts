import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiAuthorizationModule } from '../api-authorization/api-authorization.module';
import { MaterialModule } from '../material-module';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { AuthorizeInterceptor } from '../api-authorization/authorize.interceptor';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        ApiAuthorizationModule,
        HttpClientModule,
        MaterialModule,
        FlexLayoutModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
    ],
    exports: [
        CommonModule,
        ApiAuthorizationModule,
        HttpClientModule,
        MaterialModule,
        FlexLayoutModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        TranslateModule
    ]
})
export class SharedModule { }

export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http);
}
