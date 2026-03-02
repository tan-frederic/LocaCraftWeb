import { CommonModule } from '@angular/common';
import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RealEstateAsset } from '../models/real-estate-assets';
import { RealEstateService } from '../Services/real-estate.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Lease } from '../models/lease';

@Component({
  selector: 'app-real-estate-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './real-estate-form.component.html',
  styleUrl: './real-estate-form.component.css'
})

export class RealEstateFormComponent implements OnInit, OnChanges{

  @Input() assetToEdit: RealEstateAsset | null = null;

  @Input() isInDrawer: boolean = false;

  @Output() formSubmitted = new EventEmitter<RealEstateAsset>();

  @Output() formError = new EventEmitter<string>();

  @Output() formCancelled = new EventEmitter<void>();

  realEstate: RealEstateAsset = this.getEmptyRealEstate();

  isEditing: boolean = false;

  errorMessage: string = "";
  isSubmitting: boolean = false;

  constructor(private realEstateAssetService: RealEstateService,
              private router: Router,
              private activatedRoute: ActivatedRoute){}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['assetToEdit']) {
      if (this.assetToEdit) {
        this.realEstate = { ...this.assetToEdit };
        this.isEditing = true;
      } else {
        this.realEstate = this.getEmptyRealEstate();
        this.isEditing = false;
      }
    }
  }

  ngOnInit(): void {
    // Only load from route if not in drawer
    if (!this.isInDrawer) {
      this.activatedRoute.paramMap.subscribe((result) => {
        const id = result.get("id");
        if (id) {
          this.isEditing = true;
          
          this.realEstateAssetService.getRealEstateAssetById(Number(id)).subscribe({
            next: (result) => {
                this.realEstate = result;
              },
            error: (err) => {
              console.error("Error loading Real Estate Asset", err)
              this.errorMessage = `Error occured (${err.status})`
            }          
          });
        }
      });
    }
  }

  onSubmit(): void {
    if (this.isSubmitting) return;
    
    this.isSubmitting = true;
    this.errorMessage = "";

    if (this.isEditing){

    } else {
      this.realEstateAssetService.createRealEstateAsset(this.realEstate).subscribe({
        next: (result) => {
          this.isSubmitting = false;
          this.formSubmitted.emit(result);
        },
        error: (err) => {
          console.error("Error creating Real Estate Asset", err);
          this.errorMessage = `Error occured (${err.status})`
          this.isSubmitting = false;
          this.formError.emit(this.errorMessage);
        }
      })
    }
  }

  onCancel(): void {
    this.formCancelled.emit();
  }

  private getEmptyRealEstate(): RealEstateAsset {
    return {
      id: 0,
      name: "",
      description: "",
      address: "",
      addressComplement: "",
      postalCode: "",
      city: "",
      country: "",
      leases: [],
    };
  }
}
