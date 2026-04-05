import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { InquiryApiService } from '../../core/services/inquiry-api.service';

interface BenefitItem {
  title: string;
  description: string;
}

interface PricePlan {
  level: string;
  online: string;
  inPerson: string;
  note: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  readonly benefits: BenefitItem[] = [
    {
      title: 'Faire une demande en quelques minutes',
      description: 'L’élève ou le parent remplit un formulaire simple, puis nous analysons le besoin afin de proposer un accompagnement adapté au niveau, à la matière et aux objectifs.'
    },
    {
      title: 'Des tuteurs sérieux et engagés',
      description: 'Nous mettons en avant des tuteurs motivés, capables d’accompagner les élèves avec sérieux, pédagogie et régularité selon leurs disponibilités et leurs forces.'
    },
    {
      title: 'Un service rassurant dès le premier contact',
      description: 'Nous misons sur une présentation claire, professionnelle et humaine pour inspirer confiance aux élèves, aux parents et aux futurs tuteurs.'
    }
  ];

  readonly pricePlans: PricePlan[] = [
    {
      level: 'Primaire et secondaire',
      online: '30 $ / h',
      inPerson: '35 $ / h',
      note: 'Approche claire, reprise des bases et aide aux devoirs.'
    },
    {
      level: 'Cégep et université',
      online: '35 $ / h',
      inPerson: '40 $ / h',
      note: 'Pour les cours plus avancés, la méthodologie de travail, la préparation aux examens et l’accompagnement ciblé selon la matière.'
    },
    {
      level: 'Forfaits',
      online: '20 h : -5 %',
      inPerson: '30 h : -10 %',
      note: 'Une formule avantageuse pour les élèves qui ont besoin d’un suivi régulier et d’une progression sur plusieurs semaines.'
    }
  ];

  readonly tutorRequestModes = ['En ligne', 'En présentiel', 'Hybride'];
  readonly tutorApplicationModes = ['En ligne', 'En présentiel', 'Hybride'];

  readonly tutorRequestForm = this.formBuilder.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    school: [''],
    classLevel: ['', [Validators.required]],
    subject: [''],
    need: [''],
    mode: ['En ligne'],
    availability: [''],
    city: ['']
  });

  readonly tutorApplicationForm = this.formBuilder.group({
    fullName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    subjects: ['', [Validators.required]],
    coveredLevels: [''],
    institutions: [''],
    about: [''],
    mode: ['En ligne'],
    city: [''],
    availability: ['']
  });

  readonly contactForm = this.formBuilder.group({
    fullName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    message: ['', [Validators.required]]
  });

  tutorRequestState: 'idle' | 'success' | 'error' = 'idle';
  tutorApplicationState: 'idle' | 'success' | 'error' = 'idle';
  contactState: 'idle' | 'success' | 'error' = 'idle';

  loadingTutorRequest = false;
  loadingTutorApplication = false;
  loadingContact = false;

  constructor(
    private readonly formBuilder: NonNullableFormBuilder,
    private readonly inquiryApiService: InquiryApiService
  ) {}

  submitTutorRequest(): void {
    if (this.tutorRequestForm.invalid) {
      this.tutorRequestForm.markAllAsTouched();
      return;
    }

    this.loadingTutorRequest = true;
    this.tutorRequestState = 'idle';

    this.inquiryApiService
      .createTutorRequest(this.tutorRequestForm.getRawValue())
      .pipe(finalize(() => (this.loadingTutorRequest = false)))
      .subscribe({
        next: () => {
          this.tutorRequestState = 'success';
          this.tutorRequestForm.reset({ mode: 'En ligne' });
        },
        error: () => {
          this.tutorRequestState = 'error';
        }
      });
  }

  submitTutorApplication(): void {
    if (this.tutorApplicationForm.invalid) {
      this.tutorApplicationForm.markAllAsTouched();
      return;
    }

    this.loadingTutorApplication = true;
    this.tutorApplicationState = 'idle';

    this.inquiryApiService
      .createTutorApplication(this.tutorApplicationForm.getRawValue())
      .pipe(finalize(() => (this.loadingTutorApplication = false)))
      .subscribe({
        next: () => {
          this.tutorApplicationState = 'success';
          this.tutorApplicationForm.reset({ mode: 'En ligne' });
        },
        error: () => {
          this.tutorApplicationState = 'error';
        }
      });
  }

  submitContactMessage(): void {
    if (this.contactForm.invalid) {
      this.contactForm.markAllAsTouched();
      return;
    }

    this.loadingContact = true;
    this.contactState = 'idle';

    this.inquiryApiService
      .createContactMessage(this.contactForm.getRawValue())
      .pipe(finalize(() => (this.loadingContact = false)))
      .subscribe({
        next: () => {
          this.contactState = 'success';
          this.contactForm.reset();
        },
        error: () => {
          this.contactState = 'error';
        }
      });
  }

  hasError(formName: 'tutorRequest' | 'tutorApplication' | 'contact', controlName: string): boolean {
    const form: any =
      formName === 'tutorRequest'
        ? this.tutorRequestForm
        : formName === 'tutorApplication'
          ? this.tutorApplicationForm
          : this.contactForm;

    const control = form.controls?.[controlName];
    return Boolean(control && control.invalid && (control.touched || control.dirty));
  }
}
