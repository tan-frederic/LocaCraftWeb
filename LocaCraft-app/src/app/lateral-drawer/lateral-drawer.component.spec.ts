import { Component, EventEmitter, Output } from '@angular/core';
import { TestBed } from '@angular/core/testing';

import { LateralDrawerComponent } from './lateral-drawer.component';

/** Minimal embedded component that exposes the three drawer-contract outputs. */
@Component({ selector: 'app-fake-form', standalone: true, template: '' })
class FakeFormComponent {
  @Output() formSubmitted = new EventEmitter<any>();
  @Output() formError = new EventEmitter<string>();
  @Output() formCancelled = new EventEmitter<void>();
}

describe('LateralDrawerComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LateralDrawerComponent, FakeFormComponent],
    }).compileComponents();
  });

  it('should create', () => {
    expect(TestBed.createComponent(LateralDrawerComponent).componentInstance).toBeTruthy();
  });

  it('should initialise with showDrawer = false', () => {
    expect(TestBed.createComponent(LateralDrawerComponent).componentInstance.showDrawer).toBeFalse();
  });

  it('openDrawer should set showDrawer to true', () => {
    const comp = TestBed.createComponent(LateralDrawerComponent).componentInstance;
    comp.openDrawer();
    expect(comp.showDrawer).toBeTrue();
  });

  it('closeDrawer should set showDrawer to false', () => {
    const comp = TestBed.createComponent(LateralDrawerComponent).componentInstance;
    comp.showDrawer = true;
    comp.closeDrawer();
    expect(comp.showDrawer).toBeFalse();
  });

  it('closeDrawer should emit drawerClosed', () => {
    const comp = TestBed.createComponent(LateralDrawerComponent).componentInstance;
    let closed = false;
    comp.drawerClosed.subscribe(() => (closed = true));
    comp.closeDrawer();
    expect(closed).toBeTrue();
  });

  it('should relay formSubmitted from the embedded component', () => {
    const fixture = TestBed.createComponent(LateralDrawerComponent);
    fixture.componentInstance.componentToDisplay = FakeFormComponent;
    fixture.detectChanges();

    let relayed: any;
    fixture.componentInstance.formSubmitted.subscribe((v: any) => (relayed = v));

    const ref = (fixture.componentInstance as any).componentRef;
    ref?.instance.formSubmitted.emit({ id: 99 });
    expect(relayed).toEqual({ id: 99 });
  });

  it('should relay formError from the embedded component', () => {
    const fixture = TestBed.createComponent(LateralDrawerComponent);
    fixture.componentInstance.componentToDisplay = FakeFormComponent;
    fixture.detectChanges();

    let relayedError: string | undefined;
    fixture.componentInstance.formError.subscribe((e: string) => (relayedError = e));

    const ref = (fixture.componentInstance as any).componentRef;
    ref?.instance.formError.emit('some error');
    expect(relayedError).toBe('some error');
  });

  it('should relay formCancelled from the embedded component', () => {
    const fixture = TestBed.createComponent(LateralDrawerComponent);
    fixture.componentInstance.componentToDisplay = FakeFormComponent;
    fixture.detectChanges();

    let cancelled = false;
    fixture.componentInstance.formCancelled.subscribe(() => (cancelled = true));

    const ref = (fixture.componentInstance as any).componentRef;
    ref?.instance.formCancelled.emit();
    expect(cancelled).toBeTrue();
  });

  it('should apply componentData to the embedded component instance', () => {
    const fixture = TestBed.createComponent(LateralDrawerComponent);
    fixture.componentInstance.componentToDisplay = FakeFormComponent;
    fixture.componentInstance.componentData = { customProp: 'hello' };
    fixture.detectChanges();

    const ref = (fixture.componentInstance as any).componentRef;
    expect(ref?.instance.customProp).toBe('hello');
  });

  it('should destroy previous componentRef when a new component is loaded', () => {
    const fixture = TestBed.createComponent(LateralDrawerComponent);
    fixture.componentInstance.componentToDisplay = FakeFormComponent;
    fixture.detectChanges();

    const firstRef = (fixture.componentInstance as any).componentRef;
    const destroySpy = spyOn(firstRef, 'destroy').and.callThrough();

    // Setting componentToDisplay again triggers loadComponent
    fixture.componentInstance.componentToDisplay = FakeFormComponent;
    expect(destroySpy).toHaveBeenCalled();
  });
});
