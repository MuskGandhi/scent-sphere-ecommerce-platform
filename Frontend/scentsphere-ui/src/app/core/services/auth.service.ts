import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { LoginRequest, RegisterRequest, AuthResponse } from '../../models/auth';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request);
  }

  register(request: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, request);
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem('token');

    localStorage.removeItem('userName');

    localStorage.removeItem('role');
  }

  getUserName(): string {
    return localStorage.getItem('userName') || '';
  }

  getUserInitial(): string {
    const userName = this.getUserName();

    return userName ? userName.charAt(0).toUpperCase() : 'U';
  }
  getRole(): string {
    return localStorage.getItem('role') || '';
  }
  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }
}
