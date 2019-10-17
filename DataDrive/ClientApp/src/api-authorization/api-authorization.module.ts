import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RouterModule } from '@angular/router';
import { ApplicationPaths } from './api-authorization.constants';
import { HttpClient, HttpClientModule } from '@angular/common/http';

import { MaterialModule } from '../material-module';

import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        RouterModule.forChild(
            [
                { path: ApplicationPaths.Register, component: LoginComponent },
                { path: ApplicationPaths.Profile, component: LoginComponent },
                { path: ApplicationPaths.Login, component: LoginComponent },
                { path: ApplicationPaths.LoginFailed, component: LoginComponent },
                { path: ApplicationPaths.LoginCallback, component: LoginComponent },
                { path: ApplicationPaths.LogOut, component: LogoutComponent },
                { path: ApplicationPaths.LoggedOut, component: LogoutComponent },
                { path: ApplicationPaths.LogOutCallback, component: LogoutComponent }
            ]
        ),
        MaterialModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
        FlexLayoutModule

    ],
    declarations: [LoginMenuComponent, LoginComponent, LogoutComponent],
    exports: [LoginMenuComponent, LoginComponent, LogoutComponent]
})
export class ApiAuthorizationModule { }

export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http);
}
