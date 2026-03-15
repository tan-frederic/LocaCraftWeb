import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OccupancyRateComponent } from './occupancy-rate.component';

describe('OccupancyRateComponent', () => {
  let component: OccupancyRateComponent;
  let fixture: ComponentFixture<OccupancyRateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OccupancyRateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OccupancyRateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
