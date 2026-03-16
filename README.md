# LocaCraft

LocaCraft is a full-stack property management application. It helps track real estate assets, leases, tenants, and lessors, with a backend API and a single-page frontend.

## Tech Stack
- Frontend: Angular 18 + Bootstrap
- Backend: ASP.NET Core (.NET 10) + EF Core
- Database: PostgreSQL

## Main Features
- Real estate asset catalog
- Lease creation and management
- Tenant and lessor management
- INSEE index lookup (via backend service)
- Rent receipt PDF generation (client-side, via jsPDF)

## Project Structure
- `LocaCraft-app/` Angular frontend
- `LocaCraftAPI/LocaCraftAPI/` ASP.NET Core Web API
- `LocaCraftAPI/LocaCraftAPI.Tests/` xUnit unit tests for the API

## Setup

### Backend (API)
1. From `LocaCraftAPI/LocaCraftAPI`, run:
   - `dotnet restore`
   - `dotnet run`
2. The API will start on:
   - `https://localhost:7195` (HTTPS)
   - `http://localhost:5172` (HTTP)
3. Swagger UI is available at:
   - `https://localhost:7195/`

### Frontend (App)
1. From `LocaCraft-app`, install dependencies:
   - `npm install`
2. Start the dev server:
   - `npm start`
3. The app runs at:
   - `http://localhost:4200/`

### API Base URL
The frontend API URL is configured in:
- `LocaCraft-app/src/app/environments/environment.ts`

Default value:
```
https://localhost:7195/api
```

## Testing

### Run all tests with log output
From `LocaCraftAPI/`:
```bash
bash run-tests.sh
```
Results are printed to the terminal and written to `LocaCraftAPI.Tests/TestResults/test-results.log`.

### Run tests without log
From `LocaCraftAPI/LocaCraftAPI.Tests/`:
```bash
dotnet test
```

## Notes
- The API uses SQLite and auto-runs migrations at startup.
- Ensure the backend is running before using the frontend.
