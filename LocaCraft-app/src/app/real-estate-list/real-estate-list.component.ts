import { Component, OnInit, ViewChild } from '@angular/core';
import { RealEstateAsset } from '../models/real-estate-assets';
import { RealEstateService } from '../Services/real-estate.service';
import { RealEstateFormComponent } from '../real-estate-form/real-estate-form.component';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LateralDrawerComponent } from '../lateral-drawer/lateral-drawer.component';

@Component({
  selector: 'app-real-estate-list',
  standalone: true,
  imports: [CommonModule, LateralDrawerComponent],
  templateUrl: './real-estate-list.component.html',
  styleUrl: './real-estate-list.component.css'
})

export class RealEstateListComponent implements OnInit {
  realEstateAssets: RealEstateAsset[] = [];
  assetToDelete: RealEstateAsset | null = null;

  // Messages
  successMessage: string | null = null;
  errorMessage: string | null = null;

  // Drawer state
  componentToDisplay = RealEstateFormComponent;
  componentData: any = { assetToEdit: null, isInDrawer: true };
  assetToEdit: RealEstateAsset | null = null;

  realEstate: RealEstateAsset = this.getEmptyRealEstate();

  @ViewChild('drawer') drawer!: LateralDrawerComponent;

  constructor(private realEstateService: RealEstateService,
              private router: Router,
              private modalService: NgbModal) {
    
  }

  ngOnInit(){
    this.loadRealEstateAssets();
  }

  loadRealEstateAssets(): void {
    this.realEstateService.getRealEstateAssets().subscribe({
      next: (data: RealEstateAsset[]) => {
        this.realEstateAssets = data;
        console.log('Real estate assets loaded:', data);
      },
      error: (err) => {
        console.error('Error loading real estate assets:', err);
        this.showError('Erreur lors du chargement des propriétés');
      }
    });
  }

  editRealEstateAsset(asset: RealEstateAsset): void {
    this.router.navigate(['/details', asset.id]);
  }

  openDeleteModal(content: any, asset: RealEstateAsset) {
    this.assetToDelete = asset;
    this.modalService.open(content, { centered: true });
  }


  confirmDelete() {
    if (this.assetToDelete) {
        this.realEstateService.deleteRealEstateAsset(this.assetToDelete.id).subscribe({
          next: () => {
            this.realEstateAssets = this.realEstateAssets.filter(a => a.id !== this.assetToDelete!.id);
            console.log('Real estate deleted succeffully');
            this.assetToDelete = null;
            this.modalService.dismissAll();
        },
        error: (err) =>{
          console.error(`Error deleteing Real estate asset`, err);
        }
      });
    }
  }

  cancelDelete() {
    this.assetToDelete = null;
    this.modalService.dismissAll();
  }  

  createRealEstateAsset(): void {
    this.assetToEdit = null; // Mode création
    this.componentData = { assetToEdit: null, isInDrawer: true };
    this.openDrawer();
  }

  onSubmit(): void {
  }

  onFormSubmitted(asset: RealEstateAsset): void {
    if (this.assetToEdit) {
      // Mode édition: mettre à jour dans la liste
      const index = this.realEstateAssets.findIndex(a => a.id === asset.id);
      if (index !== -1) {
        this.realEstateAssets[index] = asset;
      }
      this.showSuccess(`"${asset.name}" a été modifié avec succès`);
    } else {
      // Mode création: ajouter à la liste
      this.realEstateAssets.push(asset);
      this.showSuccess(`"${asset.name}" a été créé avec succès`);
    }
    
    this.drawer.closeDrawer();
  }

  onFormError(errorMsg: string): void {
    // L'erreur est déjà affichée dans le formulaire
    console.error('Form error:', errorMsg);
  }

  onFormCancelled(): void {
    this.drawer.closeDrawer();
  }

  // Drawer

  openDrawer(): void {
    this.drawer.openDrawer();
    document.body.style.overflow = 'hidden';
  }

  closeDrawer(): void {
    this.assetToEdit = null;
    document.body.style.overflow = 'auto';
  }

  // Return an empty RealEstate
  private getEmptyRealEstate(): RealEstateAsset {
    return {
      id: 0,
      name: '',
      description: '',
      address: '',
      addressComplement: '',
      postalCode: '',
      city: '',
      country: '',
      leases: [],
    };
  }

  // Display Success Message
  private showSuccess(message: string): void {
    this.successMessage = message;
    this.errorMessage = null;
    
    setTimeout(() => {
      this.successMessage = null;
    }, 5000);
  }

  // Display Error Message
  private showError(message: string): void {
    this.errorMessage = message;
    this.successMessage = null;
    
    setTimeout(() => {
      this.errorMessage = null;
    }, 5000);
  }

  private clearMessages(): void {
    this.successMessage = null;
    this.errorMessage = null;
  }
}
