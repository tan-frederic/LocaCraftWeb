import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter, Router } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { of, throwError } from 'rxjs';
import { SimpleChange } from '@angular/core';

import { LeaseFormComponent } from './lease-form.component';
import { LeaseService } from '../Services/lease.service';
import { RealEstateService } from '../Services/real-estate.service';
import { LessorService } from '../Services/lessor.service';
import { TenantService } from '../Services/tenant.service';
import { RentReceiptService } from '../Services/rent-receipt.service';
import { Lease } from '../models/lease';
import { Lessor } from '../models/lessor';
import { Tenant } from '../models/tenant';
import { RealEstateAsset } from '../models/real-estate-assets';

const ASSET: RealEstateAsset = {
  id: 10, name: 'Appart X', description: '', address: '', addressComplement: '',
  postalCode: '', city: '', country: '', leases: [],
};
const LESSOR: Lessor = {
  id: 5, firstName: 'A', lastName: 'B', address: '1 rue', city: 'Paris',
  postalCode: '75001', country: 'France', phone: '', email: '', leases: [],
};
const LEASE: Lease = {
  id: 1, realEstateAssetId: 10, lessorId: 5, leaseName: 'Test bail',
  monthlyRent: 700, monthlyCharges: 50, deposit: 1400, rentIndexReference: 100,
  isOngoing: false, startDate: new Date('2021-01-01'), endDate: new Date('2022-01-01'),
};
const TENANT: Tenant = {
  id: 1, leaseId: 1, name: 'Doe', surname: 'John', phoneNumber: '',
  email: '', address: '1 rue', city: 'Lyon', postalCode: '69001', country: 'France',
};

