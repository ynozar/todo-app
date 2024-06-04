import { Component } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-drawer',
  templateUrl: './drawer.component.html',
  styleUrls: ['./drawer.component.css']
})
export class DrawerComponent {



  constructor(private router: Router,private route: ActivatedRoute) {}

  logout(){
    localStorage.clear();
    this.router.navigate(['/']);
  }
}
