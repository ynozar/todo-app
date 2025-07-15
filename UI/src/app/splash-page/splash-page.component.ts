import {Component, inject, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {LoginModalComponent} from "../login-modal/login-modal.component";
import {GroupsService, HealthService, ToDosService, UsersService} from "../TodoService";
import {firstValueFrom, Observable} from "rxjs";

@Component({
  selector: 'app-splash-page',
  templateUrl: './splash-page.component.html',
  styleUrls: ['./splash-page.component.css']
})
export class SplashPageComponent implements OnInit {
  public showLogin:boolean=true;
  constructor(private dialog: MatDialog,private router: Router) {}

  healthService: HealthService = inject(HealthService)


   async wakeBackend(): Promise<string> {

     try{
       await firstValueFrom(this.healthService.healthGet());}
     catch (e:any){
       //Always throws error since it doesn't like json
       this.healthService.healthGet()
       this.healthService.healthGet()
       this.healthService.healthGet()
       return e.error.text
     }
     return "Status Unknown"
  }
  ngOnInit(): void {
    const t= Date.now();
    this.wakeBackend().then(r =>

      console.log(`Backend ${r} after ${Date.now()-t}ms`));

  }

  openLoginDialog(bool:boolean): void {
    this.showLogin = true;
    const dialogRef = this.dialog.open(LoginModalComponent, {
      width: '400px', // Set the desired width
      data: {showLogin: bool,} // You can pass initial data to the dialog if needed
    });

    const sub = dialogRef.componentInstance.close.subscribe(() => {

      // Close the dialog
      dialogRef.close();
      sub.unsubscribe(); //might need to comment out

      // Redirect to the /home route
      this.router.navigate(['/home']);
    });


  }


}
