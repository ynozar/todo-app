import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {CreateComponent} from "./todo/create/create.component";
import {EditComponent} from "./todo/edit/edit.component";
import {StarterComponent} from "./starter/starter.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";
import {HomeComponent} from "./home/home.component";
import {SplashPageComponent} from "./splash-page/splash-page.component";
import {GroupPageComponent} from "./group-page/group-page.component";


const routes: Routes = [
  { path: 'todo/create', component: CreateComponent },
  { path: 'todo/:id', component: EditComponent },
  { path: 'starter', component: StarterComponent },
  { path: 'group/:id', component: GroupPageComponent },
  { path: 'group', component: GroupPageComponent },
  { path: 'home', component: HomeComponent },
  { path: '', component: SplashPageComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
