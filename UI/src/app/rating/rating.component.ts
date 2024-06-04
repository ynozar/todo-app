import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.css']
})
export class RatingComponent {
  @Input() rating = 0;
  @Input() canBeEmpty = false;
  @Output() ratingChange = new EventEmitter<any>()

  toggleSelection(event:any, index: number) {
    //console.log(index,event.target.checked);
    if (!event.target.checked) {
      this.rating = 0;

    } else {
      this.rating = index;
    }
    console.log(this.rating);
    this.ratingChange.emit(this.rating);
  }

}
