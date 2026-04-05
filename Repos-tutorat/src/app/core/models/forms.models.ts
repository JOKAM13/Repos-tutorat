export interface TutorRequestPayload {
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
}

export interface TutorApplicationPayload {
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
}

export interface ContactMessagePayload {
  fullName: string;
  email: string;
  phone?: string;
  message: string;
}
