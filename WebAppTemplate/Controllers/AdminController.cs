using PawesomePalace.Models;
using PawesomePalace.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PawesomePalace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // ── Dashboard ─────────────────────────────────────────────────────────

        public ActionResult Index()
        {
            var db = new ApplicationDbContext();

            var pendingBookings = db.BookingModels
                .Include(b => b.Pet)
                .Include(b => b.Pet.Owner)
                .Where(b => b.Status == "Pending")
                .OrderBy(b => b.BookingStartTime)
                .ToList()
                .Select(b => new AdminBookingRowViewModel
                {
                    BookingId = b.BookingId,
                    OwnerName = b.Pet.Owner != null
                        ? (b.Pet.Owner.FirstName + " " + b.Pet.Owner.LastName).Trim()
                        : "Unknown",
                    PetName = b.Pet.Name,
                    PetSpecies = b.Pet.Species,
                    PetBreed = b.Pet.Breed,
                    StartDate = b.BookingStartTime,
                    EndDate = b.BookingEndTime
                }).ToList();

            var cancellationRequests = db.BookingModels
                .Include(b => b.Pet)
                .Include(b => b.Pet.Owner)
                .Where(b => b.Status == "Cancelled" && b.CancellationReason != null && b.RefundStatus == null)
                .OrderByDescending(b => b.CancelledAt)
                .ToList()
                .Select(b => new AdminCancellationRowViewModel
                {
                    BookingId = b.BookingId,
                    OwnerName = b.Pet.Owner != null
                        ? (b.Pet.Owner.FirstName + " " + b.Pet.Owner.LastName).Trim()
                        : "Unknown",
                    Price = b.Price,
                    CancellationReason = b.CancellationReason
                }).ToList();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            var todayCheckIns = db.BookingModels
                .Count(b => b.Status == "Confirmed" && b.BookingStartTime >= today && b.BookingStartTime < tomorrow);
            var todayCheckOuts = db.BookingModels
                .Count(b => b.Status == "CheckedIn" && b.BookingEndTime >= today && b.BookingEndTime < tomorrow);
            var monthRevenue = db.BookingModels
                .Where(b => b.Status == "Completed" && b.BookingEndTime >= monthStart && b.BookingEndTime < nextMonthStart)
                .Select(b => (decimal?)b.Price)
                .Sum() ?? 0m;

            var checkInsDue = db.BookingModels
                .Include(b => b.Pet)
                .Include(b => b.Pet.Owner)
                .Where(b => b.Status == "Confirmed" && b.BookingStartTime >= today && b.BookingStartTime < tomorrow)
                .OrderBy(b => b.BookingStartTime)
                .ToList()
                .Select(b => new AdminScheduleItemViewModel
                {
                    BookingId = b.BookingId,
                    BookingReference = b.BookingReference,
                    PetName = b.Pet.Name,
                    PetSpecies = b.Pet.Species,
                    OwnerName = b.Pet.Owner != null ? (b.Pet.Owner.FirstName + " " + b.Pet.Owner.LastName).Trim() : "Unknown",
                    OwnerPhone = b.Pet.Owner?.Phone,
                    ServiceType = b.ServiceType,
                    StartDate = b.BookingStartTime,
                    EndDate = b.BookingEndTime,
                    DropOffTime = b.BookingStartTime.ToString("h:mm tt"),
                    PickUpTime = b.BookingEndTime.ToString("h:mm tt"),
                    Status = b.Status,
                    MedicalAlert = !string.IsNullOrWhiteSpace(b.Pet.MedicalNotes) || !string.IsNullOrWhiteSpace(b.Pet.Medication)
                }).ToList();

            var model = new AdminDashboardViewModel
            {
                PendingCount = pendingBookings.Count,
                ConfirmedCount = db.BookingModels.Count(b => b.Status == "Confirmed"),
                CancellationRequestCount = cancellationRequests.Count,
                PendingBookings = pendingBookings,
                CancellationRequests = cancellationRequests,
                TodayCheckIns = todayCheckIns,
                TodayCheckOuts = todayCheckOuts,
                MonthRevenue = monthRevenue,
                CheckInsDue = checkInsDue
            };

            return View(model);
        }

        // ── Review ────────────────────────────────────────────────────────────

        public ActionResult Review(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var booking = db.BookingModels
                .Include(b => b.Pet)
                .Include(b => b.Pet.Owner)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null) return HttpNotFound();

            string petDetails = booking.Pet.Name
                + (!string.IsNullOrWhiteSpace(booking.Pet.Species) ? " · " + booking.Pet.Species : "")
                + (!string.IsNullOrWhiteSpace(booking.Pet.Breed)   ? " · " + booking.Pet.Breed   : "");

            var model = new AdminReviewViewModel
            {
                BookingId           = booking.BookingId,
                BookingReference    = booking.BookingReference,
                OwnerName           = booking.Pet.Owner != null
                                        ? (booking.Pet.Owner.FirstName + " " + booking.Pet.Owner.LastName).Trim()
                                        : "Unknown",
                ServiceType         = booking.ServiceType,
                PetDetails          = petDetails,
                StartDate           = booking.BookingStartTime,
                EndDate             = booking.BookingEndTime,
                SpecialInstructions = booking.Notes,
                AdminNotes          = booking.AdminNotes,
                VetName             = booking.Pet.VetName,
                VetPhone            = booking.Pet.VetPhone,
                MedicalNotes        = booking.Pet.MedicalNotes,
                Medication          = booking.Pet.Medication,
                FeedingsPerDay      = booking.Pet.FeedingsPerDay,
                FeedAmount          = booking.Pet.FeedAmount,
                FeedingInstructions = booking.Pet.FeedingInstructions
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(AdminReviewViewModel model, string reviewAction)
        {
            var db = new ApplicationDbContext();
            var booking = db.BookingModels.Find(model.BookingId);
            if (booking == null) return HttpNotFound();

            booking.AdminNotes = model.AdminNotes;

            switch (reviewAction)
            {
                case "Approve":
                    booking.Status = "Confirmed";
                    break;
                case "Decline":
                    booking.Status = "Cancelled";
                    booking.CancelledAt = DateTime.Now;
                    booking.CancellationReason = "Declined by admin.";
                    break;
                case "Flag":
                    booking.Status = "Flagged";
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ── Review Cancellation ───────────────────────────────────────────────

        public ActionResult ReviewCancellation(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var booking = db.BookingModels
                .Include(b => b.Pet)
                .Include(b => b.Pet.Owner)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null) return HttpNotFound();

            string petDetails = booking.Pet.Name
                + (!string.IsNullOrWhiteSpace(booking.Pet.Species) ? " · " + booking.Pet.Species : "")
                + (!string.IsNullOrWhiteSpace(booking.Pet.Breed)   ? " · " + booking.Pet.Breed   : "");

            var model = new AdminCancellationReviewViewModel
            {
                BookingId          = booking.BookingId,
                BookingReference   = booking.BookingReference,
                OwnerName          = booking.Pet.Owner != null
                                       ? (booking.Pet.Owner.FirstName + " " + booking.Pet.Owner.LastName).Trim()
                                       : "Unknown",
                PetDetails         = petDetails,
                StartDate          = booking.BookingStartTime,
                EndDate            = booking.BookingEndTime,
                CancellationReason = booking.CancellationReason,
                Price              = booking.Price,
                AdminNotes         = booking.AdminNotes
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewCancellation(AdminCancellationReviewViewModel model, string reviewAction)
        {
            var db = new ApplicationDbContext();
            var booking = db.BookingModels.Find(model.BookingId);
            if (booking == null) return HttpNotFound();

            booking.AdminNotes = model.AdminNotes;
            booking.RefundStatus = reviewAction == "ApproveRefund" ? "Approved" : "Denied";

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ── Users ─────────────────────────────────────────────────────────────

        public ActionResult Users(string search = null)
        {
            var db = new ApplicationDbContext();

            var owners = db.PetOwnerModels.Include(o => o.Pets).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                owners = owners.Where(o =>
                    (o.FirstName + " " + o.LastName).ToLower().Contains(s) ||
                    o.Email.ToLower().Contains(s)).ToList();
            }

            var rows = owners.Select(o =>
            {
                var appUser = db.Users.FirstOrDefault(u => u.Email == o.Email);
                bool suspended = appUser != null
                    && appUser.LockoutEnabled
                    && appUser.LockoutEndDateUtc.HasValue
                    && appUser.LockoutEndDateUtc.Value > DateTimeOffset.UtcNow;

                string initials = "";
                if (!string.IsNullOrEmpty(o.FirstName)) initials += o.FirstName[0];
                if (!string.IsNullOrEmpty(o.LastName))  initials += o.LastName[0];

                return new AdminUserRowViewModel
                {
                    OwnerId     = o.OwnerId,
                    Initials    = initials.ToUpper(),
                    FullName    = (o.FirstName + " " + o.LastName).Trim(),
                    Email       = o.Email,
                    PetCount    = o.Pets?.Count ?? 0,
                    IsSuspended = suspended
                };
            }).OrderBy(r => r.FullName).ToList();

            return View(new AdminUsersViewModel { Users = rows, Search = search });
        }

        public ActionResult ViewUser(Guid ownerId)
        {
            var db = new ApplicationDbContext();

            var owner = db.PetOwnerModels
                .Include(o => o.Pets)
                .Include(o => o.EmergencyContacts)
                .FirstOrDefault(o => o.OwnerId == ownerId);
            if (owner == null) return HttpNotFound();

            var appUser = db.Users.FirstOrDefault(u => u.Email == owner.Email);
            bool suspended = appUser != null
                && appUser.LockoutEnabled
                && appUser.LockoutEndDateUtc.HasValue
                && appUser.LockoutEndDateUtc.Value > DateTimeOffset.UtcNow;

            string initials = "";
            if (!string.IsNullOrEmpty(owner.FirstName)) initials += owner.FirstName[0];
            if (!string.IsNullOrEmpty(owner.LastName))  initials += owner.LastName[0];

            var emergencyContacts = owner.EmergencyContacts?.Select(ec => new EmergencyContactViewModel
            {
                FirstName    = ec.FirstName,
                LastName     = ec.LastName,
                Phone        = ec.Phone,
                Relationship = ec.Relationship
            }).ToList() ?? new List<EmergencyContactViewModel>();

            var model = new AdminViewUserViewModel
            {
                OwnerId           = owner.OwnerId,
                Initials          = initials.ToUpper(),
                FullName          = (owner.FirstName + " " + owner.LastName).Trim(),
                Email             = owner.Email,
                Phone             = owner.Phone,
                MemberSince       = owner.CreatedAt,
                PetCount          = owner.Pets?.Count ?? 0,
                IsSuspended       = suspended,
                Address           = owner.Address,
                City              = owner.City,
                State             = owner.State,
                ZipCode           = owner.ZipCode,
                EmergencyContacts = emergencyContacts
            };

            return View(model);
        }

        // ── Schedule ──────────────────────────────────────────────────────────

        public ActionResult Schedule(string filter = "Today")
        {
            var db = new ApplicationDbContext();
            var today = DateTime.Today;

            IQueryable<BookingModel> query = db.BookingModels
                .Include(b => b.Pet)
                .Include(b => b.Pet.Owner)
                .Where(b => b.Status == "Confirmed" || b.Status == "CheckedIn");

            switch (filter)
            {
                case "ThisWeek":
                    var weekEnd = today.AddDays(7);
                    query = query.Where(b => b.BookingStartTime < weekEnd && b.BookingEndTime >= today);
                    break;
                case "NextWeek":
                    var nextWeekStart = today.AddDays(7);
                    var nextWeekEnd   = today.AddDays(14);
                    query = query.Where(b => b.BookingStartTime < nextWeekEnd && b.BookingEndTime >= nextWeekStart);
                    break;
                case "All":
                    break;
                default: // "Today"
                    filter = "Today";
                    var tomorrow = today.AddDays(1);
                    query = query.Where(b => b.BookingStartTime < tomorrow && b.BookingEndTime >= today);
                    break;
            }

            var items = query.OrderBy(b => b.BookingStartTime).ToList().Select(b => new AdminScheduleItemViewModel
            {
                BookingId       = b.BookingId,
                BookingReference = b.BookingReference,
                PetName         = b.Pet.Name,
                PetSpecies      = b.Pet.Species,
                OwnerName       = b.Pet.Owner != null ? (b.Pet.Owner.FirstName + " " + b.Pet.Owner.LastName).Trim() : "Unknown",
                OwnerPhone      = b.Pet.Owner?.Phone,
                ServiceType     = b.ServiceType,
                StartDate       = b.BookingStartTime,
                EndDate         = b.BookingEndTime,
                DropOffTime     = b.BookingStartTime.ToString("h:mm tt"),
                PickUpTime      = b.BookingEndTime.ToString("h:mm tt"),
                Status          = b.Status,
                MedicalAlert    = !string.IsNullOrWhiteSpace(b.Pet.MedicalNotes) || !string.IsNullOrWhiteSpace(b.Pet.Medication)
            }).ToList();

            return View(new AdminScheduleViewModel { Items = items, DateFilter = filter });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var booking = db.BookingModels.Find(bookingId);
            if (booking == null) return HttpNotFound();
            booking.Status = "CheckedIn";
            db.SaveChanges();
            return RedirectToAction("Schedule");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var booking = db.BookingModels.Find(bookingId);
            if (booking == null) return HttpNotFound();
            booking.Status = "Completed";
            db.SaveChanges();
            return RedirectToAction("Schedule");
        }

        // ── Bookings Management ───────────────────────────────────────────────

        public ActionResult Bookings(string status = "All", string search = null)
        {
            var db = new ApplicationDbContext();

            var bookings = db.BookingModels
                .Include(b => b.Pet)
                .Include(b => b.Pet.Owner)
                .AsQueryable();

            if (status != "All")
                bookings = bookings.Where(b => b.Status == status);

            var list = bookings.OrderByDescending(b => b.BookingStartTime).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                list = list.Where(b =>
                    b.BookingReference.ToLower().Contains(s) ||
                    b.Pet.Name.ToLower().Contains(s) ||
                    (b.Pet.Owner != null && (b.Pet.Owner.FirstName + " " + b.Pet.Owner.LastName).ToLower().Contains(s))
                ).ToList();
            }

            var items = list.Select(b => new AdminBookingListItemViewModel
            {
                BookingId       = b.BookingId,
                BookingReference = b.BookingReference,
                OwnerName       = b.Pet.Owner != null ? (b.Pet.Owner.FirstName + " " + b.Pet.Owner.LastName).Trim() : "Unknown",
                PetName         = b.Pet.Name,
                ServiceType     = b.ServiceType,
                StartDate       = b.BookingStartTime,
                EndDate         = b.BookingEndTime,
                Status          = b.Status,
                Price           = b.Price
            }).ToList();

            return View(new AdminBookingsListViewModel { Items = items, StatusFilter = status, Search = search });
        }

        public ActionResult BookingDetail(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var b = db.BookingModels
                .Include(x => x.Pet)
                .Include(x => x.Pet.Owner)
                .FirstOrDefault(x => x.BookingId == bookingId);

            if (b == null) return HttpNotFound();

            var model = new AdminBookingDetailViewModel
            {
                BookingId        = b.BookingId,
                BookingReference = b.BookingReference,
                Status           = b.Status,
                ServiceType      = b.ServiceType,
                StartDate        = b.BookingStartTime,
                EndDate          = b.BookingEndTime,
                DropOffTime      = b.BookingStartTime.ToString("h:mm tt"),
                PickUpTime       = b.BookingEndTime.ToString("h:mm tt"),
                Price            = b.Price,
                Notes            = b.Notes,
                AdminNotes       = b.AdminNotes,
                CancellationReason = b.CancellationReason,
                CancelledAt      = b.CancelledAt,
                RefundStatus     = b.RefundStatus,
                OwnerName        = b.Pet.Owner != null ? (b.Pet.Owner.FirstName + " " + b.Pet.Owner.LastName).Trim() : "Unknown",
                OwnerPhone       = b.Pet.Owner?.Phone,
                PetId            = b.Pet.PetId,
                PetName          = b.Pet.Name,
                PetSpecies       = b.Pet.Species,
                PetBreed         = b.Pet.Breed,
                PetSex           = b.Pet.Sex,
                VetName          = b.Pet.VetName,
                VetPhone         = b.Pet.VetPhone,
                MedicalNotes     = b.Pet.MedicalNotes,
                Medication       = b.Pet.Medication,
                FeedingsPerDay   = b.Pet.FeedingsPerDay,
                FeedAmount       = b.Pet.FeedAmount,
                FeedingInstructions = b.Pet.FeedingInstructions,
                SpecialInstructions = b.Pet.SpecialInstructions
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBookingStatus(Guid bookingId, string newStatus)
        {
            var db = new ApplicationDbContext();
            var booking = db.BookingModels.Find(bookingId);
            if (booking == null) return HttpNotFound();
            booking.Status = newStatus;
            db.SaveChanges();
            return RedirectToAction("BookingDetail", new { bookingId });
        }

        // ── Pets Management ───────────────────────────────────────────────────

        public ActionResult Pets(string search = null)
        {
            var db = new ApplicationDbContext();

            var pets = db.PetModels
                .Include(p => p.Owner)
                .Include(p => p.Bookings)
                .ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                pets = pets.Where(p =>
                    p.Name.ToLower().Contains(s) ||
                    (!string.IsNullOrEmpty(p.Species) && p.Species.ToLower().Contains(s)) ||
                    (p.Owner != null && (p.Owner.FirstName + " " + p.Owner.LastName).ToLower().Contains(s))
                ).ToList();
            }

            var rows = pets.Select(p => new AdminPetRowViewModel
            {
                PetId          = p.PetId,
                Name           = p.Name,
                Species        = p.Species,
                Breed          = p.Breed,
                OwnerName      = p.Owner != null ? (p.Owner.FirstName + " " + p.Owner.LastName).Trim() : "Unknown",
                HasMedicalNotes = !string.IsNullOrWhiteSpace(p.MedicalNotes) || !string.IsNullOrWhiteSpace(p.Medication),
                BookingCount   = p.Bookings?.Count ?? 0
            }).OrderBy(p => p.Name).ToList();

            return View(new AdminPetsListViewModel { Pets = rows, Search = search });
        }

        public ActionResult PetDetail(Guid petId)
        {
            var db = new ApplicationDbContext();
            var pet = db.PetModels
                .Include(p => p.Owner)
                .Include(p => p.Owner.EmergencyContacts)
                .FirstOrDefault(p => p.PetId == petId);

            if (pet == null) return HttpNotFound();

            var emergencyContacts = pet.Owner?.EmergencyContacts?.Select(ec => new EmergencyContactViewModel
            {
                FirstName    = ec.FirstName,
                LastName     = ec.LastName,
                Phone        = ec.Phone,
                Relationship = ec.Relationship
            }).ToList() ?? new List<EmergencyContactViewModel>();

            var model = new AdminPetDetailViewModel
            {
                PetId               = pet.PetId,
                Name                = pet.Name,
                Species             = pet.Species,
                Breed               = pet.Breed,
                Sex                 = pet.Sex,
                DateOfBirth         = pet.DateOfBirth,
                Color               = pet.Color,
                SecondaryColor      = pet.SecondaryColor,
                VetName             = pet.VetName,
                VetPhone            = pet.VetPhone,
                MedicalNotes        = pet.MedicalNotes,
                Medication          = pet.Medication,
                FeedingsPerDay      = pet.FeedingsPerDay,
                FeedAmount          = pet.FeedAmount,
                FeedingInstructions = pet.FeedingInstructions,
                SpecialInstructions = pet.SpecialInstructions,
                OwnerName           = pet.Owner != null ? (pet.Owner.FirstName + " " + pet.Owner.LastName).Trim() : "Unknown",
                OwnerPhone          = pet.Owner?.Phone,
                OwnerId             = pet.Owner?.OwnerId ?? Guid.Empty,
                EmergencyContacts   = emergencyContacts
            };

            return View(model);
        }

        // ── Services ──────────────────────────────────────────────────────────

        public ActionResult Services()
        {
            var db = new ApplicationDbContext();
            var services = db.ServiceModels.OrderBy(s => s.Name).ToList()
                .Select(s => new ServiceViewModel
                {
                    ServiceId    = s.ServiceId,
                    Name         = s.Name,
                    Description  = s.Description,
                    PricePerNight = s.PricePerNight,
                    IsActive     = s.IsActive
                }).ToList();
            return View(services);
        }

        [HttpGet]
        public ActionResult CreateService()
        {
            return View(new ServiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateService(ServiceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var db = new ApplicationDbContext();
            db.ServiceModels.Add(new ServiceModel
            {
                Name         = model.Name,
                Description  = model.Description,
                PricePerNight = model.PricePerNight,
                IsActive     = model.IsActive
            });
            db.SaveChanges();
            return RedirectToAction("Services");
        }

        [HttpGet]
        public ActionResult EditService(Guid serviceId)
        {
            var db = new ApplicationDbContext();
            var svc = db.ServiceModels.Find(serviceId);
            if (svc == null) return HttpNotFound();

            return View(new ServiceViewModel
            {
                ServiceId    = svc.ServiceId,
                Name         = svc.Name,
                Description  = svc.Description,
                PricePerNight = svc.PricePerNight,
                IsActive     = svc.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditService(ServiceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var db = new ApplicationDbContext();
            var svc = db.ServiceModels.Find(model.ServiceId);
            if (svc == null) return HttpNotFound();

            svc.Name         = model.Name;
            svc.Description  = model.Description;
            svc.PricePerNight = model.PricePerNight;
            svc.IsActive     = model.IsActive;
            db.SaveChanges();
            return RedirectToAction("Services");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleService(Guid serviceId)
        {
            var db = new ApplicationDbContext();
            var svc = db.ServiceModels.Find(serviceId);
            if (svc == null) return HttpNotFound();
            svc.IsActive = !svc.IsActive;
            db.SaveChanges();
            return RedirectToAction("Services");
        }

        // ── Contacts ──────────────────────────────────────────────────────────

        public ActionResult Contacts(bool resolved = false)
        {
            var db = new ApplicationDbContext();
            var rows = db.ContactUsModels
                .Where(c => c.IsResolved == resolved)
                .OrderByDescending(c => c.SubmittedAt)
                .ToList()
                .Select(c => new AdminContactRowViewModel
                {
                    SubmissionId = c.SubmissionId,
                    FullName     = (c.FirstName + " " + c.LastName).Trim(),
                    Email        = c.Email,
                    Subject      = c.Subject,
                    SubmittedAt  = c.SubmittedAt,
                    IsResolved   = c.IsResolved
                }).ToList();

            ViewBag.ShowResolved = resolved;
            return View(rows);
        }

        public ActionResult ContactDetail(Guid submissionId)
        {
            var db = new ApplicationDbContext();
            var c = db.ContactUsModels.Find(submissionId);
            if (c == null) return HttpNotFound();

            return View(new AdminContactDetailViewModel
            {
                SubmissionId = c.SubmissionId,
                FirstName    = c.FirstName,
                LastName     = c.LastName,
                Email        = c.Email,
                Phone        = c.Phone,
                Subject      = c.Subject,
                Message      = c.Message,
                SubmittedAt  = c.SubmittedAt,
                IsResolved   = c.IsResolved,
                ResolvedAt   = c.ResolvedAt
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResolveContact(Guid submissionId)
        {
            var db = new ApplicationDbContext();
            var c = db.ContactUsModels.Find(submissionId);
            if (c == null) return HttpNotFound();
            c.IsResolved = true;
            c.ResolvedAt = DateTime.UtcNow;
            db.SaveChanges();
            return RedirectToAction("Contacts");
        }
    }
}
