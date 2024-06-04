import {Component, ElementRef, EventEmitter, Input, Output, output, ViewChild} from '@angular/core';
import {FormsModule, FormControl, UntypedFormControl, ReactiveFormsModule} from '@angular/forms';
import { MatCheckbox, MatCheckboxChange } from '@angular/material/checkbox';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatRadioButton, MatRadioGroup } from '@angular/material/radio';
import { MatSlider, MatSliderThumb } from '@angular/material/slider';
import {
  MtxCalendar,
  MtxCalendarView,
  MtxDatetimepicker,
  MtxDatetimepickerInput,
  MtxDatetimepickerMode,
  MtxDatetimepickerToggle,
  MtxDatetimepickerType,
} from '@ng-matero/extensions/datetimepicker';
import {provideNativeDatetimeAdapter} from "@ng-matero/extensions/core";
import {MatCard} from "@angular/material/card";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-datetime-selector',
  standalone: true,
  imports: [
    MtxCalendar,
    MatCard,
    MatLabel,
    MtxDatetimepicker,
    MatFormField,
    MtxDatetimepickerToggle,
    MtxDatetimepickerInput,
    MatInput,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './datetime-selector.component.html',
  styleUrl: './datetime-selector.component.css',
  providers:[provideNativeDatetimeAdapter()
  ]
})
export class DatetimeSelectorComponent {
  type: MtxDatetimepickerType = 'datetime';
  mode: MtxDatetimepickerMode = 'auto';
  startView: MtxCalendarView = 'month';
  multiYearSelector = false;
  touchUi = false;
  twelvehour = true;
  timeInterval = 1;
  timeInput = true;

  @Input() disabled: boolean=false
  @Input() opened: boolean=false
  @Output() dateTime = new EventEmitter<Date>();
  customHeader: any=null

  datetime = new UntypedFormControl();

  @Input() selectedDateTime: Date | null = null;

  out(e: any) {
    //console.log(e.target.value);
    this.selectedDateTime=e.target.value;
    this.dateTime.emit(e.target.value)
  }
  getDateTime(){
    //console.log(this.selectedDateTime?.toLocaleString("en-US", {hour12:true}))
    return this.selectedDateTime?.toLocaleString("en-US", {hour12:true})??""
  }
  openCal(){
    this.opened=true;
  }
  closeCal(){
    console.log("closed");
    this.opened=false;
  }



}
