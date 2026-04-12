import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import {
  CoursCampus,
  FiliereCampus,
  NiveauCours,
  NiveauFiliere,
  RessourceCampus
} from '../../core/models/campus-passerelle.models';
import { AdminApiService } from '../../core/services/admin-api.service';
import { CampusPasserelleAdminApiService } from '../../core/services/campus-passerelle-admin-api.service';

type AdminTab = 'filieres' | 'cours' | 'assignations';

@Component({
  selector: 'app-admin-campus-passerelle',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-campus-passerelle.component.html',
  styleUrls: ['./admin-campus-passerelle.component.css']
})
export class AdminCampusPasserelleComponent implements OnInit {
  activeTab: AdminTab = 'filieres';

  filieres: FiliereCampus[] = [];
  cours: CoursCampus[] = [];

  editingFiliereId: string | null = null;
  editingCoursId: string | null = null;
  selectedAssignationFiliereId = '';

  authenticated = false;
  loadingLogin = false;
  loadingDashboard = false;
  loadingAction = false;
  loginError = '';
  dashboardError = '';
  actionMessage = '';

  readonly filiereLevels: NiveauFiliere[] = ['college', 'universite', 'autre'];
  readonly coursLevels: NiveauCours[] = ['debutant', 'intermediaire', 'avance'];

  readonly loginForm = this.formBuilder.group({
    username: ['admin', [Validators.required]],
    password: ['', [Validators.required]]
  });

  readonly filiereForm = this.formBuilder.group({
    nom: ['', [Validators.required]],
    niveau: ['universite' as NiveauFiliere, [Validators.required]],
    description: ['', [Validators.required]],
    notionsText: [''],
    outilsText: [''],
    conseilsText: [''],
    ressourcesText: [''],
    couleur: ['#10b981', [Validators.required]],
    actif: [true]
  });

  readonly coursForm = this.formBuilder.group({
    nom: ['', [Validators.required]],
    categorie: ['', [Validators.required]],
    niveau: ['debutant' as NiveauCours, [Validators.required]],
    description: ['', [Validators.required]],
    objectifsText: [''],
    ressourcesText: [''],
    dureeEstimee: ['', [Validators.required]]
  });

  constructor(
    private readonly formBuilder: NonNullableFormBuilder,
    private readonly adminApiService: AdminApiService,
    private readonly campusAdminApiService: CampusPasserelleAdminApiService
  ) {}

  ngOnInit(): void {
    const token = this.adminApiService.getToken();

    if (token) {
      this.authenticated = true;
      this.loadDashboard();
    }
  }

  get assignedCours(): CoursCampus[] {
    const filiere = this.selectedAssignationFiliere;
    if (!filiere) {
      return [];
    }

    return this.cours.filter((item) => filiere.coursIds.includes(item.id));
  }

  get availableCours(): CoursCampus[] {
    return this.cours;
  }

