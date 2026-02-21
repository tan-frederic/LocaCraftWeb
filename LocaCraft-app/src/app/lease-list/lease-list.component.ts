import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Lease } from '../models/lease';
import { LeaseService } from '../Services/lease.service';

@Component({
  selector: 'app-lease-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './lease-list.component.html',
  styleUrl: './lease-list.component.css'
})
export class LeaseListComponent implements OnInit, OnChanges {
  @Input() realEstateAssetId: number | null = null;

  leases: Lease[] = [];

  constructor(private router: Router, private leaseService: LeaseService) {}

  ngOnInit(): void {
    if (this.realEstateAssetId) {
      this.loadLeases(this.realEstateAssetId);
    } else {
      this.leases = [];
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['realEstateAssetId']) {
      if (this.realEstateAssetId) {
        this.loadLeases(this.realEstateAssetId);
      } else {
        this.leases = [];
      }
    }
  }

  createNewLease(): void {
    const queryParams = this.realEstateAssetId
      ? { realEstateAssetId: this.realEstateAssetId }
      : {};
    this.router.navigate(['/lease/create'], { queryParams });
  }

  editLease(lease: Lease): void {
    const queryParams = {
      realEstateAssetId: this.realEstateAssetId ?? lease.realEstateAssetId,
      leaseId: lease.id,
    };
    this.router.navigate(['/lease/create'], { queryParams });
  }

  deleteLease(lease: Lease): void {
    this.leaseService.deleteLease(lease.id).subscribe({
      next: () => {
        this.leases = this.leases.filter((l) => l.id !== lease.id);
      },
      error: (err) => {
        console.error('Error deleting Lease', err);
      },
    });
  }

  private loadLeases(realEstateAssetId: number): void {
    this.leaseService.getLeasesByRealEstateAssetId(realEstateAssetId).subscribe({
      next: (data) => {
        this.leases = data;
      },
      error: (err) => {
        console.error('Error loading Leases', err);
        this.leases = [];
      },
    });
  }
}
