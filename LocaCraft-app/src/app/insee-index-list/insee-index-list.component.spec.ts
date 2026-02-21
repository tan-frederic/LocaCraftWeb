import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InseeIndexListComponent } from './insee-index-list.component';

describe('InseeIndexListComponent', () => {
  let component: InseeIndexListComponent;
  let fixture: ComponentFixture<InseeIndexListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InseeIndexListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InseeIndexListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
