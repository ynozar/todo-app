import {AfterViewInit, Component, ElementRef, EventEmitter, inject, OnInit, Output, ViewChild} from '@angular/core';
import {GroupsService, ToDosService, UsersService} from "../../TodoService";
import {ActivatedRoute, Router} from "@angular/router";
import {jwtDecode} from "jwt-decode";
import Quill from 'quill';
import {FormControl, FormGroup} from "@angular/forms";


@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css'],
})
export class CreateComponent implements OnInit, AfterViewInit {
  public drawerOpen= false;
  public userToken:string="";
  public decodedToken:any={};
  public groups:any=[];
  public todos:any=[];
  public query:any=""

  public todoFormGroup: FormGroup = new FormGroup({
    title: new FormControl(null),
    description: new FormControl(''),
    groupUid: new FormControl(null),
    dueAt: new FormControl(null),
    priority: new FormControl(null),
  })


  @ViewChild('editorRef') editorRef: ElementRef| null = null;

  @ViewChild("editorContainer", { static: true })
  editorContainer: ElementRef | null = null;
  quill: Quill | undefined;



  @Output() activeToast: EventEmitter<string> = new EventEmitter<string>();

  todoItemService: ToDosService = inject(ToDosService)
  usersService: UsersService = inject(UsersService)
  groupsService: GroupsService = inject(GroupsService)

  constructor(private router: Router,private route: ActivatedRoute) {}


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
    this.groupsService.getAllGroups().subscribe({
      next: res => {
        this.groups=res;
        console.log(this.groups);
      }, error: (err) => {
        console.log(err);
      }
    });

    this.route.queryParams.subscribe(params => {
      this.todoFormGroup.patchValue({groupUid: params['group']})
    })



  }


  ngAfterViewInit(): void {


    const toolbarOptions = [
      ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
      ['blockquote', 'code-block'],
      ['link', 'image', 'video', 'formula'],

      [{ 'header': 1 }, { 'header': 2 }],               // custom button values
      [{ 'list': 'ordered'}, { 'list': 'bullet' }, { 'list': 'check' }],
      [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
      [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
      [{ 'direction': 'rtl' }],                         // text direction

      //[{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
      [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

      [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
      [{ 'font': [] }],
      [{ 'align': [] }],

      ['clean']                                         // remove formatting button
    ];


    if (this.editorContainer) {
      try {
        this.quill = new Quill(this.editorContainer.nativeElement, {
          modules: {
            toolbar: toolbarOptions,
          },
          theme: "snow",

        });
      } catch (error) {
        console.error("Error creating Quill editor:", error);
      }
    } else {
      console.error("Element with #editorContainer not found!");
    }


    this.quill?.keyboard.addBinding({
      key: 'b',
      shortKey: true
    }, (range: any, context: any) => {
      this.quill?.formatText(range, 'bold', true);
    });

    this.quill?.keyboard.addBinding({
      key: 's',
      shortKey: true
    }, (range: any, context: any) => {
      //this.save(); add back for edit
    });




  }


  save(){

console.log("saved: ",this.getEditorContent())
  }

  getEditorContent() { //I should wrap in custom tag
    if (this.quill) {
      return this.quill.root.innerHTML;
    }
    return "";
  }
  setEditorContent(content:string) { //for edit
    if (this.quill) {
      this.quill.root.innerHTML=content;
    }
    return "";
  }
  openDrawer() {
    this.drawerOpen = true;

  }
  closeDrawer() {
    this.drawerOpen = false;

  }

  onSubmit(){
    this.setDesc()
    console.log(this.todoFormGroup.value)

    this.todoItemService.createToDo(this.todoFormGroup.value).subscribe({
      next: res => {
        console.log(res);
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.log(err);
      }
    })

  }

  setDateTime(e:any) {
    this.todoFormGroup.patchValue({dueAt:e})
  }

  setPriority(e:any) {
    this.todoFormGroup.patchValue({priority:e})

  }
  setDesc() {
    this.todoFormGroup.patchValue({description:this.getEditorContent()})

  }
  checkSubmit(){
    for (const controlName in this.todoFormGroup.controls) {
      if (this.todoFormGroup.controls[controlName].value==null&&controlName!="groupUid") {
        return false
      }
    }
    return true;

  }

}
