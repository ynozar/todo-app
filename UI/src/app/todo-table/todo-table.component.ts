import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {GroupsService, ToDosService, UsersService} from "../TodoService";
import {jwtDecode} from "jwt-decode";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-todo-table',
  templateUrl: './todo-table.component.html',
  styleUrl: './todo-table.component.css'
})
export class TodoTableComponent implements OnInit {
  @Input() todos:any=[]
  @Input() filtered_todos:any=[]
  @Output() todosChange:any= new EventEmitter<any>()
  @Input() groups:any=[]
  @Input() singleGroup:boolean=false
  @Input() query:any=""


  public userToken:string="";
  public decodedToken:any={};
  todoItemService: ToDosService = inject(ToDosService)
  usersService: UsersService = inject(UsersService)
  groupsService: GroupsService = inject(GroupsService)

  constructor(private router: Router,private route: ActivatedRoute) {}


  ngOnInit() {
    this.userToken=localStorage.getItem("token") ?? "";
    this.userToken=this.userToken.replaceAll("\"","");
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
  }

  removeMarkdown(markdown:string) {
  //implement

  return markdown;
}
  extractDate(date:string) {
  let extracted=new Date(date);

  return extracted.toLocaleString("en-US")
  }

  getGroupName(todo:any) {
    const x = this.groups.find((x:any) => x?.uid === todo.groupUid);
    return x?.name ?? ""

  }


  getTodos(){
    //evaluate filter


    //search query
   const filtered_groups=this.groups.filter((group:any)=>group?.name.toUpperCase().includes(this.query)).map((x:any) => x.uid);
    return this.todos.filter((todo:any)=>{
      return (todo.title.toUpperCase().includes(this.query)
        ||todo.description.toUpperCase().includes(this.query)
        || filtered_groups.includes(todo.groupUid)
      );

    })
  }

  async deleteTodo(todoUid:string){
    console.log("delete ", todoUid)
    const r = await this.todoItemService.deleteToDo(todoUid).subscribe({
      next: res => {
        console.log(res);
        this.refreshTodos()
      }, error: (err) => {
        console.log(err);
      }
    })

    }

    refreshTodos(){
      this.todoItemService.getAllToDos((this.singleGroup)?this.groups[0].uid:undefined).subscribe({
        next: res => {
          this.todos=res
          this.todosChange.emit(this.todos);
        }, error: (err) => {
          console.log(err);
        }
      })
      //evaluate query and filters and export // might not neeed

    }

  patchTodoPriority(todoUid:string,priority:number){
    console.log("priority ", todoUid, " ", priority)
    this.todoItemService.patchToDo({
      uid:todoUid,
      priority:priority
    }).subscribe({
      next: res => {
        this.refreshTodos()
        console.log(res);
      }, error: (err) => {
        console.log(err);
      }})

  }
  patchTodoStatus(todoUid:string,event:any){
    console.log("status ", todoUid, " ", event.target.checked)
    this.todoItemService.patchToDo({
      uid:todoUid,
      isComplete:event.target.checked
    }).subscribe({
      next: res => {
        console.log(res);
        this.refreshTodos()
      }, error: (err) => {
        console.log(err);
      }})

  }


  getGroupColor(todo:any) {
    const x = this.groups.find((x:any) => x?.uid === todo.groupUid);
    const c = this.getTextColor(x?.color??"#FFFFFF")
    return [x?.color??"#bdc000",c]
// bg-[${x.color}]
  }
   getTextColor(hexColor:string) {
    const r = parseInt(hexColor.slice(1, 3), 16);
    const g = parseInt(hexColor.slice(3, 5), 16);
    const b = parseInt(hexColor.slice(5, 7), 16);
    const brightness = Math.sqrt(0.299 * r ** 2 + 0.587 * g ** 2 + 0.114 * b ** 2);
    return brightness < 128 ? 'white' : 'black';
  }
// Usage: getTextColor('#336699'); // Returns either white or black

  trimDescription(desc:string,max:number=20){
    //flip order, get screensize dynamically
    desc=this.getFirstPContent(desc);
    //console.log("desc ","ss",desc);
    desc=desc.substring(0,Math.min(max,desc.length));
    return desc;
  }

  getFirstPContent(htmlString:string) {
    const parser = new DOMParser();
    const doc = parser.parseFromString(htmlString, 'text/html');
    const firstP = doc.querySelector('p');
    if(firstP==null &&this.hasHtmlTags(htmlString)){
      return ""
    }
    return  firstP?.textContent?.trim() ?? htmlString;
  }

   hasHtmlTags(text:string) {
     return /<[^\s>]/.test(text); //switch to something robust
   }



}