describe('LeaseFormComponent', () => {
  let leaseSpy: jasmine.SpyObj<LeaseService>;
  let realEstateSpy: jasmine.SpyObj<RealEstateService>;
  let lessorSpy: jasmine.SpyObj<LessorService>;
  let tenantSpy: jasmine.SpyObj<TenantService>;
  let receiptSpy: jasmine.SpyObj<RentReceiptService>;

  function setup(queryParams: Record<string, string> = {}) {
    TestBed.overrideProvider(ActivatedRoute, {
      useValue: { queryParamMap: of(convertToParamMap(queryParams)) },
    });
    return TestBed.createComponent(LeaseFormComponent);
  }

  beforeEach(async () => {
    leaseSpy = jasmine.createSpyObj('LeaseService', [
      'getLeaseById', 'createLease', 'updateLease', 'deleteLease',
    ]);
    realEstateSpy = jasmine.createSpyObj('RealEstateService', ['getRealEstateAssetById']);
    lessorSpy = jasmine.createSpyObj('LessorService', [
      'getLessorsByRealEstateAssetId', 'getLessorById', 'createLessor',
    ]);
    tenantSpy = jasmine.createSpyObj('TenantService', [
      'getTenantByLeaseId', 'getTenantsByLeaseId', 'createTenant',
    ]);
    receiptSpy = jasmine.createSpyObj('RentReceiptService', ['generateRentReceiptPDF']);

    realEstateSpy.getRealEstateAssetById.and.returnValue(of(ASSET));
    lessorSpy.getLessorsByRealEstateAssetId.and.returnValue(of([LESSOR]));
    lessorSpy.getLessorById.and.returnValue(of(LESSOR));
    lessorSpy.createLessor.and.returnValue(of(LESSOR));
    leaseSpy.getLeaseById.and.returnValue(of(LEASE));
    leaseSpy.createLease.and.returnValue(of(LEASE));
    leaseSpy.updateLease.and.returnValue(of(LEASE));
    tenantSpy.getTenantsByLeaseId.and.returnValue(of([TENANT]));
    tenantSpy.getTenantByLeaseId.and.returnValue(of(TENANT));
    tenantSpy.createTenant.and.returnValue(of(TENANT));

    await TestBed.configureTestingModule({
      imports: [LeaseFormComponent],
      providers: [
        provideRouter([]),
        provideLocationMocks(),
        { provide: LeaseService, useValue: leaseSpy },
        { provide: RealEstateService, useValue: realEstateSpy },
        { provide: LessorService, useValue: lessorSpy },
        { provide: TenantService, useValue: tenantSpy },
        { provide: RentReceiptService, useValue: receiptSpy },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    expect(setup().componentInstance).toBeTruthy();
  });

  it('ngOnChanges should update formData.realEstateAssetId when input changes', () => {
    const fixture = setup();
    fixture.componentInstance.realEstateAssetId = 10;
    fixture.componentInstance.ngOnChanges({
      realEstateAssetId: new SimpleChange(null, 10, true),
    });
    expect(fixture.componentInstance.formData.realEstateAssetId).toBe(10);
  });

  it('ngOnChanges should load real estate name and lessors when realEstateAssetId is set', () => {
    const fixture = setup();
    fixture.componentInstance.realEstateAssetId = 10;
    fixture.componentInstance.ngOnChanges({
      realEstateAssetId: new SimpleChange(null, 10, true),
    });
    expect(realEstateSpy.getRealEstateAssetById).toHaveBeenCalledWith(10);
    expect(lessorSpy.getLessorsByRealEstateAssetId).toHaveBeenCalledWith(10);
    expect(fixture.componentInstance.realEstateName).toBe('Appart X');
    expect(fixture.componentInstance.lessors).toEqual([LESSOR]);
  });

  it('ngOnInit should read realEstateAssetId from query params', () => {
    const fixture = setup({ realEstateAssetId: '10' });
    fixture.detectChanges();
    expect(fixture.componentInstance.formData.realEstateAssetId).toBe(10);
    expect(realEstateSpy.getRealEstateAssetById).toHaveBeenCalledWith(10);
  });

  it('ngOnInit should load lease and tenants when leaseId query param is present', () => {
    const fixture = setup({ realEstateAssetId: '10', leaseId: '1' });
    fixture.detectChanges();
    expect(leaseSpy.getLeaseById).toHaveBeenCalledWith(1);
    expect(tenantSpy.getTenantsByLeaseId).toHaveBeenCalledWith(1);
    expect(fixture.componentInstance.isEditing).toBeTrue();
    expect(fixture.componentInstance.formData.leaseName).toBe('Test bail');
  });

  it('onSubmit should create lease and emit formSubmitted', () => {
    const comp = setup().componentInstance;
    let submitted: Lease | undefined;
    comp.formSubmitted.subscribe((l: Lease) => (submitted = l));
    comp.onSubmit();
    expect(leaseSpy.createLease).toHaveBeenCalled();
    expect(submitted).toEqual(LEASE);
    expect(comp.isSubmitting).toBeFalse();
  });

  it('onSubmit should navigate to /details after creating a lease', () => {
    // inject must come AFTER setup() to avoid instantiating the module before overrideProvider
    const comp = setup().componentInstance;
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    comp.formData.realEstateAssetId = 10;
    comp.onSubmit();
    expect(router.navigate).toHaveBeenCalledWith(['/details', 10]);
  });

  it('onSubmit should emit formError and set errorMessage on create failure', () => {
    leaseSpy.createLease.and.returnValue(throwError(() => ({ status: 500 })));
    const comp = setup().componentInstance;
    let errEmitted: string | undefined;
    comp.formError.subscribe((e: string) => (errEmitted = e));
    comp.onSubmit();
    expect(comp.errorMessage).toContain('500');
    expect(errEmitted).toBe(comp.errorMessage);
    expect(comp.isSubmitting).toBeFalse();
  });

  it('onSubmit should do nothing if isSubmitting is true', () => {
    const comp = setup().componentInstance;
    comp.isSubmitting = true;
    comp.onSubmit();
    expect(leaseSpy.createLease).not.toHaveBeenCalled();
  });

  it('onSubmit with new lessor should create lessor then lease', () => {
    const comp = setup().componentInstance;
    comp.lessorSelection = 'new';
    comp.newLessor = { ...LESSOR, firstName: 'Jean', lastName: 'Dupont' };
    comp.onSubmit();
    expect(lessorSpy.createLessor).toHaveBeenCalled();
    expect(leaseSpy.createLease).toHaveBeenCalled();
  });

  it('onSubmit with invalid new lessor should set errorMessage and abort', () => {
    const comp = setup().componentInstance;
    comp.lessorSelection = 'new';
    comp.newLessor = { ...LESSOR, firstName: '' }; // invalid: empty firstName
    comp.onSubmit();
    expect(comp.errorMessage).toContain('required');
    expect(comp.isSubmitting).toBeFalse();
    expect(leaseSpy.createLease).not.toHaveBeenCalled();
  });

  it('onCancel should emit formCancelled', () => {
    const comp = setup().componentInstance;
    let cancelled = false;
    comp.formCancelled.subscribe(() => (cancelled = true));
    comp.onCancel();
    expect(cancelled).toBeTrue();
  });

  it('onOngoingToggle should set isOngoing and clear endDate when toggled on', () => {
    const comp = setup().componentInstance;
    comp.formData.endDate = '2025-01-01';
    comp.onOngoingToggle(true);
    expect(comp.isOngoing).toBeTrue();
    expect(comp.formData.endDate).toBe('');
  });

  it('formatAmount should round to 2 decimal places', () => {
    const comp = setup().componentInstance;
    comp.formData.monthlyRent = 800.567;
    comp.formatAmount('monthlyRent');
    expect(comp.formData.monthlyRent).toBe(800.57);
  });

  it('formatAmount should set null for NaN values', () => {
    const comp = setup().componentInstance;
    comp.formData.monthlyRent = NaN;
    comp.formatAmount('monthlyRent');
    expect(comp.formData.monthlyRent).toBeNull();
  });

  it('isNewLessorValid should return false when a required field is empty', () => {
    const comp = setup().componentInstance;
    comp.newLessor = { ...LESSOR, firstName: '' };
    expect(comp.isNewLessorValid()).toBeFalse();
  });

  it('isNewLessorValid should return true when all required fields are filled', () => {
    const comp = setup().componentInstance;
    comp.newLessor = { ...LESSOR, firstName: 'Jean', lastName: 'Dupont' };
    expect(comp.isNewLessorValid()).toBeTrue();
  });

  it('addTenant should set tenantError when the lease has not been saved yet', () => {
    const comp = setup().componentInstance;
    comp.addTenant();
    expect(comp.tenantError).toContain('saved');
    expect(tenantSpy.createTenant).not.toHaveBeenCalled();
  });

  it('generateRentReceipt should set receiptError when the lease is not saved', () => {
    const comp = setup().componentInstance;
    comp.generateRentReceipt();
    expect(comp.receiptError).toContain('saved');
  });
});
