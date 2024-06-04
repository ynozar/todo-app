import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  inject,
  Input,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import { gsap } from "gsap";
import {GroupsService, ToDosService, UsersService} from "../TodoService";
import {Router} from "@angular/router";
import {jwtDecode} from "jwt-decode";

@Component({
  selector: 'app-filter-hero',
  templateUrl: './filter-hero.component.html',
  styleUrls: ['./filter-hero.component.css']
})
export class FilterHeroComponent implements OnInit {

  @Input() todos:any=[];

  @Output() query = new EventEmitter<any>()

  @Output() sort = new EventEmitter<any>() //might not need

  public sortByVal="----------";


  public ascending=false;

  public completedTasks=0
  public completionRate=0

  public priorityTasks=0

  public filterParams:any={
    isCompleted:undefined,
    priority:undefined,
  }

  searchQuery: string = '';


  @Output() priorityEvent = new EventEmitter<number>()

  @Output() statusEvent = new EventEmitter<any>()

  @Output() filterEvent = new EventEmitter<any>()




  ngOnInit() {

    this.completionRate = Math.trunc(this.completedTasks / Math.min(this.todos.length, 1));

  }



  //stats helpers
  getCompletedTasks() {
    this.completedTasks=this.todos.filter((todo:any)=>todo.isComplete).length;
    return this.completedTasks
  }

  getCompletionRate() {
    return Math.trunc(this.completedTasks/Math.max(this.todos.length,1)*100);
  }

  getTasksLeft() {
    return this.todos.length-this.completedTasks;
  }

  getPriorityTasks() {
    this.priorityTasks=this.todos.filter((todo:any)=>!todo.isComplete && todo.priority==3).length;
    return this.priorityTasks
  }
  getPriorityRate() {
    return Math.trunc(this.priorityTasks/Math.max(this.todos.length-this.completedTasks,1)*100);
  }

  getNextTask() {

    if (this.todos.length==0) {
      return ["",""]
    }
    const t=this.todos.filter((a:any)=> {
      return (Date.parse(a.dueAt) > Date.now() && !a.isComplete) //
    });
    const task=t.sort((a:any, b:any) => {
      return a?.dueAt < b?.dueAt ? -1 : 1;
    })[0]

    const d = (task?.dueAt!=null)?(new Date(task.dueAt)).toLocaleDateString("en-us",{ month: '2-digit', day: '2-digit' }):"";
    return [d,task?.title??""]
  }


  //filter helpers

  searchItem() {
    //console.log(this.searchQuery);
    this.query.emit(this.searchQuery.toUpperCase())
  }

  //if it aint broken dont fix
  sortBy(e:any) {
    const val = e.target.value

    this.todos=this.todos.sort((a:any, b:any) => {
        switch (val){
          case "Title": {
            return a.title.toUpperCase() > b.title.toUpperCase() ? -1 : 1;
          }
          case "Priority": {
            return a.priority > b.priority ? -1 : 1;
          }
          case "Due Date": {
            return a.dueAt < b.dueAt ? 1 : -1;
          }
          case "Status": {
            return (a.isComplete === b.isComplete)? 0 : a.isComplete? -1 : 1;
          }
          default: { //----------
            return (a.isComplete === b.isComplete)? ( a.dueAt < b.dueAt ? 1 : -1) : a.isComplete? 1 : -1;

          }
        }
      })
    if (!val.includes("-")&&this.ascending){
      //.log("rev")
      this.todos=this.todos.reverse()
    }
  }
  reverse(){
    if (!this.sortByVal.includes("-")){
      //console.log("rev")
      this.todos=this.todos.reverse()
    }

  }
//filter api calls

  filterByPriority(e:any) {
    this.priorityEvent.emit(e)
    this.filterParams.priority=e
    this.filterEvent.emit(this.filterParams)


  }

  filterByStatus(e:any) {
    const val = e.target.value
    let emit=undefined;
    if(val=="Completed Only"){
      emit=true
    }
    else if (val=="Uncompleted Only"){
      emit=false
    }
    this.filterParams.isCompleted=emit
    this.filterEvent.emit(this.filterParams)


    this.statusEvent.emit(emit)
  }



}
