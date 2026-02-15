import { Routes } from '@angular/router';
import { RealEstateListComponent } from './real-estate-list/real-estate-list.component';
import { RealEstateFormComponent } from './real-estate-form/real-estate-form.component';

export const routes: Routes = [
    {path: ``, component: RealEstateListComponent},
    {path: `create`, component: RealEstateFormComponent}
];
