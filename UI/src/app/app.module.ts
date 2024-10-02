import {importProvidersFrom, NgModule, isDevMode} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StarterComponent } from './starter/starter.component';
import { CreateComponent } from './todo/create/create.component';
import { EditComponent } from './todo/edit/edit.component';
import { HomeComponent } from './home/home.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { SplashPageComponent } from './splash-page/splash-page.component';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatDialogModule} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatIconModule} from "@angular/material/icon";
import { NavbarComponent } from './navbar/navbar.component';
import { FilterHeroComponent } from './filter-hero/filter-hero.component';
import { RatingComponent } from './rating/rating.component';
import {MatButtonModule} from "@angular/material/button";
import { GroupCarouselComponent } from './group-carousel/group-carousel.component';
import { DrawerComponent } from './drawer/drawer.component';
import {HttpClientModule} from "@angular/common/http";
import {ApiModule, Configuration, ConfigurationParameters} from "./TodoService";
import {TodoTableComponent} from "./todo-table/todo-table.component";
import {CountdownComponent} from "./countdown/countdown.component";
import { ServiceWorkerModule } from '@angular/service-worker';
import {ToastComponent} from "./toast/toast.component";
import {AddGroupModalComponent} from "./add-group-modal/add-group-modal.component";
import {environment} from "../environments/environment";
import {GroupPageComponent} from "./group-page/group-page.component";
import { MtxDatetimepickerModule } from '@ng-matero/extensions/datetimepicker';
import {DatetimeSelectorComponent} from "./datetime-selector/datetime-selector.component";

export function apiConfigFactory(): Configuration {
  const params: ConfigurationParameters = {
    basePath: environment.apiUrl,
  };
  return new Configuration(params);
}
@NgModule({


  declarations: [
    AppComponent,
    StarterComponent,
    CreateComponent,
    EditComponent,
    HomeComponent,
    PageNotFoundComponent,
    SplashPageComponent,
    LoginModalComponent,
    NavbarComponent,
    FilterHeroComponent,
    RatingComponent,
    GroupCarouselComponent,
    DrawerComponent,
    ToastComponent,
    AddGroupModalComponent,
    GroupPageComponent,
    TodoTableComponent,
    CountdownComponent
  ],
  //entryComponents: [LoginModalComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    HttpClientModule,
    MtxDatetimepickerModule,
    ApiModule.forRoot(apiConfigFactory),
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: !isDevMode(),
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
    DatetimeSelectorComponent,
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports: [
    RatingComponent
  ]
})
export class AppModule { }
