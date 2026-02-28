import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LeaseService } from '../Services/lease.service';
import { RealEstateService } from '../Services/real-estate.service';
import { LessorService } from '../Services/lessor.service';
import { Lease } from '../models/lease';
import { Lessor } from '../models/lessor';
import { LessorFormComponent } from '../lessor-form/lessor-form.component';

@Component({
  selector: 'app-lease-form',
  standalone: true,
  imports: [CommonModule, FormsModule, LessorFormComponent],
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
  lessors: Lessor[] = [];
  lessorSelection: number | 'new' | null = null;
  isLoadingLessors: boolean = false;
  newLessor: Lessor = this.getEmptyLessor();
  isOngoing: boolean = false;
  isEditing: boolean = false;
  leaseId: number | null = null;

  constructor(
    private leaseService: LeaseService,
    private activatedRoute: ActivatedRoute,
    private realEstateService: RealEstateService,
    private lessorService: LessorService,
    private router: Router,
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['realEstateAssetId']) {
      this.formData = {
        ...this.formData,
        realEstateAssetId: this.realEstateAssetId,
      };
      if (this.realEstateAssetId) {
        this.loadRealEstateName(this.realEstateAssetId);
        this.loadLessors(this.realEstateAssetId);
      }
    }
  }

  onSubmit(): void {
    if (this.isSubmitting) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    if (this.lessorSelection === 'new') {
      if (!this.isNewLessorValid()) {
        this.errorMessage = 'Lessor information is required';
        this.isSubmitting = false;
        return;
      }
      console.log('Creating new Lessor', this.newLessor);
      this.lessorService.createLessor(this.newLessor).subscribe({
        next: (lessor) => {
          this.lessors = [...this.lessors, lessor];
          this.formData.lessorId = lessor.id;
          this.lessorSelection = lessor.id;
          this.submitLease();
        },
        error: (err) => {
          console.error('Error creating Lessor', err);
          this.errorMessage = `Error occured during Lessor creation (${err.status})`;
          this.isSubmitting = false;
          this.formError.emit(this.errorMessage);
        },
      });
      return;
    }

    this.submitLease();
  }

  ngOnInit(): void {
    this.activatedRoute.queryParamMap.subscribe((params) => {
      const realEstateAssetIdParam = params.get('realEstateAssetId');
      const leaseIdParam = params.get('leaseId');
      if (realEstateAssetIdParam) {
        this.realEstateAssetId = Number(realEstateAssetIdParam);
        this.formData = {
          ...this.formData,
          realEstateAssetId: this.realEstateAssetId,
        };
        this.loadRealEstateName(this.realEstateAssetId);
        this.loadLessors(this.realEstateAssetId);
      }
      if (leaseIdParam) {
        this.leaseId = Number(leaseIdParam);
        this.isEditing = true;
        this.loadLease(this.leaseId);
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

  private getEmptyLessor(): Lessor {
    return {
      id: 0,
      name: '',
      address: '',
      city: '',
      postalCode: '',
      country: '',
      leases: [],
    };
  }

  private loadLessors(realEstateAssetId: number): void {
    this.isLoadingLessors = true;
    this.lessorService.getLessorsByRealEstateAssetId(realEstateAssetId).subscribe({
      next: (data) => {
        this.lessors = data;
        this.isLoadingLessors = false;
      },
      error: (err) => {
        console.error('Error loading Lessors', err);
        this.lessors = [];
        this.isLoadingLessors = false;
      },
    });
  }

  onLessorSelectionChange(value: number | 'new' | null): void {
    if (value === 'new') {
      this.formData.lessorId = null;
      this.lessorSelection = 'new';
      this.newLessor = this.getEmptyLessor();
      return;
    }
    this.formData.lessorId = typeof value === 'number' ? value : null;
  }

  private loadLease(leaseId: number): void {
    this.leaseService.getLeaseById(leaseId).subscribe({
      next: (lease) => {
        const isOngoing = lease.isOngoing ?? !lease.endDate;
        this.isOngoing = isOngoing;
        this.formData = {
          ...this.formData,
          leaseName: lease.leaseName ?? '',
          realEstateAssetId: lease.realEstateAssetId,
          lessorId: lease.lessorId,
          monthlyRent: lease.monthlyRent,
          monthlyCharge: lease.monthlyCharges,
          deposit: lease.deposit,
          rentIndexReference: lease.rentIndexReference,
          startDate: this.formatDateInput(lease.startDate),
          endDate: isOngoing ? '' : this.formatDateInput(lease.endDate),
        };
        this.lessorSelection = lease.lessorId;
        if (lease.realEstateAssetId && lease.realEstateAssetId !== this.realEstateAssetId) {
          this.realEstateAssetId = lease.realEstateAssetId;
          this.loadRealEstateName(lease.realEstateAssetId);
          this.loadLessors(lease.realEstateAssetId);
        }
      },
      error: (err) => {
        console.error('Error loading Lease', err);
      },
    });
  }

  private buildLeasePayload(): Lease {
    const endDateValue = !this.isOngoing && this.formData.endDate
      ? new Date(this.formData.endDate)
      : null;

    return {
      id: this.isEditing && this.leaseId ? this.leaseId : 0,
      realEstateAssetId: this.formData.realEstateAssetId ?? 0,
      lessorId: this.formData.lessorId ?? 0,
      leaseName: this.formData.leaseName.trim(),
      monthlyRent: this.formData.monthlyRent ?? 0,
      monthlyCharges: this.formData.monthlyCharge ?? 0,
      deposit: this.formData.deposit ?? 0,
      rentIndexReference: this.formData.rentIndexReference ?? 0,
      isOngoing: this.isOngoing,
      startDate: new Date(this.formData.startDate),
      endDate: endDateValue,
    };
  }

  private submitLease(): void {
    const leasePayload = this.buildLeasePayload();
    if (this.isEditing && !this.leaseId) {
      this.errorMessage = 'Missing lease id for update.';
      this.isSubmitting = false;
      this.formError.emit(this.errorMessage);
      return;
    }

    const request$ = this.isEditing
      ? this.leaseService.updateLease({ ...leasePayload, id: this.leaseId ?? leasePayload.id })
      : this.leaseService.createLease(leasePayload);

    request$.subscribe({
      next: (result) => {
        this.isSubmitting = false;
        this.formSubmitted.emit(result);
        if (this.formData.realEstateAssetId) {
          this.router.navigate(['/details', this.formData.realEstateAssetId]);
        }
      },
      error: (err) => {
        console.error(`Error ${this.isEditing ? 'updating' : 'creating'} Lease`, err);
        this.errorMessage = `Error occured on Lease ${this.isEditing ? 'update' : 'creation'} (${err.status})`;
        this.isSubmitting = false;
        this.formError.emit(this.errorMessage);
      },
    });
  }

  isNewLessorValid(): boolean {
    return Boolean(
      this.newLessor.name?.trim() &&
      this.newLessor.address?.trim() &&
      this.newLessor.city?.trim() &&
      this.newLessor.postalCode?.trim() &&
      this.newLessor.country?.trim()
    );
  }

  formatAmount(field: 'monthlyRent' | 'monthlyCharge' | 'deposit'): void {
    const value = this.formData[field];
    if (value === null || value === undefined || Number.isNaN(value)) {
      this.formData[field] = null;
      return;
    }
    const normalized = Number(value);
    this.formData[field] = Number(normalized.toFixed(2));
  }

  onOngoingToggle(isOngoing: boolean): void {
    this.isOngoing = isOngoing;
    if (isOngoing) {
      this.formData.endDate = '';
    }
  }

  private formatDateInput(value: Date | string | null | undefined): string {
    if (!value) return '';
    const date = value instanceof Date ? value : new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');
    return `${year}-${month}-${day}`;
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
