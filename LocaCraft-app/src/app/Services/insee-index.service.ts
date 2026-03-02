import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InseeIndex } from '../models/insee-index';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InseeIndexService {

private apiUrl = `${environment.apiUrl}/InseeIndex`

  constructor(private http: HttpClient) { }

  getInseeIndexes(): Observable<InseeIndex[]> {
    console.log('Fetching Insee indexes from API:', this.apiUrl);
    return this.http.get<InseeIndex[]>(`${this.apiUrl}?LastNIndex=${20}`);
  }
}
