import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KpiRentComponent } from './kpi-rent.component';

describe('KpiRentComponent', () => {
  let component: KpiRentComponent;
  let fixture: ComponentFixture<KpiRentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KpiRentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KpiRentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
