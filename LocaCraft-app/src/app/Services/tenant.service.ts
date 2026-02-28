import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Tenant } from '../models/tenant';

@Injectable({
  providedIn: 'root'
})
export class TenantService {
  private apiUrl = `${environment.apiUrl}/Tenant`;

  constructor(private http: HttpClient) {}

  getTenantByLeaseId(leaseId: number): Observable<Tenant> {
    return this.http.get<Tenant>(`${this.apiUrl}/lease/${leaseId}`);
  }

  getTenantsByLeaseId(leaseId: number): Observable<Tenant[]> {
    return this.http.get<Tenant[]>(`${this.apiUrl}/lease/${leaseId}/all`);
  }

  createTenant(tenant: Tenant): Observable<Tenant> {
    return this.http.post<Tenant>(this.apiUrl, tenant);
  }
}
