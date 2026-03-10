import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { of, throwError } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { RealEstateListComponent } from './real-estate-list.component';
import { RealEstateService } from '../Services/real-estate.service';
import { RealEstateAsset } from '../models/real-estate-assets';

const ASSET_A: RealEstateAsset = {
  id: 1, name: 'Appart A', description: '', address: '1 rue X',
  addressComplement: '', postalCode: '75001', city: 'Paris', country: 'France', leases: [],
};
const ASSET_B: RealEstateAsset = { ...ASSET_A, id: 2, name: 'Appart B' };

describe('RealEstateListComponent', () => {
  let serviceSpy: jasmine.SpyObj<RealEstateService>;
  let modalSpy: jasmine.SpyObj<NgbModal>;

  beforeEach(async () => {
    serviceSpy = jasmine.createSpyObj('RealEstateService', [
      'getRealEstateAssets', 'deleteRealEstateAsset',
    ]);
    serviceSpy.getRealEstateAssets.and.returnValue(of([ASSET_A, ASSET_B]));
    serviceSpy.deleteRealEstateAsset.and.returnValue(of(undefined));

    modalSpy = jasmine.createSpyObj('NgbModal', ['open', 'dismissAll']);

    await TestBed.configureTestingModule({
      imports: [RealEstateListComponent],
      providers: [
        provideRouter([]),
        provideLocationMocks(),
        { provide: RealEstateService, useValue: serviceSpy },
        { provide: NgbModal, useValue: modalSpy },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    expect(TestBed.createComponent(RealEstateListComponent).componentInstance).toBeTruthy();
  });

  it('should load real estate assets on ngOnInit', () => {
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    expect(serviceSpy.getRealEstateAssets).toHaveBeenCalled();
    expect(fixture.componentInstance.realEstateAssets).toEqual([ASSET_A, ASSET_B]);
  });

  it('should set errorMessage on load failure', () => {
    serviceSpy.getRealEstateAssets.and.returnValue(throwError(() => ({ status: 500 })));
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    expect(fixture.componentInstance.errorMessage).toContain('Erreur');
  });

  it('editRealEstateAsset should navigate to /details/:id', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    const comp = TestBed.createComponent(RealEstateListComponent).componentInstance;
    comp.editRealEstateAsset(ASSET_A);
    expect(router.navigate).toHaveBeenCalledWith(['/details', 1]);
  });

  it('createRealEstateAsset should set componentData and open the drawer', () => {
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    const comp = fixture.componentInstance;
    spyOn(comp.drawer, 'openDrawer');
    comp.createRealEstateAsset();
    expect(comp.componentData).toEqual({ assetToEdit: null, isInDrawer: true });
    expect(comp.drawer.openDrawer).toHaveBeenCalled();
  });

  it('onFormSubmitted should add new asset to list in create mode', () => {
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    const comp = fixture.componentInstance;
    comp.assetToEdit = null;
    spyOn(comp.drawer, 'closeDrawer');
    const newAsset: RealEstateAsset = { ...ASSET_A, id: 99, name: 'Nouveau' };
    comp.onFormSubmitted(newAsset);
    expect(comp.realEstateAssets).toContain(newAsset);
  });

  it('onFormSubmitted should update existing asset in edit mode', () => {
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    const comp = fixture.componentInstance;
    comp.assetToEdit = ASSET_A;
    spyOn(comp.drawer, 'closeDrawer');
    const updated: RealEstateAsset = { ...ASSET_A, name: 'Modifié' };
    comp.onFormSubmitted(updated);
    expect(comp.realEstateAssets[0].name).toBe('Modifié');
  });

  it('onFormSubmitted should show a success message that auto-dismisses after 5s', fakeAsync(() => {
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    const comp = fixture.componentInstance;
    comp.assetToEdit = null;
    spyOn(comp.drawer, 'closeDrawer');
    comp.onFormSubmitted(ASSET_A);
    expect(comp.successMessage).toContain('Appart A');
    tick(5000);
    expect(comp.successMessage).toBeNull();
  }));

  it('onFormCancelled should close the drawer', () => {
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    const comp = fixture.componentInstance;
    spyOn(comp.drawer, 'closeDrawer');
    comp.onFormCancelled();
    expect(comp.drawer.closeDrawer).toHaveBeenCalled();
  });

  it('confirmDelete should remove asset from list and dismiss modal', () => {
    const fixture = TestBed.createComponent(RealEstateListComponent);
    fixture.detectChanges();
    const comp = fixture.componentInstance;
    comp.assetToDelete = ASSET_A;
    comp.confirmDelete();
    expect(serviceSpy.deleteRealEstateAsset).toHaveBeenCalledWith(1);
    expect(comp.realEstateAssets).not.toContain(ASSET_A);
    expect(modalSpy.dismissAll).toHaveBeenCalled();
    expect(comp.assetToDelete).toBeNull();
  });

  it('cancelDelete should clear assetToDelete and dismiss modal', () => {
    const comp = TestBed.createComponent(RealEstateListComponent).componentInstance;
    comp.assetToDelete = ASSET_A;
    comp.cancelDelete();
    expect(comp.assetToDelete).toBeNull();
    expect(modalSpy.dismissAll).toHaveBeenCalled();
  });

  it('closeDrawer should reset assetToEdit', () => {
    const comp = TestBed.createComponent(RealEstateListComponent).componentInstance;
    comp.assetToEdit = ASSET_A;
    comp.closeDrawer();
    expect(comp.assetToEdit).toBeNull();
  });
});
