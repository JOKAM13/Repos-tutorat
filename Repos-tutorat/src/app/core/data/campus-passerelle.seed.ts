import { CampusPasserelleStore } from '../models/campus-passerelle.models';

export const CAMPUS_PASSERELLE_SEED: CampusPasserelleStore = {
  filieres: [
    {
      id: 'informatique',
      nom: 'Informatique',
      niveau: 'universite',
      description:
        'Prépare ton arrivée en informatique avec une base claire sur la logique, les outils et la méthode de travail attendue à l’université.',
      notions: [
        'Logique algorithmique',
        'Variables, conditions et boucles',
        'Fonctions et décomposition',
        'Structure d’un programme',
        'Résolution de problèmes'
      ],
      outils: ['Visual Studio Code', 'Git', 'GitHub', 'Terminal', 'Navigateur web'],
      conseils: [
        'Commence à pratiquer un peu chaque semaine avant la rentrée.',
        'Ne cherche pas à tout mémoriser : comprends la logique.',
        'Habitue-toi à lire les consignes et à découper un problème.',
        'Prépare ton ordinateur et tes outils avant le début des cours.'
      ],
      ressources: [
        {
          id: 'res-info-1',
          titre: 'Checklist de préparation avant la rentrée',
          description: 'Les éléments essentiels à préparer avant de commencer.'
        },
        {
          id: 'res-info-2',
          titre: 'Mini-guide de démarrage en programmation',
          description: 'Un support simple pour revoir les bases.'
        }
      ],
      couleur: '#10b981',
      actif: true,
      coursIds: ['cours-programmation', 'cours-outils-numeriques']
    }
  ],
  cours: [
    {
      id: 'cours-programmation',
      nom: 'Programmation - bases essentielles',
      categorie: 'Préparation académique',
      niveau: 'debutant',
      description:
        'Un cours préparatoire pour comprendre les fondements de la programmation avant les premiers cours.',
      objectifs: [
        'Comprendre la logique d’un programme',
        'Écrire des conditions simples',
        'Utiliser des boucles',
        'Découper un problème en étapes'
      ],
      ressources: [
        {
          id: 'res-cours-1',
          titre: 'Série d’exercices de logique',
          description: 'Petits exercices pour s’entraîner progressivement.'
        }
      ],
      dureeEstimee: '4 à 6 heures'
    },
    {
      id: 'cours-outils-numeriques',
      nom: 'Prise en main des outils numériques',
      categorie: 'Méthodologie',
      niveau: 'debutant',
      description:
        'Découvre les outils qui te seront utiles dès le début de session pour travailler de façon efficace.',
      objectifs: [
        'Installer un éditeur de code',
        'Comprendre Git et GitHub',
        'Organiser ses fichiers',
        'Utiliser les plateformes de travail en équipe'
      ],
      ressources: [
        {
          id: 'res-cours-2',
          titre: 'Guide d’installation rapide',
          description: 'Étapes pour préparer ton environnement de travail.'
        }
      ],
      dureeEstimee: '2 à 3 heures'
    }
  ]
};
