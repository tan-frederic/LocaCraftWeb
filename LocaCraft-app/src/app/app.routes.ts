import { Routes } from '@angular/router';
import { RealEstateListComponent } from './real-estate-list/real-estate-list.component';
import { RealEstateFormComponent } from './real-estate-form/real-estate-form.component';
import { RealEstateDetailsComponent } from './real-estate-details/real-estate-details.component';
import { LeaseFormComponent } from './lease-form/lease-form.component';
import { InseeIndexListComponent } from './insee-index-list/insee-index-list.component';
import { LessorFormComponent } from './lessor-form/lessor-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    {path: '', redirectTo: 'home', pathMatch: 'full'},
    {path: 'home', component: RealEstateListComponent, canActivate: [authGuard]},
    {path: 'create', component: RealEstateFormComponent, canActivate: [authGuard]},
    {path: 'details/:id', component: RealEstateDetailsComponent, canActivate: [authGuard]},
    {path: 'lease/create', component: LeaseFormComponent, canActivate: [authGuard]},
    {path: 'insee', component: InseeIndexListComponent, canActivate: [authGuard]},
    {path: 'lessor/create', component: LessorFormComponent, canActivate: [authGuard]},
    {path: 'login', component: LoginFormComponent}
];
