import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { of } from 'rxjs';

import { RealEstateDetailsComponent } from './real-estate-details.component';
import { RealEstateService } from '../Services/real-estate.service';
import { LeaseService } from '../Services/lease.service';

describe('RealEstateDetailsComponent', () => {
  function createFixture(paramId: string | null) {
    TestBed.overrideProvider(ActivatedRoute, {
      useValue: { paramMap: of(convertToParamMap(paramId ? { id: paramId } : {})) },
    });
    return TestBed.createComponent(RealEstateDetailsComponent);
  }

  beforeEach(async () => {
    const realEstateSpy = jasmine.createSpyObj('RealEstateService', ['getRealEstateAssetById']);
    const leaseSpy = jasmine.createSpyObj('LeaseService', ['getLeasesByRealEstateAssetId']);
    leaseSpy.getLeasesByRealEstateAssetId.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [RealEstateDetailsComponent],
      providers: [
        provideRouter([]),
        provideLocationMocks(),
        { provide: RealEstateService, useValue: realEstateSpy },
        { provide: LeaseService, useValue: leaseSpy },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    expect(createFixture('1').componentInstance).toBeTruthy();
  });

  it('should extract realEstateId as a number from route param on ngOnInit', () => {
    const fixture = createFixture('42');
    fixture.detectChanges();
    expect(fixture.componentInstance.realEstateId).toBe(42);
  });

  it('should set realEstateId to null when route param is absent', () => {
    const fixture = createFixture(null);
    fixture.detectChanges();
    expect(fixture.componentInstance.realEstateId).toBeNull();
  });
});
