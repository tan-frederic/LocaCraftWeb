import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { of, throwError } from 'rxjs';
import { SimpleChange } from '@angular/core';

import { RealEstateFormComponent } from './real-estate-form.component';
import { RealEstateService } from '../Services/real-estate.service';
import { RealEstateAsset } from '../models/real-estate-assets';

const ASSET: RealEstateAsset = {
  id: 1, name: 'Appartement Paris', description: 'Studio',
  address: '1 rue X', addressComplement: '', postalCode: '75001',
  city: 'Paris', country: 'France', leases: [],
};

describe('RealEstateFormComponent', () => {
  let serviceSpy: jasmine.SpyObj<RealEstateService>;

  function setup(routeParamId?: string) {
    TestBed.overrideProvider(ActivatedRoute, {
      useValue: {
        paramMap: of(convertToParamMap(routeParamId ? { id: routeParamId } : {})),
      },
    });
    return TestBed.createComponent(RealEstateFormComponent);
  }

  beforeEach(async () => {
    serviceSpy = jasmine.createSpyObj('RealEstateService', [
      'getRealEstateAssetById', 'createRealEstateAsset',
    ]);
    serviceSpy.getRealEstateAssetById.and.returnValue(of(ASSET));
    serviceSpy.createRealEstateAsset.and.returnValue(of(ASSET));

    await TestBed.configureTestingModule({
      imports: [RealEstateFormComponent],
      providers: [
        provideRouter([]),
        provideLocationMocks(),
        { provide: RealEstateService, useValue: serviceSpy },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    expect(setup().componentInstance).toBeTruthy();
  });

  it('ngOnChanges should copy assetToEdit into form and set isEditing to true', () => {
    const comp = setup().componentInstance;
    comp.assetToEdit = ASSET; // must be set before ngOnChanges checks this.assetToEdit
    comp.ngOnChanges({ assetToEdit: new SimpleChange(null, ASSET, true) });
    expect(comp.realEstate).toEqual(ASSET);
    expect(comp.isEditing).toBeTrue();
  });

  it('ngOnChanges should reset form and isEditing when assetToEdit becomes null', () => {
    const comp = setup().componentInstance;
    // First load the asset
    comp.assetToEdit = ASSET;
    comp.ngOnChanges({ assetToEdit: new SimpleChange(null, ASSET, true) });
    // Then nullify it
    comp.assetToEdit = null;
    comp.ngOnChanges({ assetToEdit: new SimpleChange(ASSET, null, false) });
    expect(comp.realEstate.id).toBe(0);
    expect(comp.isEditing).toBeFalse();
  });

  it('ngOnInit should skip route param when isInDrawer is true', () => {
    const fixture = setup('5');
    fixture.componentInstance.isInDrawer = true;
    fixture.detectChanges();
    expect(serviceSpy.getRealEstateAssetById).not.toHaveBeenCalled();
  });

  it('ngOnInit should load asset from route param when not in drawer', () => {
    const fixture = setup('1');
    fixture.componentInstance.isInDrawer = false;
    fixture.detectChanges();
    expect(serviceSpy.getRealEstateAssetById).toHaveBeenCalledWith(1);
    expect(fixture.componentInstance.realEstate).toEqual(ASSET);
    expect(fixture.componentInstance.isEditing).toBeTrue();
  });

  it('ngOnInit should set errorMessage on load failure', () => {
    serviceSpy.getRealEstateAssetById.and.returnValue(throwError(() => ({ status: 404 })));
    const fixture = setup('1');
    fixture.componentInstance.isInDrawer = false;
    fixture.detectChanges();
    expect(fixture.componentInstance.errorMessage).toContain('404');
  });

  it('onSubmit should call createRealEstateAsset in create mode', () => {
    const comp = setup().componentInstance;
    comp.onSubmit();
    expect(serviceSpy.createRealEstateAsset).toHaveBeenCalledWith(comp.realEstate);
  });

  it('onSubmit should emit formSubmitted on success', () => {
    const comp = setup().componentInstance;
    let emitted: RealEstateAsset | undefined;
    comp.formSubmitted.subscribe((a: RealEstateAsset) => (emitted = a));
    comp.onSubmit();
    expect(emitted).toEqual(ASSET);
  });

  it('onSubmit should emit formError and set errorMessage on failure', () => {
    serviceSpy.createRealEstateAsset.and.returnValue(throwError(() => ({ status: 500 })));
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
    expect(serviceSpy.createRealEstateAsset).not.toHaveBeenCalled();
  });

  it('onCancel should emit formCancelled', () => {
    const comp = setup().componentInstance;
    let cancelled = false;
    comp.formCancelled.subscribe(() => (cancelled = true));
    comp.onCancel();
    expect(cancelled).toBeTrue();
  });
});
