import {Component, inject} from '@angular/core';
import { Router } from '@angular/router';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {LoginModalComponent} from "../login-modal/login-modal.component";
import {GroupsService, ToDosService, UsersService} from "../TodoService";

@Component({
  selector: 'app-splash-page',
  templateUrl: './splash-page.component.html',
  styleUrls: ['./splash-page.component.css']
})
export class SplashPageComponent {
  public showLogin:boolean=true;
  constructor(private dialog: MatDialog,private router: Router) {}



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
