import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { CoursCampus, FiliereCampus } from '../../core/models/campus-passerelle.models';
import { CampusPasserelleApiService } from '../../core/services/campus-passerelle-api.service';

@Component({
  selector: 'app-campus-passerelle',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './campus-passerelle.component.html',
  styleUrls: ['./campus-passerelle.component.css']
})
export class CampusPasserelleComponent implements OnInit {
  filieres: FiliereCampus[] = [];
  selectedFiliereId = '';
  selectedFiliere: FiliereCampus | null = null;
  selectedCours: CoursCampus[] = [];

  loadingFilieres = false;
  loadingDetails = false;
  errorMessage = '';

  constructor(private readonly campusApi: CampusPasserelleApiService) {}

  ngOnInit(): void {
    this.loadFilieres();
  }

  selectFiliere(id: string): void {
    if (!id) {
      this.selectedFiliereId = '';
      this.selectedFiliere = null;
      this.selectedCours = [];
      return;
    }

    this.selectedFiliereId = id;
    this.loadFiliereDetails(id);
  }

  trackById(_: number, item: { id: string }): string {
    return item.id;
  }

  private loadFilieres(): void {
    this.loadingFilieres = true;
    this.errorMessage = '';

    this.campusApi
      .getActiveFilieres()
      .pipe(finalize(() => (this.loadingFilieres = false)))
      .subscribe({
        next: (filieres) => {
          this.filieres = filieres;

          if (!filieres.length) {
            this.selectedFiliereId = '';
            this.selectedFiliere = null;
            this.selectedCours = [];
            return;
          }

          this.selectFiliere(filieres[0].id);
        },
        error: () => {
          this.filieres = [];
          this.selectedFiliere = null;
          this.selectedCours = [];
          this.errorMessage = 'Impossible de charger les filières pour le moment.';
        }
      });
  }

  private loadFiliereDetails(filiereId: string): void {
    this.loadingDetails = true;
    this.errorMessage = '';

    this.campusApi
      .getFiliereDetails(filiereId)
      .pipe(finalize(() => (this.loadingDetails = false)))
      .subscribe({
        next: (response) => {
          this.selectedFiliere = response.filiere;
          this.selectedCours = response.cours;
        },
        error: () => {
          this.selectedFiliere = null;
          this.selectedCours = [];
          this.errorMessage = 'Impossible de charger les détails de cette filière.';
        }
      });
  }
}
