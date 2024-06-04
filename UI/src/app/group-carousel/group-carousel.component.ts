import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener, inject,
  Input,
  input,
  OnInit,
  Output,
  Renderer2,
  ViewChild
} from '@angular/core';
import ScrollBooster from 'scrollbooster';
import {group} from "@angular/animations";
import {LoginModalComponent} from "../login-modal/login-modal.component";
import {MatDialog} from "@angular/material/dialog";
import {AddGroupModalComponent} from "../add-group-modal/add-group-modal.component";
import {Router} from "@angular/router";
import {GroupsService, ToDosService, UsersService} from "../TodoService";

@Component({
  selector: 'app-group-carousel',
  templateUrl: './group-carousel.component.html',
  styleUrls: ['./group-carousel.component.css']
})

export class GroupCarouselComponent implements OnInit {

  @Input() groups:any=[]
  @Output() groupsChange = new EventEmitter<any>();
  @Input() todos:any=[]
  @Input() query:any="";



  public userToken=""
  todoItemService: ToDosService = inject(ToDosService)
  usersService: UsersService = inject(UsersService)
  groupsService: GroupsService = inject(GroupsService)
  Error = false;



  @ViewChild('carousel', { static: true }) carouselRef!: ElementRef;



  constructor(private dialog: MatDialog, private router: Router,private renderer: Renderer2) {

  }

  //should probably make calls...
  getGroupCount(group:any){
    //runs forever?? try console log
  const x = this.todos.filter((x:any) => x.groupUid === group.uid);
  return x?.length ?? 0
  }



  getFillValue(group:any){
    const x = this.todos.filter((x:any) => x.groupUid === group.uid);
    const y = this.todos.filter((x:any) => (x.groupUid === group.uid) && x.isComplete);
    const z= (y?.length ?? 0)/(Math.max(x.length??0,1))*100
    return Math.max(z,5); //or should I make it indeterminate
  }

  getFillColor(group:any) {//Old
    const x=this.getFillValue(group);
    if(x>66){
      return 'progress-primary'
    }
    else if(x>33){
      return 'progress-secondary'
    }
    else{
      return 'progress-error'
    }
  }


  getGroupColor(group:any) {
    return group?.color ?? "#bdc000" //manually set figure out how to change

  }

  getGroups() {
    return this.groups.filter((groups: any) => {
      return groups.name.toUpperCase().includes(this.query)
    })
  }

  ngOnInit(): void {
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

  addGroupDialog(): void {
    const dialogRef = this.dialog.open(AddGroupModalComponent, {
      width: '400px', // Set the desired width
      data: {} // You can pass initial data to the dialog if needed
    });

    const sub = dialogRef.componentInstance.close.subscribe(() => {

      // Close the dialog
      dialogRef.close();
      sub.unsubscribe(); //might need to comment out
      // refresh
      //this.groups=this.groupsService.getAllGroups();

      this.groupsService.getAllGroups()
        .subscribe({next:res=>
          {
            console.log(res)
            this.groups=res
          }, error:()=>{
            console.log("ERROR");
            this.Error = true;
          }
        })

      this.groupsChange.emit(this.groups)
    });
  }

  //slider
/*

  @ViewChild('sliderRef') sliderRef!: ElementRef<HTMLDivElement>;

  isDown = false;
  startX: number=0;
  scrollLeft: number=0;



  onMouseDown(event: MouseEvent) {
    this.isDown = true;
    console.log("MOUSE DOWN",this.isDown);
    this.renderer.addClass(this.sliderRef.nativeElement, 'active');
    this.startX = event.pageX - this.sliderRef.nativeElement.offsetLeft;
    this.scrollLeft = this.sliderRef.nativeElement.scrollLeft;
  }

  onMouseLeave() {
    this.isDown = false;
    console.log("MOUSE LEAVE",this.isDown);
    this.renderer.removeClass(this.sliderRef.nativeElement, 'active');
  }

  onMouseUp() {
    this.isDown = false;
    console.log("MOUSE UP",this.isDown);
    this.renderer.removeClass(this.sliderRef.nativeElement, 'active');
  }

  onMouseMove(event: MouseEvent) {
    if (!this.isDown) return;
    //event.preventDefault();
    const x = event.pageX - this.sliderRef.nativeElement.offsetLeft;
    const walk = (x - this.startX) * 3;
    this.sliderRef.nativeElement.scrollLeft = this.scrollLeft - walk;
  }

  // for html : (mousedown)="onMouseDown($event)" (mouseup)="onMouseUp()" (mouseleave)="onMouseLeave()" (mousemove)="onMouseMove($event)"
*/









}
