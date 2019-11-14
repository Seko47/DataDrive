import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

import { DriveComponent } from './drive/drive.component';
import { FilesDriveModule } from '../files-drive/files-drive.module';
import { SharedModule } from '../shared/shared.module';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { ShareDriveModule } from '../share-drive/share-drive.module';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        CounterComponent,
        FetchDataComponent,
        DriveComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        SharedModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'drive', component: DriveComponent, pathMatch: 'full', canActivate: [AuthorizeGuard] },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
        ]),
        FilesDriveModule,
        ShareDriveModule
    ],
    bootstrap: [AppComponent],
})
export class AppModule { }
