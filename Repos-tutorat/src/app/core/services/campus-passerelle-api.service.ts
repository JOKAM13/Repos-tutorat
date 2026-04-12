import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { CampusFiliereDetailsResponse, FiliereCampus } from '../models/campus-passerelle.models';

import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class CampusPasserelleApiService {
  constructor(private readonly http: HttpClient) {}

  getActiveFilieres(): Observable<FiliereCampus[]> {
    return this.http.get<FiliereCampus[]>(`${API_BASE_URL}/campus-passerelle/filieres/actives`);
  }

  getFiliereDetails(filiereId: string): Observable<CampusFiliereDetailsResponse> {
    return this.http.get<CampusFiliereDetailsResponse>(`${API_BASE_URL}/campus-passerelle/filieres/${filiereId}`);
  }
}
