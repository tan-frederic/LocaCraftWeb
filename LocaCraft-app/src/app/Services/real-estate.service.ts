import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RealEstateAsset } from '../models/real-estate-assets';

@Injectable({
  providedIn: 'root'
})
export class RealEstateService {

  private apiUrl = `${environment.apiUrl}/RealEstateAsset`

  constructor(private http: HttpClient) { }

  getRealEstateAssets(): Observable<RealEstateAsset[]>{
    return this.http.get<RealEstateAsset[]>(this.apiUrl);
  }

  getRealEstateAssetById(id: number): Observable<RealEstateAsset>{
    return this.http.get<RealEstateAsset>(this.apiUrl);
  }

  createRealEstateAsset(realEstate: RealEstateAsset): Observable<RealEstateAsset>{
    return this.http.post<RealEstateAsset>(this.apiUrl, realEstate);
  }

  deleteRealEstateAsset(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
  }

  editRealEstateAsset(realEstate: RealEstateAsset): Observable<RealEstateAsset> {
    return this.http.put<RealEstateAsset>(`${this.apiUrl}/${realEstate.id}`, realEstate)
  }
}
