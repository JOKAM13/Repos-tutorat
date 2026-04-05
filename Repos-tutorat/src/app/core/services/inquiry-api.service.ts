import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { ContactMessagePayload, TutorApplicationPayload, TutorRequestPayload } from '../models/forms.models';

@Injectable({ providedIn: 'root' })
export class InquiryApiService {
  constructor(private readonly http: HttpClient) {}

  createTutorRequest(payload: TutorRequestPayload): Observable<unknown> {
    return this.http.post(`${API_BASE_URL}/tutor-requests`, payload);
  }

  createTutorApplication(payload: TutorApplicationPayload): Observable<unknown> {
    return this.http.post(`${API_BASE_URL}/tutor-applications`, payload);
  }

  createContactMessage(payload: ContactMessagePayload): Observable<unknown> {
    return this.http.post(`${API_BASE_URL}/contact-messages`, payload);
  }
}
