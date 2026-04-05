import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { AdminContactMessage, AdminDashboardResponse, AdminTutorApplication, AdminTutorRequest } from '../../core/models/admin.models';
import { AdminApiService } from '../../core/services/admin-api.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  readonly loginForm = this.formBuilder.group({
    username: ['admin', [Validators.required]],
    password: ['', [Validators.required]]
  });

  dashboard: AdminDashboardResponse | null = null;
  loginError = '';
  dashboardError = '';
  loadingLogin = false;
  loadingDashboard = false;
  authenticated = false;
  actionMessage = '';

  constructor(
    private readonly formBuilder: NonNullableFormBuilder,
    private readonly adminApiService: AdminApiService
  ) {}

  ngOnInit(): void {
    const token = this.adminApiService.getToken();
    if (token) {
      this.authenticated = true;
      this.loadDashboard();
    }
  }

  submitLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loadingLogin = true;
    this.loginError = '';

    this.adminApiService
      .login(this.loginForm.getRawValue())
      .pipe(finalize(() => (this.loadingLogin = false)))
      .subscribe({
        next: (response) => {
          this.adminApiService.saveToken(response.token);
          this.authenticated = true;
          this.loadDashboard();
        },
        error: () => {
          this.authenticated = false;
          this.loginError = 'Login ou mot de passe invalide.';
        }
      });
  }

  loadDashboard(): void {
    this.loadingDashboard = true;
    this.dashboardError = '';
    this.actionMessage = '';

    this.adminApiService
      .getDashboard()
      .pipe(finalize(() => (this.loadingDashboard = false)))
      .subscribe({
        next: (response) => {
          this.dashboard = response;
          this.authenticated = true;
        },
        error: () => {
          this.dashboard = null;
          this.authenticated = false;
          this.adminApiService.clearToken();
          this.dashboardError = 'Impossible de charger les données administrateur.';
        }
      });
  }


  deleteTutorRequest(item: AdminTutorRequest): void {
    if (!confirm(`Supprimer la demande de ${item.firstName} ${item.lastName} ?`)) {
      return;
    }

    this.adminApiService.deleteTutorRequest(item.id).subscribe({
      next: () => {
        if (this.dashboard) {
          this.dashboard.tutorRequests = this.dashboard.tutorRequests.filter(entry => entry.id !== item.id);
        }
        this.actionMessage = 'La demande élève a bien été supprimée.';
      },
      error: () => {
        this.actionMessage = 'Impossible de supprimer cette demande pour le moment.';
      }
    });
  }

  deleteTutorApplication(item: AdminTutorApplication): void {
    if (!confirm(`Supprimer la candidature de ${item.fullName} ?`)) {
      return;
    }

    this.adminApiService.deleteTutorApplication(item.id).subscribe({
      next: () => {
        if (this.dashboard) {
          this.dashboard.tutorApplications = this.dashboard.tutorApplications.filter(entry => entry.id !== item.id);
        }
        this.actionMessage = 'La candidature tuteur a bien été supprimée.';
      },
      error: () => {
        this.actionMessage = 'Impossible de supprimer cette candidature pour le moment.';
      }
    });
  }

  deleteContactMessage(item: AdminContactMessage): void {
    if (!confirm(`Supprimer le message de ${item.fullName} ?`)) {
      return;
    }

    this.adminApiService.deleteContactMessage(item.id).subscribe({
      next: () => {
        if (this.dashboard) {
          this.dashboard.contactMessages = this.dashboard.contactMessages.filter(entry => entry.id !== item.id);
        }
        this.actionMessage = 'Le message contact a bien été supprimé.';
      },
      error: () => {
        this.actionMessage = 'Impossible de supprimer ce message pour le moment.';
      }
    });
  }

  logout(): void {
    this.adminApiService.logout().subscribe({
      next: () => this.finalizeLogout(),
      error: () => this.finalizeLogout()
    });
  }

  private finalizeLogout(): void {
    this.adminApiService.clearToken();
    this.authenticated = false;
    this.dashboard = null;
    this.actionMessage = '';
    this.loginForm.reset({ username: 'admin', password: '' });
  }

  trackById(_: number, item: { id: string }): string {
    return item.id;
  }
}
