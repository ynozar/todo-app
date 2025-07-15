import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-countdown',
  templateUrl: './countdown.component.html',
  styleUrl: './countdown.component.css'
})
export class CountdownComponent implements OnInit {

  @Input() todo:any={}
  @Input() targetDate:string=""
  public hours=0
  public minutes=0
  public seconds=0
  public hidden=false;


  ngOnInit(): void {
    console.log("this.targetDate")
  const date=new Date(this.targetDate)
    //console.log(this.targetDate)

    let now =Date.now();
    if(((date.valueOf() - now) / 36e5)>72){
      this.hidden=true;
    }
    setInterval(() => {

      const totalSeconds = Math.abs((date.valueOf() - now) / 1000);

      if (date.valueOf() < now) {
        this.hours=0
        this.minutes=0
        this.seconds=0 //just in case
        return;
      }
      // Calculate hours, minutes, and remaining seconds
      this.hours = Math.floor(totalSeconds / 3600);
      this.minutes = Math.floor((totalSeconds % 3600) / 60);
      this.seconds = Math.floor(totalSeconds % 60);
      //console.log(hours, minutes, Math.trunc(seconds));
      now+=1000
    }, 1000)

  }


}
