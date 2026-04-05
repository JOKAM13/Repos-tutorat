import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { AdminComponent } from './pages/admin/admin.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, title: 'Étude Réussie | Trouver un tuteur' },
  { path: 'admin', component: AdminComponent, title: 'Étude Réussie | Administration' },
  { path: '**', redirectTo: '' }
];
