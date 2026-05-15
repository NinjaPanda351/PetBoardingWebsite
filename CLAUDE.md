# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

**Build:**
```bash
msbuild WebAppTemplate.sln
```
Or use Visual Studio (Build > Build Solution).

**Run:**
- Press F5 in Visual Studio — launches on `https://localhost:44377/` via IIS Express.

**Run Tests:**
```bash
dotnet test WebAppTemplateTests/WebAppTemplateTests.csproj
```
The test project targets .NET 9.0 and uses NUnit 4.3.2. Run individual tests via Visual Studio Test Explorer or `dotnet test --filter "FullyQualifiedName~TestName"`.

**Database Migrations:**
```bash
# From Package Manager Console in Visual Studio
Update-Database
Add-Migration <MigrationName>
```
`AutomaticMigrationsEnabled = false` — all migrations must be added explicitly.

## Tech Stack

- **Backend**: ASP.NET MVC 5.3, .NET Framework 4.8.1
- **ORM**: Entity Framework 6.5.1 (Code-First, SQL Server LocalDB)
- **Auth**: ASP.NET Identity 2.2.4 with OWIN middleware (cookie auth, 2FA support)
- **Frontend**: Bootstrap 5.3.3, jQuery 3.7.1, Razor views

## Architecture

### Project Structure

- `WebAppTemplate/` — main web application
- `WebAppTemplateTests/` — NUnit test project (targets .NET 9.0)

### Controllers & Routing

Routes follow the standard MVC convention (`{controller}/{action}/{id}`). Key controllers:

| Controller | Purpose |
|---|---|
| `HomeController` | Public pages (Index, About, Contact, GitBasics) |
| `AccountController` | Registration, login, password reset, email confirmation |
| `ManageController` | Authenticated user account management, 2FA, phone setup |
| `AdminController` | Admin area — pet and booking management |
| `PetsController` | Pet CRUD |
| `BookingsController` | Booking CRUD |
| `MyAccountController` | Customer-facing account page |
| `ContactUsController` | Contact form |
| `HoursController` | Business hours display |

### Data Layer

- `ApplicationDbContext` (in `Models/IdentityModels.cs`) inherits `IdentityDbContext<ApplicationUser>` — the single EF context.
- `ApplicationUser` extends `IdentityUser` for ASP.NET Identity.
- `ToDoModel.cs` is a placeholder model demonstrating the pattern for new domain models.
- Migrations are in `WebAppTemplate/Migrations/`.
- ViewModels live in `WebAppTemplate/ViewModels/` (recently refactored out of `Models/`).

### Views & Layouts

- `Views/Shared/_Layout.cshtml` — main site layout
- `Views/Shared/_AdminLayout.cshtml` — admin area layout
- `Views/Shared/_LoginPartial.cshtml` — nav bar auth partial

### Authentication

OWIN cookie-based auth is configured in `App_Start/Startup.Auth.cs` and `App_Start/IdentityConfig.cs`. User/role managers are resolved from the OWIN context per-request. Password policy: min 6 chars, requires uppercase, lowercase, digit, and special character. Lockout: 5 failed attempts triggers a 5-minute lockout.

### Bundling

CSS/JS bundles are defined in `App_Start/BundleConfig.cs` and referenced in views via `@Styles.Render` / `@Scripts.Render`.
