import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaseListComponent } from './lease-list.component';

describe('LeaseListComponent', () => {
  let component: LeaseListComponent;
  let fixture: ComponentFixture<LeaseListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeaseListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LeaseListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
