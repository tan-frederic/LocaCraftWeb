import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LateralDrawerComponent } from './lateral-drawer.component';

describe('LateralDrawerComponent', () => {
  let component: LateralDrawerComponent;
  let fixture: ComponentFixture<LateralDrawerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LateralDrawerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LateralDrawerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
