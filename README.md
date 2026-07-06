# Pawesome Palace Pet Boarding

A full-stack pet boarding management web application built with ASP.NET MVC 5, featuring a customer-facing portal and a staff admin dashboard.

---

## Screenshots

### Landing Page
<!-- Screenshot: home page hero section -->
![Landing Page](screenshots/landing-page.png)

### About Page
<!-- Screenshot: about page -->
![About Page](screenshots/about.png)

### Business Hours
<!-- Screenshot: hours page -->
![Business Hours](screenshots/hours.png)

### Contact Us
<!-- Screenshot: contact form page -->
![Contact Us](screenshots/contact-us.png)

---

## Customer Portal

### My Account Dashboard
<!-- Screenshot: customer dashboard showing pet count and active bookings -->
![My Account](screenshots/my-account.png)

### My Pets
<!-- Screenshot: pet list page -->
![My Pets](screenshots/pets-index.png)

### Add a Pet
<!-- Screenshot: pet creation form with medical and feeding fields -->
![Add Pet](screenshots/pets-create.png)

### Pet Details
<!-- Screenshot: full pet profile view -->
![Pet Details](screenshots/pets-details.png)

### Edit Pet
<!-- Screenshot: pet edit form -->
![Edit Pet](screenshots/pets-edit.png)

### My Bookings
<!-- Screenshot: booking list with status filter tabs (All / Pending / Confirmed / Completed / Cancelled) -->
![My Bookings](screenshots/bookings-index.png)

### Create a Booking
<!-- Screenshot: booking form with service selection and date picker -->
![Create Booking](screenshots/bookings-create.png)

### Booking Details
<!-- Screenshot: booking detail view with drop-off/pick-up times and status -->
![Booking Details](screenshots/bookings-details.png)

### Cancel a Booking
<!-- Screenshot: cancellation request form with reason field -->
![Cancel Booking](screenshots/bookings-cancel.png)

---

## Authentication & Account Management

### Register
<!-- Screenshot: registration form -->
![Register](screenshots/register.png)

### Login
<!-- Screenshot: login form -->
![Login](screenshots/login.png)

### Forgot Password
<!-- Screenshot: password recovery form -->
![Forgot Password](screenshots/forgot-password.png)

### Manage Profile
<!-- Screenshot: profile page with personal info and password change -->
![Manage Profile](screenshots/manage-profile.png)

---

## Admin Dashboard

### Dashboard Overview
<!-- Screenshot: admin index with KPIs — pending bookings, today's check-ins/outs, monthly revenue -->
![Admin Dashboard](screenshots/admin-dashboard.png)

### Schedule
<!-- Screenshot: staff schedule view with Today / This Week / Next Week filter -->
![Schedule](screenshots/admin-schedule.png)

### Review Bookings
<!-- Screenshot: booking approval workflow with Approve / Decline / Flag actions -->
![Review Bookings](screenshots/admin-review.png)

### Review Cancellations
<!-- Screenshot: cancellation refund decision interface -->
![Review Cancellations](screenshots/admin-review-cancellation.png)

### Booking Management
<!-- Screenshot: admin bookings list with search and status filters -->
![Admin Bookings](screenshots/admin-bookings.png)

### Booking Detail (Admin)
<!-- Screenshot: full booking record with admin notes and status controls -->
![Admin Booking Detail](screenshots/admin-booking-detail.png)

### Pet Inventory
<!-- Screenshot: pets list with medical alert flags -->
![Admin Pets](screenshots/admin-pets.png)

### Pet Detail (Admin)
<!-- Screenshot: pet profile with owner info and emergency contacts -->
![Admin Pet Detail](screenshots/admin-pet-detail.png)

### User Management
<!-- Screenshot: users list with search and suspension tracking -->
![User Management](screenshots/admin-users.png)

### User Detail (Admin)
<!-- Screenshot: user profile with emergency contact details -->
![User Detail](screenshots/admin-user-detail.png)

### Services Management
<!-- Screenshot: services list with active/inactive toggle and pricing -->
![Services](screenshots/admin-services.png)

### Contact Submissions
<!-- Screenshot: contact form submissions filtered by resolved/unresolved -->
![Contacts](screenshots/admin-contacts.png)

### Contact Detail (Admin)
<!-- Screenshot: individual contact submission with resolution tracking -->
![Contact Detail](screenshots/admin-contact-detail.png)

---

## Features

**Customer Portal**
- Register, log in, and manage your profile
- Add and manage multiple pets with detailed medical and feeding information
- Create, view, edit, and cancel bookings
- Select from available services with per-night pricing
- View drop-off/pick-up times and real-time booking status

**Admin Dashboard**
- KPI overview: pending bookings, today's check-ins/outs, monthly revenue
- Approve, decline, or flag booking requests
- Manage cancellation requests and refund decisions
- Staff schedule view filtered by day or week
- Full pet inventory with medical alert flags
- User management with profile and emergency contact details
- Service CRUD with active/inactive toggling
- Contact form submission tracking and resolution

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET MVC 5.3 / .NET Framework 4.8.1 |
| ORM | Entity Framework 6.5.1 (Code-First) |
| Database | SQL Server LocalDB |
| Auth | ASP.NET Identity 2.2.4 + OWIN (cookie auth, 2FA support) |
| Frontend | Bootstrap 5.3.3, jQuery 3.7.1, Razor Views |
| Testing | NUnit 4.3.2 (.NET 9.0 test project) |

---

## Getting Started

**Prerequisites:** Visual Studio 2022, SQL Server LocalDB

```bash
# Restore packages and build
msbuild WebAppTemplate.sln

# Apply database migrations (run in Visual Studio Package Manager Console)
Update-Database
```

Run with **F5** in Visual Studio — launches at `https://localhost:44377/` via IIS Express.

**Run tests:**
```bash
dotnet test WebAppTemplateTests/WebAppTemplateTests.csproj
```

---

## Author

**Joey Fausto** — [GitHub](https://github.com/NinjaPanda351/PetBoardingWebsite)