  get selectedAssignationFiliere(): FiliereCampus | undefined {
    return this.filieres.find((item) => item.id === this.selectedAssignationFiliereId);
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

  logout(): void {
    this.adminApiService.logout().subscribe({
      next: () => this.finalizeLogout(),
      error: () => this.finalizeLogout()
    });
  }

  loadDashboard(): void {
    this.loadingDashboard = true;
    this.dashboardError = '';
    this.actionMessage = '';

    this.campusAdminApiService
      .getDashboard()
      .pipe(finalize(() => (this.loadingDashboard = false)))
      .subscribe({
        next: (response) => {
          this.filieres = response.filieres;
          this.cours = response.cours;
          this.authenticated = true;
          this.preserveSelectedFiliere();
        },
        error: (error: HttpErrorResponse) => {
          this.filieres = [];
          this.cours = [];
          this.dashboardError = error.error?.message || 'Impossible de charger les données Campus Passerelle.';

          if (error.status === 401) {
            this.finalizeLogout();
            this.loginError = 'Ta session a expiré. Reconnecte-toi.';
          }
        }
      });
  }

  setTab(tab: AdminTab): void {
    this.activeTab = tab;
  }

  submitFiliere(): void {
    this.dashboardError = '';
    this.actionMessage = '';

    if (this.filiereForm.invalid) {
      this.filiereForm.markAllAsTouched();
      this.dashboardError = 'Remplis tous les champs obligatoires du formulaire filière.';
      return;
    }

    const raw = this.filiereForm.getRawValue();
    const existingCoursIds = this.editingFiliereId
      ? this.filieres.find((item) => item.id === this.editingFiliereId)?.coursIds ?? []
      : [];

    const payload: Omit<FiliereCampus, 'id'> = {
      nom: raw.nom.trim(),
      niveau: raw.niveau,
      description: raw.description.trim(),
      notions: this.toArray(raw.notionsText),
      outils: this.toArray(raw.outilsText),
      conseils: this.toArray(raw.conseilsText),
      ressources: this.toResourceList(raw.ressourcesText),
      couleur: raw.couleur,
      actif: raw.actif,
      coursIds: existingCoursIds
    };

    this.loadingAction = true;

    const request$ = this.editingFiliereId
      ? this.campusAdminApiService.updateFiliere(this.editingFiliereId, payload)
      : this.campusAdminApiService.createFiliere(payload);

    request$
      .pipe(finalize(() => (this.loadingAction = false)))
      .subscribe({
        next: () => {
          this.dashboardError = '';
          this.actionMessage = this.editingFiliereId
            ? 'La filière a bien été mise à jour.'
            : 'La filière a bien été créée.';
          this.cancelFiliereEdit();
          this.loadDashboard();
        },
        error: (error: HttpErrorResponse) => {
          this.actionMessage = '';
          this.dashboardError =
            error.error?.message ||
            'Impossible d’enregistrer la filière pour le moment.';
        }
      });
  }

  editFiliere(item: FiliereCampus): void {
    this.dashboardError = '';
    this.actionMessage = '';

    this.editingFiliereId = item.id;
    this.filiereForm.reset({
      nom: item.nom,
      niveau: item.niveau as NiveauFiliere,
      description: item.description,
      notionsText: item.notions.join('\n'),
      outilsText: item.outils.join('\n'),
      conseilsText: item.conseils.join('\n'),
      ressourcesText: this.resourcesToLines(item.ressources),
      couleur: item.couleur,
      actif: item.actif
    });
    this.activeTab = 'filieres';
  }

  deleteFiliere(item: FiliereCampus): void {
    if (!confirm(`Supprimer la filière ${item.nom} ?`)) {
      return;
    }

    this.loadingAction = true;
    this.dashboardError = '';
    this.actionMessage = '';

    this.campusAdminApiService
      .deleteFiliere(item.id)
      .pipe(finalize(() => (this.loadingAction = false)))
      .subscribe({
        next: () => {
          this.actionMessage = 'La filière a bien été supprimée.';
          this.cancelFiliereEdit();
          this.loadDashboard();
        },
        error: (error: HttpErrorResponse) => {
          this.dashboardError =
            error.error?.message ||
            'Impossible de supprimer cette filière pour le moment.';
        }
      });
  }

  cancelFiliereEdit(): void {
    this.dashboardError = '';
    this.actionMessage = '';
    this.editingFiliereId = null;
    this.filiereForm.reset({
      nom: '',
      niveau: 'universite',
      description: '',
      notionsText: '',
      outilsText: '',
      conseilsText: '',
      ressourcesText: '',
      couleur: '#10b981',
      actif: true
    });
  }

  submitCours(): void {
    this.dashboardError = '';
    this.actionMessage = '';

    if (this.coursForm.invalid) {
      this.coursForm.markAllAsTouched();
      this.dashboardError = 'Remplis tous les champs obligatoires du formulaire cours.';
      return;
    }

    const raw = this.coursForm.getRawValue();
    const payload: Omit<CoursCampus, 'id'> = {
      nom: raw.nom.trim(),
      categorie: raw.categorie.trim(),
      niveau: raw.niveau,
      description: raw.description.trim(),
      objectifs: this.toArray(raw.objectifsText),
      ressources: this.toResourceList(raw.ressourcesText),
      dureeEstimee: raw.dureeEstimee.trim()
    };

    this.loadingAction = true;

    const request$ = this.editingCoursId
      ? this.campusAdminApiService.updateCours(this.editingCoursId, payload)
      : this.campusAdminApiService.createCours(payload);

    request$
      .pipe(finalize(() => (this.loadingAction = false)))
      .subscribe({
        next: () => {
          this.dashboardError = '';
          this.actionMessage = this.editingCoursId
            ? 'Le cours a bien été mis à jour.'
            : 'Le cours a bien été créé.';
          this.cancelCoursEdit();
          this.loadDashboard();
        },
        error: (error: HttpErrorResponse) => {
          this.actionMessage = '';
          this.dashboardError =
            error.error?.message ||
            'Impossible d’enregistrer ce cours pour le moment.';
        }
      });
  }

  editCours(item: CoursCampus): void {
    this.dashboardError = '';
    this.actionMessage = '';

    this.editingCoursId = item.id;
    this.coursForm.reset({
      nom: item.nom,
      categorie: item.categorie,
      niveau: item.niveau as NiveauCours,
      description: item.description,
      objectifsText: item.objectifs.join('\n'),
      ressourcesText: this.resourcesToLines(item.ressources),
      dureeEstimee: item.dureeEstimee
    });
    this.activeTab = 'cours';
  }

  deleteCours(item: CoursCampus): void {
    if (!confirm(`Supprimer le cours ${item.nom} ?`)) {
      return;
    }

    this.loadingAction = true;
    this.dashboardError = '';
    this.actionMessage = '';

    this.campusAdminApiService
      .deleteCours(item.id)
      .pipe(finalize(() => (this.loadingAction = false)))
      .subscribe({
        next: () => {
          this.actionMessage = 'Le cours a bien été supprimé.';
          this.cancelCoursEdit();
          this.loadDashboard();
        },
        error: (error: HttpErrorResponse) => {
          this.dashboardError =
            error.error?.message ||
            'Impossible de supprimer ce cours pour le moment.';
        }
      });
  }

  cancelCoursEdit(): void {
    this.dashboardError = '';
    this.actionMessage = '';
    this.editingCoursId = null;
    this.coursForm.reset({
      nom: '',
      categorie: '',
      niveau: 'debutant',
      description: '',
      objectifsText: '',
      ressourcesText: '',
      dureeEstimee: ''
    });
  }

  assignCours(coursId: string): void {
    if (!this.selectedAssignationFiliereId) {
      return;
    }

    this.loadingAction = true;
    this.dashboardError = '';
    this.actionMessage = '';

    this.campusAdminApiService
      .assignerCours(this.selectedAssignationFiliereId, coursId)
      .pipe(finalize(() => (this.loadingAction = false)))
      .subscribe({
        next: () => {
          this.actionMessage = 'Le cours a bien été assigné.';
          this.loadDashboard();
        },
        error: (error: HttpErrorResponse) => {
          this.dashboardError =
            error.error?.message ||
            'Impossible d’assigner ce cours pour le moment.';
        }
      });
  }

  removeCours(coursId: string): void {
    if (!this.selectedAssignationFiliereId) {
      return;
    }

    this.loadingAction = true;
    this.dashboardError = '';
    this.actionMessage = '';

    this.campusAdminApiService
      .retirerCours(this.selectedAssignationFiliereId, coursId)
      .pipe(finalize(() => (this.loadingAction = false)))
      .subscribe({
        next: () => {
          this.actionMessage = 'Le cours a bien été retiré.';
          this.loadDashboard();
        },
        error: (error: HttpErrorResponse) => {
          this.dashboardError =
            error.error?.message ||
            'Impossible de retirer ce cours pour le moment.';
        }
      });
  }

  isAssigned(coursId: string): boolean {
    return this.assignedCours.some((item) => item.id === coursId);
  }

  trackById(_: number, item: { id: string }): string {
    return item.id;
  }

  private finalizeLogout(): void {
    this.adminApiService.clearToken();
    this.authenticated = false;
    this.filieres = [];
    this.cours = [];
    this.selectedAssignationFiliereId = '';
    this.editingFiliereId = null;
    this.editingCoursId = null;
    this.actionMessage = '';
    this.dashboardError = '';
    this.loginForm.reset({ username: 'admin', password: '' });
  }

  private preserveSelectedFiliere(): void {
    if (!this.filieres.length) {
      this.selectedAssignationFiliereId = '';
      return;
    }

    const exists = this.filieres.some((item) => item.id === this.selectedAssignationFiliereId);
    this.selectedAssignationFiliereId = exists ? this.selectedAssignationFiliereId : this.filieres[0].id;
  }

  private toArray(value: string): string[] {
    return value
      .split('\n')
      .map((item) => item.trim())
      .filter(Boolean);
  }

  private toResourceList(value: string): RessourceCampus[] {
    return value
      .split('\n')
      .map((line) => line.trim())
      .filter(Boolean)
      .map((line, index) => {
        const parts = line.split('|').map((part) => part.trim());
        return {
          id: `res-${Date.now()}-${index}`,
          titre: parts[0] || 'Ressource',
          description: parts[1] || '',
          lien: parts[2] || ''
        };
      });
  }

  private resourcesToLines(resources: RessourceCampus[]): string {
    return resources
      .map((item) => [item.titre, item.description ?? '', item.lien ?? ''].join(' | '))
      .join('\n');
  }
}