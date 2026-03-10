import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { of, throwError } from 'rxjs';
import { SimpleChange } from '@angular/core';

import { LeaseListComponent } from './lease-list.component';
import { LeaseService } from '../Services/lease.service';
import { Lease } from '../models/lease';

const LEASE_A: Lease = {
  id: 1, realEstateAssetId: 10, lessorId: 2, leaseName: 'Bail A',
  monthlyRent: 800, monthlyCharges: 50, deposit: 1600,
  rentIndexReference: 100, isOngoing: true,
  startDate: new Date('2020-01-01'), endDate: null,
};
const LEASE_B: Lease = { ...LEASE_A, id: 2, leaseName: 'Bail B' };

describe('LeaseListComponent', () => {
  let leaseServiceSpy: jasmine.SpyObj<LeaseService>;

  beforeEach(async () => {
    leaseServiceSpy = jasmine.createSpyObj('LeaseService', [
      'getLeasesByRealEstateAssetId', 'deleteLease',
    ]);
    leaseServiceSpy.getLeasesByRealEstateAssetId.and.returnValue(of([LEASE_A, LEASE_B]));
    leaseServiceSpy.deleteLease.and.returnValue(of(undefined));

    await TestBed.configureTestingModule({
      imports: [LeaseListComponent],
      providers: [
        provideRouter([]),
        provideLocationMocks(),
        { provide: LeaseService, useValue: leaseServiceSpy },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    expect(TestBed.createComponent(LeaseListComponent).componentInstance).toBeTruthy();
  });

  it('should load leases on ngOnInit when realEstateAssetId is set', () => {
    const fixture = TestBed.createComponent(LeaseListComponent);
    fixture.componentInstance.realEstateAssetId = 10;
    fixture.detectChanges();
    expect(leaseServiceSpy.getLeasesByRealEstateAssetId).toHaveBeenCalledWith(10);
    expect(fixture.componentInstance.leases).toEqual([LEASE_A, LEASE_B]);
  });

  it('should keep leases empty on ngOnInit when realEstateAssetId is null', () => {
    const fixture = TestBed.createComponent(LeaseListComponent);
    fixture.detectChanges();
    expect(leaseServiceSpy.getLeasesByRealEstateAssetId).not.toHaveBeenCalled();
    expect(fixture.componentInstance.leases).toEqual([]);
  });

  it('should reload leases on ngOnChanges when realEstateAssetId changes', () => {
    const fixture = TestBed.createComponent(LeaseListComponent);
    fixture.detectChanges();
    fixture.componentInstance.realEstateAssetId = 20;
    fixture.componentInstance.ngOnChanges({
      realEstateAssetId: new SimpleChange(null, 20, false),
    });
    expect(leaseServiceSpy.getLeasesByRealEstateAssetId).toHaveBeenCalledWith(20);
  });

  it('should clear leases on ngOnChanges when realEstateAssetId becomes null', () => {
    const fixture = TestBed.createComponent(LeaseListComponent);
    fixture.componentInstance.realEstateAssetId = 10;
    fixture.detectChanges();
    fixture.componentInstance.realEstateAssetId = null;
    fixture.componentInstance.ngOnChanges({
      realEstateAssetId: new SimpleChange(10, null, false),
    });
    expect(fixture.componentInstance.leases).toEqual([]);
  });

  it('should set leases to [] on load error', () => {
    leaseServiceSpy.getLeasesByRealEstateAssetId.and.returnValue(throwError(() => ({ status: 500 })));
    const fixture = TestBed.createComponent(LeaseListComponent);
    fixture.componentInstance.realEstateAssetId = 10;
    fixture.detectChanges();
    expect(fixture.componentInstance.leases).toEqual([]);
  });

  it('should navigate with realEstateAssetId query param on createNewLease', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    const comp = TestBed.createComponent(LeaseListComponent).componentInstance;
    comp.realEstateAssetId = 10;
    comp.createNewLease();
    expect(router.navigate).toHaveBeenCalledWith(['/lease/create'], {
      queryParams: { realEstateAssetId: 10 },
    });
  });

  it('should navigate with empty queryParams when no realEstateAssetId', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    const comp = TestBed.createComponent(LeaseListComponent).componentInstance;
    comp.createNewLease();
    expect(router.navigate).toHaveBeenCalledWith(['/lease/create'], { queryParams: {} });
  });

  it('should navigate with both ids on editLease', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    const comp = TestBed.createComponent(LeaseListComponent).componentInstance;
    comp.realEstateAssetId = 10;
    comp.editLease(LEASE_A);
    expect(router.navigate).toHaveBeenCalledWith(['/lease/create'], {
      queryParams: { realEstateAssetId: 10, leaseId: 1 },
    });
  });

  it('should remove deleted lease from the list', () => {
    const fixture = TestBed.createComponent(LeaseListComponent);
    fixture.componentInstance.realEstateAssetId = 10;
    fixture.detectChanges();
    fixture.componentInstance.deleteLease(LEASE_A);
    expect(fixture.componentInstance.leases).toEqual([LEASE_B]);
  });
});
