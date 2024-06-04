import {Component, EventEmitter, HostListener, inject, OnInit, Output} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {LoginModalComponent} from "../login-modal/login-modal.component";
import {GroupsService, ToDosService, UsersService} from "../TodoService";
import {Router} from "@angular/router";
import {jwtDecode} from "jwt-decode";
import {list} from "postcss";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit{
  public drawerOpen= false;
  public userToken:string="";
  public decodedToken:any={};
  public userSettings:any={};
  public groups:any=[];
  public todos:any=[];
  public query:any=""


  @Output() activeToast: EventEmitter<string> = new EventEmitter<string>();

  todoItemService: ToDosService = inject(ToDosService)
  usersService: UsersService = inject(UsersService)
  groupsService: GroupsService = inject(GroupsService)

  constructor(private router: Router) {}


  ngOnInit(): void {
    this.userToken=localStorage.getItem("token") ?? "";
    this.userToken=this.userToken.replaceAll("\"","");
    //get from local storage includes " for some reason //fix unauth cors issue as well
    try {
      this.decodedToken=jwtDecode(this.userToken);
    }
    catch (e){
      this.router.navigate(['/']);
    }

    if(this.userToken) {
      this.usersService.configuration.apiKeys = {
        'Authorization': `Bearer ${this.userToken}`,
      }
      this.todoItemService.configuration.apiKeys = {
        'Authorization': `Bearer ${this.userToken}`,
      }
      this.groupsService.configuration.apiKeys = {
        'Authorization': `Bearer ${this.userToken}`,
      }
      this.usersService.verify().subscribe({
        next: res => {
          console.log(res);
          this.decodedToken=jwtDecode(this.userToken);
          console.log(this.decodedToken);
        }, error: () => {
          console.log();
          this.router.navigate(['/']);
          return;
        }
      });
    }
    else {
      this.router.navigate(['/']);
      return;
    }
    //get data
    this.todoItemService.getAllToDos().subscribe({
      next: res => {
        this.todos=res;
        this.todos=this.todos.sort((a:any,b:any)=> {
          return (a.isComplete === b.isComplete)? 0 : a.isComplete? 1 : -1;
        })
        console.log(this.todos);
      }, error: (err) => {
        console.log(err);
      }
    });
    this.groupsService.getAllGroups().subscribe({
      next: res => {
        this.groups=res;
        console.log(this.groups);
      }, error: (err) => {
        console.log(err);
      }
    });


  }

  openDrawer() {
    this.drawerOpen = true;

  }
  closeDrawer() {
    this.drawerOpen = false;

  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'k' && event.metaKey) {
      // Handle Command + K shortcut here
      console.log('Command + K pressed');
      // You can navigate to a specific input box or perform any other action
      document.getElementById("search-input")?.focus()
    }
  }

  setQuery(e:any){

    this.query=e
  }

  setToast(e:any){
    this.activeToast.emit("Yo Yo Yo")
  }

  filterByPriority(e:any){
    this.todoItemService.getAllToDos(undefined,undefined,e>0?e:undefined).subscribe({
      next: res => {
        this.todos=res;
        this.todos=this.todos.sort((a:any,b:any)=> {
          return (a.isComplete === b.isComplete)? 0 : a.isComplete? 1 : -1; //might not be needed
        })
        console.log(this.todos);
      }, error: (err) => {
        console.log(err);
      }
    });
  }

  filterByStatus(e:any){
    console.log(e);
    this.todoItemService.getAllToDos(undefined,e,undefined,undefined).subscribe({
      next: res => {
        this.todos=res;
        console.log("res",this.todos);
      }, error: (err) => {
        console.log(err);
      }
    });
  }



}
