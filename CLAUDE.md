# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

LocaCraft is a French property management app (real estate assets, leases, tenants, lessors, rent receipts). The UI text and domain vocabulary are in French.

Two sub-projects:
- `LocaCraft-app/` — Angular 18 frontend
- `LocaCraftAPI/` — ASP.NET Core (.NET 10) Web API

## Commands

### Backend (from `LocaCraftAPI/LocaCraftAPI/`)
```bash
dotnet restore
dotnet run          # starts on https://localhost:7195 and http://localhost:5172
                    # Swagger UI served at https://localhost:7195/
```

### Frontend (from `LocaCraft-app/`)
```bash
npm install
npm start           # dev server at http://localhost:4200
npm run build       # production build
ng test             # run Karma/Jasmine unit tests
ng test --include=src/app/some.component.spec.ts  # run a single test file
```

### EF Core migrations (from `LocaCraftAPI/LocaCraftAPI/`)
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```
Migrations run automatically at startup via `db.Database.Migrate()`.

## Architecture

### Backend

**Pattern**: Controller → Repository → `AppDbContext` (EF Core / SQLite `app.db`)

- `Models/` — EF Core entities: `RealEstateAsset`, `Lease`, `Tenant`, `Lessor`, `LeaseDocuments`, `InseeIndexModel`
- `LocaCraftAPI.Data/AppDbContext.cs` — single DbContext with four DbSets
- `Repositories/` — interface + EF Core implementation for each entity (e.g., `IRealEstateAssetRepository` / `RealEstateAssetRepository`)
- `Controllers/` — one controller per entity, routes follow `api/[controller]`, all async
- `Services/InseeService.cs` — fetches French rental index data (IRL, ILC, ICC, ILAT) from the INSEE SDMX API, results cached in `IMemoryCache` for 24 hours

Key relationships: `RealEstateAsset` → `Lease[]` → `Tenant[]` + `LeaseDocuments[]`; each `Lease` also references a `Lessor`.

The API base URL expected by the frontend is configured in `LocaCraft-app/src/environments/environment.ts` (default: `https://localhost:7195/api`).

### Frontend

**Pattern**: Standalone Angular components + injectable services per entity

- `src/app/Services/` — one service per entity (`real-estate.service.ts`, `lease-service.ts`, `tenant.service.ts`, `lessor.service.ts`, `rent-receipt.service.ts`, `insee-index.service.ts`), each wraps `HttpClient` calls to the API
- `src/app/models/` — TypeScript interfaces mirroring backend models
- Each feature area has its own folder (e.g., `real-estate-list/`, `lease-form/`, `tenant-form/`)
- `lateral-drawer/` — reusable slide-in panel that dynamically loads any standalone component via `ViewContainerRef.createComponent()`. It bridges the host's `@Output` events (`formSubmitted`, `formError`, `formCancelled`) by subscribing to the embedded component's matching outputs
- `rent-receipt.service.ts` — client-side PDF generation using `jsPDF` + `jspdf-autotable`; no server involvement

**Routing** (`app.routes.ts`):
| Path | Component |
|------|-----------|
| `` | `RealEstateListComponent` (home) |
| `create` | `RealEstateFormComponent` |
| `details/:id` | `RealEstateDetailsComponent` |
| `lease/create` | `LeaseFormComponent` |
| `insee` | `InseeIndexListComponent` |
| `lessor/create` | `LessorFormComponent` |

`RealEstateDetailsComponent` embeds both `RealEstateFormComponent` and `LeaseListComponent` for in-page editing.

### LateralDrawer pattern

Forms embedded in the drawer must expose `@Output() formSubmitted`, `@Output() formError`, and `@Output() formCancelled` EventEmitters so the drawer can relay events to the host component. The host passes data to the drawer via `componentData` (an object whose keys are assigned directly onto the embedded component instance).

## Linear Workflow

When asked to create a Linear issue, always execute the following three steps automatically without asking for confirmation.

### Step 1 — Create the Linear issue
- Create the issue with the provided title and description
- Retrieve the generated identifier (e.g. `LC-12`)

### Step 2 — Create a GitHub branch
- Base branch: `dev_env`
- Naming convention: `feature/<TitleInUpperCamelCase>`
- Example: `feature/AddTenantForm`
- Keep the branch name concise: truncate the title at 5 words maximum

### Step 2b — Créer un commit vide
- Après la création de la branche, créer un commit vide sur cette branche
- Message du commit : `chore: init <TitleInUpperCamelCase>`
- Commande : `git commit --allow-empty -m "chore: init <TitleInUpperCamelCase>"`

### Step 3 — Create a draft Pull Request
- Title: `Titre de l'issue`
- Target branch: `dev_env`
- Status: **draft**
- PR body template:
  ```
  ## Description
  <!-- Décris les changements apportés -->

  ## Issue Linear
  <lien vers l'issue Linear>

  ## Type de changement
  - [ ] Bug fix
  - [ ] Nouvelle fonctionnalité
  - [ ] Refactoring
  - [ ] Documentation
  ```

### Confirmation summary
Once all three steps are done, print a short summary:
```
✅ Issue créée   : <titre>
✅ Branche       : feature/<slug>
✅ Draft PR      : #<numéro> → dev
```
