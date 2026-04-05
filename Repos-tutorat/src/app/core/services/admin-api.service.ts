import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { AdminContactMessage, AdminDashboardResponse, AdminLoginPayload, AdminLoginResponse, AdminTutorApplication, AdminTutorRequest } from '../models/admin.models';

@Injectable({ providedIn: 'root' })
export class AdminApiService {
  private readonly tokenStorageKey = 'etude-reussie-admin-token';

  constructor(private readonly http: HttpClient) {}

  login(payload: AdminLoginPayload): Observable<AdminLoginResponse> {
    return this.http.post<AdminLoginResponse>(`${API_BASE_URL}/admin/login`, payload);
  }

  getDashboard(): Observable<AdminDashboardResponse> {
    return this.http.get<AdminDashboardResponse>(`${API_BASE_URL}/admin/dashboard`, {
      headers: this.createHeaders()
    });
  }

  logout(): Observable<unknown> {
    return this.http.post(`${API_BASE_URL}/admin/logout`, {}, {
      headers: this.createHeaders()
    });
  }

  deleteTutorRequest(id: AdminTutorRequest['id']): Observable<unknown> {
    return this.http.delete(`${API_BASE_URL}/admin/tutor-requests/${id}`, {
      headers: this.createHeaders()
    });
  }

  deleteTutorApplication(id: AdminTutorApplication['id']): Observable<unknown> {
    return this.http.delete(`${API_BASE_URL}/admin/tutor-applications/${id}`, {
      headers: this.createHeaders()
    });
  }

  deleteContactMessage(id: AdminContactMessage['id']): Observable<unknown> {
    return this.http.delete(`${API_BASE_URL}/admin/contact-messages/${id}`, {
      headers: this.createHeaders()
    });
  }

  saveToken(token: string): void {
    localStorage.setItem(this.tokenStorageKey, token);
  }

  getToken(): string {
    return localStorage.getItem(this.tokenStorageKey) ?? '';
  }

  clearToken(): void {
    localStorage.removeItem(this.tokenStorageKey);
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders({
      'X-Admin-Token': this.getToken()
    });
  }
}
