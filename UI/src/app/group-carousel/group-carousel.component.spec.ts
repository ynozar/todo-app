import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupCarouselComponent } from './group-carousel.component';

describe('GroupCarouselComponent', () => {
  let component: GroupCarouselComponent;
  let fixture: ComponentFixture<GroupCarouselComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupCarouselComponent]
    });
    fixture = TestBed.createComponent(GroupCarouselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
