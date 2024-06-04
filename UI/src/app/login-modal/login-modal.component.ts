import {Component, EventEmitter, inject, Inject, Output} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {FormControl, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {GroupsService, ToDosService, UsersService} from "../TodoService";
import {jwtDecode} from "jwt-decode";
import {HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {gsap} from "gsap";

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.css']
})
export class LoginModalComponent {
  public showLogin:boolean=true;
  public myFormGroup: FormGroup;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.showLogin = data.showLogin
    this.myFormGroup = new FormGroup({
      isLogin:new FormControl(this.showLogin),
      fullName: new FormControl(''),
      email: new FormControl(''),
      username: new FormControl(''),
      password: new FormControl('')
    });
    console.log(data);
  }
  @Output() close = new EventEmitter<void>();

  todoItemService: ToDosService = inject(ToDosService)
  usersService: UsersService = inject(UsersService)
  groupsService: GroupsService = inject(GroupsService)
  wrongPassword = false;

  onSubmit() {
    // Handle login logic here
    console.log(`${this.showLogin?"Login":"Sign Up"} with`, this.myFormGroup.value);

    if (this.showLogin) {
      this.usersService.login( {
        username:this.myFormGroup.value.username,
        password:this.myFormGroup.value.password} )
        .subscribe({next:res=>
      {
          console.log("successful login!", res);
          // Close modal after successful login
          this.wrongPassword = false
          this.close.emit();

        //save jwt to local storage and redirect to /home //delete
        localStorage.setItem('token', JSON.stringify(res));
        localStorage.setItem('user', JSON.stringify(jwtDecode(res)));

      }, error:()=>{
            console.log("Incorrect username or password.");
            this.wrongPassword = true;
            this.onIncorrect();
          }
        })
    }
    else{
      this.usersService.create( {
        username:this.myFormGroup.value.username,
        password:this.myFormGroup.value.password,
        email:this.myFormGroup.value.email,
        name:this.myFormGroup.value.fullName}
      ).subscribe({next:res=>
        {
          console.log("successful login!", res);
          // Close modal after successful login
          this.wrongPassword = false
          this.close.emit();

          //save jwt to local storage and redirect to /home //delete
          localStorage.setItem('token', JSON.stringify(res));
          localStorage.setItem('decodedToken', JSON.stringify(jwtDecode(res))); //maybe dont save

        }, error:(res)=>{
          console.log("res",res);
          this.wrongPassword = true;
          this.onIncorrect();
        }
      })


    }
  }

  onIncorrect(){
    const element = document.getElementById("login-btn");
    const shakeTL = gsap.timeline({ repeat: 3, yoyo: true });

    // Shake the element back and forth

    shakeTL.to(element, {
      x: 5,
      duration: 0.1,
    }).to(element, {
      x: -5,
      duration: 0.1,
    });
  }

}
