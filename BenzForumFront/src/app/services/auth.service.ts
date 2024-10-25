import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http'; // Import HttpClientModule and HttpClient
import { environment } from '../../environments/environment';
import { Credentials } from '../models/credentials.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl
  public islogin = new Subject<string>();

  constructor(private http: HttpClient) {}

  // User registration
  register(userData: Credentials): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/register`, userData);
  }

  // User login
  login(credentials: Credentials): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/login`, credentials);
  }

  // Logout user
  logout(): void {
    localStorage.removeItem('token');
    this.islogin.next('logout');
  }

  // Check if user is authenticated
  isAuthenticated(): boolean {
    return Boolean(localStorage.getItem('token'));
  }

  // Get JWT token
  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
