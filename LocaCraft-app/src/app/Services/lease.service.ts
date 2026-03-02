import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Lease } from '../models/lease';

@Injectable({
  providedIn: 'root'
})
export class LeaseService {
  private apiUrl = `${environment.apiUrl}/Lease`;

  constructor(private http: HttpClient) {}

  getLeasesByRealEstateAssetId(realEstateAssetId: number): Observable<Lease[]> {
    return this.http.get<Lease[]>(`${this.apiUrl}/realestateasset/${realEstateAssetId}`);
  }

  getLeaseById(id: number): Observable<Lease> {
    return this.http.get<Lease>(`${this.apiUrl}/${id}`);
  }

  createLease(lease: Lease): Observable<Lease> {
    return this.http.post<Lease>(this.apiUrl, lease);
  }

  updateLease(lease: Lease): Observable<Lease> {
    return this.http.put<Lease>(`${this.apiUrl}/${lease.id}`, lease);
  }

  deleteLease(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
