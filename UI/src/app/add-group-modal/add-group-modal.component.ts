import {Component, EventEmitter, inject, Inject, OnInit, Output} from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {GroupsService, ToDosService, UsersService} from "../TodoService";
import {jwtDecode} from "jwt-decode";
import {gsap} from "gsap";

@Component({
  selector: 'app-add-group-modal',
  templateUrl: './add-group-modal.component.html',
  styleUrl: './add-group-modal.component.css'
})
export class AddGroupModalComponent implements OnInit {

  public userToken=""
  public myFormGroup: FormGroup;
  public selectedColor=""
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.myFormGroup = new FormGroup({
      color: new FormControl(''),
      name: new FormControl(''),
    });
    console.log(data);
  }
  @Output() close = new EventEmitter<void>();

  todoItemService: ToDosService = inject(ToDosService)
  usersService: UsersService = inject(UsersService)
  groupsService: GroupsService = inject(GroupsService)
  Error = false;

  ngOnInit() {
    this.userToken = localStorage.getItem("token") ?? "";
    this.userToken = this.userToken.replaceAll("\"", "");
    //get from local storage includes " for some reason //fix unauth cors issue as well

    if (this.userToken) {
      this.usersService.configuration.apiKeys = {
        'Authorization': `Bearer ${this.userToken}`,
      }
      this.todoItemService.configuration.apiKeys = {
        'Authorization': `Bearer ${this.userToken}`,
      }
      this.groupsService.configuration.apiKeys = {
        'Authorization': `Bearer ${this.userToken}`,
      }
    }
  }


  onSubmit() {
    // Handle login logic here
    console.log(`Created Group with`, this.myFormGroup.value);

      this.groupsService.createGroup( {
        name:this.myFormGroup.value.name,
        color:this.selectedColor!=""? this.selectedColor:undefined,
      } )
        .subscribe({next:res=>
          {
            console.log("ADD SUCCESSFUL", res);
            this.Error = false
            this.close.emit();

          }, error:()=>{
            console.log("ERROR");
            this.Error = true;
            this.onIncorrect();
          }
        })
    }

  onIncorrect(){
    const element = document.getElementById("add-group-btn");
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
