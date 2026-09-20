# Mediscreen

Mediscreen est une application permettant de gérer des patients et d'évaluer leur risque de développer un diabète de type 2.

Le projet a été réalisé en ASP.NET Core avec une architecture en microservices.

## Fonctionnalités

L'application permet de :

- afficher, ajouter et modifier des patients ;
- consulter et ajouter des notes médicales ;
- calculer le niveau de risque d'un patient ;
- se connecter avec un compte sécurisé ;
- lancer l'ensemble de l'application avec Docker.

Les niveaux de risque possibles sont :

- None
- Borderline
- In Danger
- Early onset

## Technologies utilisées

- .NET 10
- ASP.NET Core
- Entity Framework Core
- SQL Server
- MongoDB
- ASP.NET Core Identity
- JWT
- Ocelot
- Docker
- ASP.NET Core MVC

## Architecture

Le projet contient plusieurs services :

```text
Frontend
   |
Gateway
   |
   +-- AuthService
   +-- PatientService --> SQL Server
   +-- NoteService --> MongoDB
   +-- RiskService
```

- `AuthService` : connexion et gestion des utilisateurs.
- `PatientService` : gestion des patients.
- `NoteService` : gestion des notes médicales.
- `RiskService` : calcul du risque de diabète.
- `Gateway` : point d'entrée entre le Frontend et les APIs.

## Sécurité

L'application utilise ASP.NET Core Identity et des tokens JWT.

Les services PatientService, NoteService et RiskService sont protégés et nécessitent un token valide.

Les secrets utilisés par Docker sont placés dans un fichier `.env` qui n'est pas envoyé sur GitHub.

## Lancer le projet

Créer un fichier `.env` à la racine :

```env
SQL_SA_PASSWORD=VotreMotDePasse
JWT_KEY=VotreCleJWT
JWT_ISSUER=MediscreenAuthService
JWT_AUDIENCE=MediscreenApplication
JWT_EXPIRES_MINUTES=60
```

Construire et lancer les conteneurs :

```powershell
docker compose build
docker compose up -d
```

Le Frontend est disponible sur :

```text
http://localhost:8082
```

Pour arrêter l'application :

```powershell
docker compose down
```

## Données de test

| Patient | Risque attendu |
|---|---|
| TestNone | None |
| TestBorderline | Borderline |
| TestInDanger | In Danger |
| TestEarlyOnset | Early onset |

## Green Code

Quelques principes ont été appliqués :

- utilisation de Docker avec des images finales limitées au runtime ;
- utilisation de `async/await` ;
- récupération uniquement des données nécessaires ;
- conservation des données avec des volumes Docker.

Des améliorations comme la pagination, le cache ou la réduction des logs pourraient encore être ajoutées.

## Auteur

Projet réalisé dans le cadre de ma formation de développeur d'application.