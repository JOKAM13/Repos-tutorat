# Étude Réussie — version minimale fullstack

Cette version garde uniquement l'essentiel :
- une belle page d'accueil
- le formulaire **Trouver un tuteur**
- le formulaire **Devenir tuteur**
- la section **Prix**
- la section **Contact**
- un backend ASP.NET pour enregistrer les soumissions dans une base SQLite

## Structure

- `frontend/` : interface Angular
- `backend/` : API ASP.NET + base SQLite

## Lancer le backend

```bash
cd backend/src/EtudeReussie.Api
dotnet restore
dotnet run
```

API prévue sur : `http://localhost:5167`

La base sera créée automatiquement au premier lancement dans :
- `backend/src/EtudeReussie.Api/App_Data/etude-reussie.db`

## Lancer le frontend

Dans un autre terminal :

```bash
cd frontend
npm install
npm start
```

Application Angular prévue sur : `http://localhost:4200`

## Endpoints utilisés

- `POST /api/tutor-requests`
- `POST /api/tutor-applications`
- `POST /api/contact-messages`
- `GET /api/health`

## Remarque

Le frontend appelle l'API sur `http://localhost:5167/api`.
Si tu changes le port du backend, modifie le fichier :

`frontend/src/app/core/config/api.config.ts`
