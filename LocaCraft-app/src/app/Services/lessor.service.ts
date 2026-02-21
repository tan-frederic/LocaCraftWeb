import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Lessor } from '../models/lessor';

@Injectable({
  providedIn: 'root'
})
export class LessorService {
  private apiUrl = `${environment.apiUrl}/Lessor`;

  constructor(private http: HttpClient) {}

  getLessors(): Observable<Lessor[]> {
    return this.http.get<Lessor[]>(this.apiUrl);
  }

  getLessorsByRealEstateAssetId(realEstateAssetId: number): Observable<Lessor[]> {
    return this.http.get<Lessor[]>(`${this.apiUrl}?realEstateAssetId=${realEstateAssetId}`);
  }

  createLessor(lessor: Lessor): Observable<Lessor> {
    return this.http.post<Lessor>(this.apiUrl, lessor);
  }
}
