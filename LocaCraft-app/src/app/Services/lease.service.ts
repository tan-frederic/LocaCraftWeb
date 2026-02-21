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

  createLease(lease: Lease): Observable<Lease> {
    return this.http.post<Lease>(this.apiUrl, lease);
  }
}
