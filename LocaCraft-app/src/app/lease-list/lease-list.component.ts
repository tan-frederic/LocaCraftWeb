import { Component } from '@angular/core';
import { Lease } from '../models/lease';

@Component({
  selector: 'app-lease-list',
  standalone: true,
  imports: [],
  templateUrl: './lease-list.component.html',
  styleUrl: './lease-list.component.css'
})
export class LeaseListComponent {
  leases: Lease[] = []; // This will hold the list of leases
  
  createNewLease() {
    // Logic to create a new lease
  }
}
