# Homecare

Homecare is an ASP.NET Core MVC application for managing home care appointments. It supports client appointment booking, personnel availability planning, and admin oversight for slots and scheduled visits.

## Features

- Client dashboard with upcoming and past appointments
- Appointment creation, editing, details, and deletion
- Personnel dashboard for managing working days
- Automatic creation of three daily availability slots
- Admin dashboard for monitoring users, slots, and appointments
- SQLite database seeded with demo users, care tasks, slots, and appointments
- Unit tests for key client and personnel workflows

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core with SQLite
- xUnit and Moq for tests
- Bootstrap, Bootstrap Icons, and jQuery
- Serilog file logging

## Requirements

- .NET 8 SDK or newer
- Node.js and npm

The current development machine also has .NET 9 installed, which is used by the test project.

## Getting Started

Restore .NET packages:

```powershell
dotnet restore webprogrammering2025.sln
```

Install frontend packages:

```powershell
cd Homecare
npm install
cd ..
```

Build the application:

```powershell
dotnet build webprogrammering2025.sln
```

Run tests:

```powershell
dotnet test Homecare.Tests\Homecare.Tests.csproj
```

Start the application:

```powershell
dotnet run --project Homecare\Homecare.csproj --launch-profile http
```

Open the app at:

```text
http://localhost:5003
```

## Demo Data

The application seeds demo data in development mode. The SQLite database is stored at:

```text
Homecare/App_Data/homecare.db
```

Example demo users include:

- Admin One
- Nurse A through Nurse E
- Client Ali, Client Eva, Client Leo, Client Mia, and Client Yan

## Project Structure

```text
Homecare/              Main ASP.NET Core MVC application
Homecare.Tests/        xUnit test project
Homecare/App_Data/     SQLite database
Homecare/Controllers/  MVC controllers
Homecare/DAL/          Data access interfaces and repositories
Homecare/Models/       Entity models and DbContext
Homecare/Views/        Razor views
Homecare/wwwroot/      Static assets
```

## Notes

- The app runs in Development mode by default when using the included launch profile.
- Logs are written under `Homecare/Logs`.
- The main app targets .NET 8. The test project currently targets .NET 9.
