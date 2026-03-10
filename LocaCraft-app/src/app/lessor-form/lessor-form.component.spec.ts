import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { LessorFormComponent } from './lessor-form.component';
import { LessorService } from '../Services/lessor.service';
import { Lessor } from '../models/lessor';

const EMPTY_LESSOR: Lessor = {
  id: 0, firstName: '', lastName: '', address: '',
  city: '', postalCode: '', country: '', phone: '', email: '', leases: [],
};
const CREATED_LESSOR: Lessor = {
  id: 1, firstName: 'Jean', lastName: 'Dupont', address: '1 rue de la Paix',
  city: 'Paris', postalCode: '75001', country: 'France',
  phone: '0600000000', email: 'jean@example.com', leases: [],
};

describe('LessorFormComponent', () => {
  let serviceSpy: jasmine.SpyObj<LessorService>;

  beforeEach(async () => {
    serviceSpy = jasmine.createSpyObj('LessorService', ['createLessor']);
    serviceSpy.createLessor.and.returnValue(of(CREATED_LESSOR));

    await TestBed.configureTestingModule({
      imports: [LessorFormComponent],
      providers: [{ provide: LessorService, useValue: serviceSpy }],
    }).compileComponents();
  });

  it('should create', () => {
    expect(TestBed.createComponent(LessorFormComponent).componentInstance).toBeTruthy();
  });

  it('should initialise with an empty lessor', () => {
    const comp = TestBed.createComponent(LessorFormComponent).componentInstance;
    expect(comp.lessor).toEqual(EMPTY_LESSOR);
  });

  it('should call service.createLessor on submit', () => {
    const comp = TestBed.createComponent(LessorFormComponent).componentInstance;
    comp.onSubmit();
    expect(serviceSpy.createLessor).toHaveBeenCalledWith(comp.lessor);
  });

  it('should emit formSubmitted with the created lessor on success', () => {
    const comp = TestBed.createComponent(LessorFormComponent).componentInstance;
    let emitted: Lessor | undefined;
    comp.formSubmitted.subscribe((l: Lessor) => (emitted = l));
    comp.onSubmit();
    expect(emitted).toEqual(CREATED_LESSOR);
  });

  it('should reset isSubmitting to false after success', () => {
    const comp = TestBed.createComponent(LessorFormComponent).componentInstance;
    comp.onSubmit();
    expect(comp.isSubmitting).toBeFalse();
  });

  it('should set errorMessage and emit formError on API failure', () => {
    serviceSpy.createLessor.and.returnValue(throwError(() => ({ status: 500 })));
    const comp = TestBed.createComponent(LessorFormComponent).componentInstance;
    let errorEmitted: string | undefined;
    comp.formError.subscribe((e: string) => (errorEmitted = e));
    comp.onSubmit();
    expect(comp.errorMessage).toContain('500');
    expect(errorEmitted).toBe(comp.errorMessage);
    expect(comp.isSubmitting).toBeFalse();
  });

  it('should not call service if isSubmitting is already true', () => {
    const comp = TestBed.createComponent(LessorFormComponent).componentInstance;
    comp.isSubmitting = true;
    comp.onSubmit();
    expect(serviceSpy.createLessor).not.toHaveBeenCalled();
  });

  it('should emit formCancelled on cancel', () => {
    const comp = TestBed.createComponent(LessorFormComponent).componentInstance;
    let cancelled = false;
    comp.formCancelled.subscribe(() => (cancelled = true));
    comp.onCancel();
    expect(cancelled).toBeTrue();
  });
});
