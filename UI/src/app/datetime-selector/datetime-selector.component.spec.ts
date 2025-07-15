import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DatetimeSelectorComponent } from './datetime-selector.component';

describe('DatetimeSelectorComponent', () => {
  let component: DatetimeSelectorComponent;
  let fixture: ComponentFixture<DatetimeSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DatetimeSelectorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DatetimeSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
