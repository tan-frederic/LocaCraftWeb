import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { LeaseService } from '../Services/lease.service';
import { RealEstateService } from '../Services/real-estate.service';
import { Lease } from '../models/lease';
import { RealEstateAsset } from '../models/real-estate-assets';
import { Lessor } from '../models/lessor';

@Component({
  selector: 'app-lease-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './lease-form.component.html',
  styleUrl: './lease-form.component.css'
})
export class LeaseFormComponent implements OnChanges, OnInit {
  @Input() realEstateAssetId: number | null = null;
  @Input() isInDrawer: boolean = false;

  @Output() formSubmitted = new EventEmitter<Lease>();
  @Output() formError = new EventEmitter<string>();
  @Output() formCancelled = new EventEmitter<void>();

  formData: LeaseFormData = this.getEmptyFormData();
  errorMessage: string = '';
  isSubmitting: boolean = false;
  realEstateName: string = '';
  isLoadingRealEstate: boolean = false;

  constructor(
    private leaseService: LeaseService,
    private activatedRoute: ActivatedRoute,
    private realEstateService: RealEstateService,
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['realEstateAssetId']) {
      this.formData = {
        ...this.formData,
        realEstateAssetId: this.realEstateAssetId,
      };
    }
  }

  onSubmit(): void {
    if (this.isSubmitting) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    const leaseToCreate: Lease = {
      id: 0,
      realEstateAssetId: this.formData.realEstateAssetId ?? 0,
      realEstateAsset: this.getEmptyRealEstateAsset(),
      lessorId: this.formData.lessorId ?? 0,
      lessor: this.getEmptyLessor(),
      leaseName: this.formData.leaseName.trim(),
      monthlyRent: this.formData.monthlyRent ?? 0,
      monthlyCharge: this.formData.monthlyCharge ?? 0,
      deposit: this.formData.deposit ?? 0,
      rentIndexReference: this.formData.rentIndexReference ?? 0,
      tenants: [],
      startDate: new Date(this.formData.startDate),
      endDate: new Date(this.formData.endDate),
    };

    this.leaseService.createLease(leaseToCreate).subscribe({
      next: (result) => {
        this.isSubmitting = false;
        this.formSubmitted.emit(result);
      },
      error: (err) => {
        console.error('Error creating Lease', err);
        this.errorMessage = `Error occured (${err.status})`;
        this.isSubmitting = false;
        this.formError.emit(this.errorMessage);
      },
    });
  }

  ngOnInit(): void {
    this.activatedRoute.queryParamMap.subscribe((params) => {
      const realEstateAssetIdParam = params.get('realEstateAssetId');
      if (realEstateAssetIdParam) {
        this.realEstateAssetId = Number(realEstateAssetIdParam);
        this.formData = {
          ...this.formData,
          realEstateAssetId: this.realEstateAssetId,
        };
        this.loadRealEstateName(this.realEstateAssetId);
      }
    });
  }

  private loadRealEstateName(realEstateAssetId: number): void {
    this.isLoadingRealEstate = true;
    this.realEstateService.getRealEstateAssetById(realEstateAssetId).subscribe({
      next: (asset) => {
        this.realEstateName = asset.name;
        this.isLoadingRealEstate = false;
      },
      error: (err) => {
        console.error('Error loading Real Estate Asset', err);
        this.realEstateName = '';
        this.errorMessage = `Error occured (${err.status})`;
        this.isLoadingRealEstate = false;
      },
    });
  }

  onCancel(): void {
    this.formCancelled.emit();
  }

  private getEmptyFormData(): LeaseFormData {
    return {
      leaseName: '',
      realEstateAssetId: this.realEstateAssetId,
      lessorId: null,
      monthlyRent: null,
      monthlyCharge: null,
      deposit: null,
      rentIndexReference: null,
      startDate: '',
      endDate: '',
    };
  }

  private getEmptyRealEstateAsset(): RealEstateAsset {
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

  private getEmptyLessor(): Lessor {
    return {
      id: 0,
      name: '',
      adress: '',
      city: '',
      postalCode: '',
      country: '',
      leases: [],
    };
  }
}

interface LeaseFormData {
  leaseName: string;
  realEstateAssetId: number | null;
  lessorId: number | null;
  monthlyRent: number | null;
  monthlyCharge: number | null;
  deposit: number | null;
  rentIndexReference: number | null;
  startDate: string;
  endDate: string;
}
