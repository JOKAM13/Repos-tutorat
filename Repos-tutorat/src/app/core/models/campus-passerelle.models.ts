export interface RessourceCampus {
  id: string;
  titre: string;
  type?: string;
  lien?: string;
  description?: string;
}

export type NiveauFiliere = 'college' | 'universite' | 'autre';
export type NiveauCours = 'debutant' | 'intermediaire' | 'avance';

export interface FiliereCampus {
  id: string;
  nom: string;
  niveau: NiveauFiliere | string;
  description: string;
  notions: string[];
  outils: string[];
  conseils: string[];
  ressources: RessourceCampus[];
  couleur: string;
  actif: boolean;
  coursIds: string[];
}

export interface CoursCampus {
  id: string;
  nom: string;
  categorie: string;
  niveau: NiveauCours | string;
  description: string;
  objectifs: string[];
  ressources: RessourceCampus[];
  dureeEstimee: string;
}

export interface CampusFiliereDetailsResponse {
  filiere: FiliereCampus;
  cours: CoursCampus[];
}

export interface CampusAdminDashboardResponse {
  filieres: FiliereCampus[];
  cours: CoursCampus[];
}
