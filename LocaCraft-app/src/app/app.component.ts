import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { RealEstateListComponent } from './real-estate-list/real-estate-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RealEstateListComponent, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'LocaCraft-app';
}
