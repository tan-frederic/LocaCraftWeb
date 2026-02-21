import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Lease } from '../models/lease';

@Component({
  selector: 'app-lease-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './lease-list.component.html',
  styleUrl: './lease-list.component.css'
})
export class LeaseListComponent {
  @Input() realEstateAssetId: number | null = null;

  leases: Lease[] = [];

  constructor(private router: Router) {}

  createNewLease(): void {
    const queryParams = this.realEstateAssetId
      ? { realEstateAssetId: this.realEstateAssetId }
      : {};
    this.router.navigate(['/lease/create'], { queryParams });
  }
}
