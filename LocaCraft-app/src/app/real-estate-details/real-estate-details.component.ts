import { Component } from '@angular/core';
import { RealEstateFormComponent } from '../real-estate-form/real-estate-form.component';

@Component({
  selector: 'app-real-estate-details',
  standalone: true,
  imports: [RealEstateFormComponent],
  templateUrl: './real-estate-details.component.html',
  styleUrl: './real-estate-details.component.css'
})
export class RealEstateDetailsComponent {

}
