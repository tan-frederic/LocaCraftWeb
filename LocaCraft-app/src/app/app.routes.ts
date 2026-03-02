import { Routes } from '@angular/router';
import { RealEstateListComponent } from './real-estate-list/real-estate-list.component';
import { RealEstateFormComponent } from './real-estate-form/real-estate-form.component';
import { RealEstateDetailsComponent } from './real-estate-details/real-estate-details.component';
import { LeaseFormComponent } from './lease-form/lease-form.component';
import { InseeIndexListComponent } from './insee-index-list/insee-index-list.component';
import { LessorFormComponent } from './lessor-form/lessor-form.component';

export const routes: Routes = [
    {path: ``, component: RealEstateListComponent},
    {path: `create`, component: RealEstateFormComponent},
    {path: `details/:id`, component: RealEstateDetailsComponent},
    {path: `lease/create`, component: LeaseFormComponent},
    {path: `insee`, component: InseeIndexListComponent},
    {path: `lessor/create`, component: LessorFormComponent}
];
