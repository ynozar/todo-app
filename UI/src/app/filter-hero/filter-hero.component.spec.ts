import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterHeroComponent } from './filter-hero.component';

describe('FilterHeroComponent', () => {
  let component: FilterHeroComponent;
  let fixture: ComponentFixture<FilterHeroComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FilterHeroComponent]
    });
    fixture = TestBed.createComponent(FilterHeroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
