import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../environments/environment';

export interface RegisterRequest {
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiration: string;
}

export interface UserProfile {
  email: string;
  profilePictureBase64: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly tokenKey = 'auth_token';

  constructor(private http: HttpClient) {}

  register(request: RegisterRequest): Observable<{ id: string; email: string }> {
    return this.http.post<{ id: string; email: string }>(`${environment.apiUrl}/register`, request);
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/login`, request).pipe(
      tap(response => localStorage.setItem(this.tokenKey, response.token))
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getMe(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${environment.apiUrl}/account/me`);
  }

  changeEmail(newEmail: string): Observable<void> {
    return this.http.patch<void>(`${environment.apiUrl}/account/email`, { newEmail });
  }

  changePassword(currentPassword: string, newPassword: string): Observable<void> {
    return this.http.patch<void>(`${environment.apiUrl}/account/password`, { currentPassword, newPassword });
  }

  updateProfilePicture(profilePictureBase64: string): Observable<void> {
    return this.http.patch<void>(`${environment.apiUrl}/account/profile-picture`, { profilePictureBase64 });
  }
}
