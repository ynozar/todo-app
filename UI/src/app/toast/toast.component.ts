import {Component, Input, OnChanges} from '@angular/core';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css'
})
export class ToastComponent {

  @Input() toast:string=""

  public toasts:any=[]



  addToast(toast:string){
    this.toasts.push(this.toast);
    setTimeout(() => {
      console.log("Delayed for 1 second.");
      this.toasts.splice(this.toasts.indexOf(toast), 1); //can probably just pop(0)
    }, 1000);
  }
}
