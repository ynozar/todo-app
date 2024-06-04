import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

  @Input() name:any=""

  public openDrawer=false;
  @Output() toggleDrawerEvent = new EventEmitter<boolean>();
  toggleDrawer(){
    this.openDrawer=!this.openDrawer;
    //document.getElementById("my-drawer-4")!.checked=true;
    this.toggleDrawerEvent.emit(this.openDrawer);
  }

}
