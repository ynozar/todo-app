import {Component, EventEmitter, inject, OnDestroy, OnInit, Output} from '@angular/core';
import {GroupsService, ToDosService, UsersService} from "../TodoService";
import {ActivatedRoute, Router} from "@angular/router";
import {jwtDecode} from "jwt-decode";

@Component({
  selector: 'app-group-page',
  templateUrl: './group-page.component.html',
  styleUrl: './group-page.component.css'
})
export class GroupPageComponent implements OnInit{
  public drawerOpen= false;
  public userToken:string="";
  public decodedToken:any={};
  public selectedColor=""//might need default value
  public group:any=null;
  public groupName=""
  groupUid: string = "";
  public todos:any=[];
  public isEditing=false;
  public query:any=""
  public params:any={
    isCompleted:null,
    priority:null,
  }


  @Output() activeToast: EventEmitter<string> = new EventEmitter<string>();

  todoItemService: ToDosService = inject(ToDosService)
  usersService: UsersService = inject(UsersService)
  groupsService: GroupsService = inject(GroupsService)

  constructor(private router: Router,private route: ActivatedRoute) {}


  ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
          this.groupUid = params.get('id') ??"";
        });
        if(this.groupUid==""){
          this.router.navigate(["/home"])
        }
        console.log(this.groupUid);
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
    })

    //get data //might not need this method and can just get group by ID with todos
    this.todoItemService.getAllToDos(this.groupUid).subscribe({
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
    this.groupsService.getGroupById(this.groupUid).subscribe({
      next: res => {
        this.group=res;
        this.groupName =res.name ?? ""
        this.selectedColor=res.color??"#AABBCC";
        console.log(this.group);
      }, error: (err) => {
        console.log(err);
      }
    });


  }
  editGroup(){
    this.isEditing=true;
  }

  deleteGroup(){
    if(this.group.isDefault){
      console.log("you cant delete the default group")
    }
    else{
    this.groupsService.deleteGroup(this.groupUid).subscribe({
      next: res => {
        this.router.navigate(["/home"]);
      },
      error: err => {
        console.log(err);
      }
    })
    }
  }

  discardChanges(){
    this.isEditing=false;
  }

  saveChanges(){
    console.log(this.groupName)
    this.isEditing=false;
    this.groupsService.updateGroup({uid:this.groupUid,name:this.groupName, color:this.selectedColor}).subscribe({
      next: res => {
        this.group=res
      },
      error: err => {
        console.log(err);
      }
    })
  }

  openDrawer() {
    this.drawerOpen = true;

  }
  closeDrawer() {
    this.drawerOpen = false;

  }

  getDate(d:any){
    return new Date(d)?.toLocaleString("en-US") ??""
  }

  getCompPercent(){
    const x = this.todos.filter((obj:any)=>obj.isComplete).length
    return Math.trunc(x/Math.max(this.todos.length,1)*100);

  }

}
