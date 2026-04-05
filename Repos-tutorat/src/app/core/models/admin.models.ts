export interface AdminLoginPayload {
  username: string;
  password: string;
}

export interface AdminLoginResponse {
  token: string;
  username: string;
}

export interface AdminTutorRequest {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  school?: string;
  classLevel: string;
  subject?: string;
  need?: string;
  mode?: string;
  availability?: string;
  city?: string;
  createdAtUtc: string;
}

export interface AdminTutorApplication {
  id: string;
  fullName: string;
  email: string;
  phone?: string;
  subjects: string;
  coveredLevels?: string;
  institutions?: string;
  about?: string;
  mode?: string;
  city?: string;
  availability?: string;
  createdAtUtc: string;
}

export interface AdminContactMessage {
  id: string;
  fullName: string;
  email: string;
  phone?: string;
  message: string;
  createdAtUtc: string;
}

export interface AdminDashboardResponse {
  tutorRequests: AdminTutorRequest[];
  tutorApplications: AdminTutorApplication[];
  contactMessages: AdminContactMessage[];
}
