import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { AdminComponent } from './pages/admin/admin.component';
import { CampusPasserelleComponent } from './pages/campus-passerelle/campus-passerelle.component';
import { AdminCampusPasserelleComponent } from './pages/admin-campus-passerelle/admin-campus-passerelle.component';
import { PolitiqueConfidentialiteComponent } from './pages/politique-confidentialite/politique-confidentialite.component';
export const routes: Routes = [
  { path: '', component: HomeComponent, title: 'Étude Réussie | Trouver un tuteur' },
  { path: 'admin', component: AdminComponent, title: 'Étude Réussie | Administration' },
  { path: 'campus-passerelle', component: CampusPasserelleComponent, title: 'Étude Réussie | Campus Passerelle' },
  { path: 'espace-prive-campus-passerelle-a82m', component: AdminCampusPasserelleComponent, title: 'Étude Réussie | Admin Campus Passerelle' },
  { path: 'politique-confidentialite', component: PolitiqueConfidentialiteComponent, title: 'Étude Réussie | Politique de confidentialité' },
  { path: '**', redirectTo: '' }
];
