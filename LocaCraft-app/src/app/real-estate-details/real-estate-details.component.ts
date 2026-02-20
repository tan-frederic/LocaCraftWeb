import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RealEstateFormComponent } from '../real-estate-form/real-estate-form.component';
import { LeaseListComponent } from '../lease-list/lease-list.component';
import { RealEstateService } from '../Services/real-estate.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-real-estate-details',
  standalone: true,
  imports: [RealEstateFormComponent, LeaseListComponent, CommonModule],
  templateUrl: './real-estate-details.component.html',
  styleUrl: './real-estate-details.component.css'
})
export class RealEstateDetailsComponent implements OnInit {

  constructor(private realEstateAssetService: RealEstateService,
              private router: Router,
              private activatedRoute: ActivatedRoute){ }

  ngOnInit(): void {
    
  }

}
