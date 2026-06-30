using PawesomePalace.Models;
using PawesomePalace.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PawesomePalace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
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

            var model = new AdminDashboardViewModel
            {
                PendingCount = pendingBookings.Count,
                ConfirmedCount = db.BookingModels.Count(b => b.Status == "Confirmed"),
                CancellationRequestCount = cancellationRequests.Count,
                PendingBookings = pendingBookings,
                CancellationRequests = cancellationRequests
            };

            return View(model);
        }

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
                AdminNotes          = booking.AdminNotes
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

            var owner = db.PetOwnerModels.Include(o => o.Pets).FirstOrDefault(o => o.OwnerId == ownerId);
            if (owner == null) return HttpNotFound();

            var appUser = db.Users.FirstOrDefault(u => u.Email == owner.Email);
            bool suspended = appUser != null
                && appUser.LockoutEnabled
                && appUser.LockoutEndDateUtc.HasValue
                && appUser.LockoutEndDateUtc.Value > DateTimeOffset.UtcNow;

            string initials = "";
            if (!string.IsNullOrEmpty(owner.FirstName)) initials += owner.FirstName[0];
            if (!string.IsNullOrEmpty(owner.LastName))  initials += owner.LastName[0];

            var model = new AdminViewUserViewModel
            {
                OwnerId     = owner.OwnerId,
                Initials    = initials.ToUpper(),
                FullName    = (owner.FirstName + " " + owner.LastName).Trim(),
                Email       = owner.Email,
                Phone       = owner.Phone,
                MemberSince = owner.CreatedAt,
                PetCount    = owner.Pets?.Count ?? 0,
                IsSuspended = suspended
            };

            return View(model);
        }

        public ActionResult Pets()
        {
            return View();
        }

        public ActionResult Bookings()
        {
            return View();
        }
    }
}
