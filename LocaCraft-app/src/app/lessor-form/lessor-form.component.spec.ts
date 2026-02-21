import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LessorFormComponent } from './lessor-form.component';

describe('LessorFormComponent', () => {
  let component: LessorFormComponent;
  let fixture: ComponentFixture<LessorFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LessorFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LessorFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
