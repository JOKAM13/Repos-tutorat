import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { CampusAdminDashboardResponse, CoursCampus, FiliereCampus } from '../models/campus-passerelle.models';

@Injectable({ providedIn: 'root' })
export class CampusPasserelleAdminApiService {
  private readonly tokenStorageKey = 'etude-reussie-admin-token';

  constructor(private readonly http: HttpClient) {}

  getDashboard(): Observable<CampusAdminDashboardResponse> {
    return this.http.get<CampusAdminDashboardResponse>(`${API_BASE_URL}/admin/campus-passerelle/dashboard`, {
      headers: this.createHeaders()
    });
  }

  createFiliere(payload: Omit<FiliereCampus, 'id'>): Observable<unknown> {
    return this.http.post(`${API_BASE_URL}/admin/campus-passerelle/filieres`, payload, {
      headers: this.createHeaders()
    });
  }

  updateFiliere(id: string, payload: Omit<FiliereCampus, 'id'>): Observable<unknown> {
    return this.http.put(`${API_BASE_URL}/admin/campus-passerelle/filieres/${id}`, payload, {
      headers: this.createHeaders()
    });
  }

  deleteFiliere(id: string): Observable<unknown> {
    return this.http.delete(`${API_BASE_URL}/admin/campus-passerelle/filieres/${id}`, {
      headers: this.createHeaders()
    });
  }

  createCours(payload: Omit<CoursCampus, 'id'>): Observable<unknown> {
    return this.http.post(`${API_BASE_URL}/admin/campus-passerelle/cours`, payload, {
      headers: this.createHeaders()
    });
  }

  updateCours(id: string, payload: Omit<CoursCampus, 'id'>): Observable<unknown> {
    return this.http.put(`${API_BASE_URL}/admin/campus-passerelle/cours/${id}`, payload, {
      headers: this.createHeaders()
    });
  }

  deleteCours(id: string): Observable<unknown> {
    return this.http.delete(`${API_BASE_URL}/admin/campus-passerelle/cours/${id}`, {
      headers: this.createHeaders()
    });
  }

  assignerCours(filiereId: string, coursId: string): Observable<unknown> {
    return this.http.post(
      `${API_BASE_URL}/admin/campus-passerelle/assignations`,
      { filiereId, coursId },
      { headers: this.createHeaders() }
    );
  }

  retirerCours(filiereId: string, coursId: string): Observable<unknown> {
    return this.http.delete(`${API_BASE_URL}/admin/campus-passerelle/filieres/${filiereId}/cours/${coursId}`, {
      headers: this.createHeaders()
    });
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders({
      'X-Admin-Token': localStorage.getItem(this.tokenStorageKey) ?? ''
    });
  }
}
