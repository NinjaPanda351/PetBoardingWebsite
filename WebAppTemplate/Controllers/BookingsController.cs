using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PawesomePalace.Models;
using PawesomePalace.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace PawesomePalace.Controllers
{
    public class BookingsController : Controller
    {

        // GET: /Bookings/Add?petId=...&startTime=2026-06-01&endTime=2026-06-05
        public ActionResult Add(Guid petId, DateTime startTime, DateTime endTime)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            
            var pet = dbContext.PetModels.Find(petId);

            if (pet == null)
            {
                return Content("Error: Pet not found. A booking must be associated with an existing pet.");
            }

            if (startTime >= endTime)
            { 
                return Content("Error: Start date must come before end date."); 
            }


            var booking = new BookingModel();


            booking.PetId = petId;
            booking.BookingStartTime = startTime;
            booking.BookingEndTime = endTime;
            booking.Status = "Pending";
            booking.CreatedAt = DateTime.Now;
            

            dbContext.BookingModels.Add(booking);
            dbContext.SaveChanges();

            return Content($"Booking added with ID: {booking.BookingId}");
        
        }

        // GET: /Bookings/Remove?bookingId=...
        public ActionResult Remove(Guid bookingId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            
            var booking = dbContext.BookingModels.Find(bookingId);
            if (booking == null)
            {
                return Content("Error: Booking not found.");
            }
                    

            dbContext.BookingModels.Remove(booking);
            dbContext.SaveChanges();

            return Content($"Booking '{booking.BookingId}' removed.");
        }

        // GET: /Bookings/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index(string status = "All")
        {
            var db = new ApplicationDbContext(); var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null) return View(new BookingsIndexViewModel { ActiveFilter = status });

            var petIds = db.PetModels
                .Where(p => p.OwnerId == owner.OwnerId)
                .Select(p => p.PetId)
                .ToList();

            var query = db.BookingModels
             .Include(b => b.Pet)
             .Where(b => petIds.Contains(b.PetId));

            if (status != "All")
                query = query.Where(b => b.Status == status);

            var bookings = query
                .OrderBy(b => b.BookingStartTime)
                .ToList()
                .Select(b => new BookingListItemViewModel
                {
                    BookingId = b.BookingId,
                    PetName = b.Pet.Name,
                    PetSpecies = b.Pet.Species,
                    ServiceType = b.ServiceType,
                    BookingReference = b.BookingReference,
                    Price = b.Price,
                    StartDate = b.BookingStartTime,
                    EndDate = b.BookingEndTime,
                    Status = b.Status
                }).ToList();

            return View(new BookingsIndexViewModel
            {
                Bookings = bookings,
                ActiveFilter = status
            });
        }

        // GET: /Bookings/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            var pets = owner != null
                ? db.PetModels.Where(p => p.OwnerId == owner.OwnerId).ToList()
                : new List<PetModel>();

            var model = new CreateBookingViewModel
            {
                PetOptions = pets.Select(p => new SelectListItem
                {
                    Value = p.PetId.ToString(),
                    Text = p.Name + " (" + (p.Species ?? "Pet") + ")"
                }).ToList(),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };

            return View(model);
        }

        // GET: /Bookings/Details/5
        [Authorize]
        [HttpGet]
        public ActionResult Details(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var booking = db.BookingModels.Include(b => b.Pet).FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return HttpNotFound();

            // Security: ensure this booking belongs to the current user
            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || booking.Pet.OwnerId != owner.OwnerId)
                return new HttpUnauthorizedResult();

            var model = new BookingDetailViewModel
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                ServiceType = booking.ServiceType,
                Status = booking.Status,
                PetName = booking.Pet.Name,
                PetSex = booking.Pet.Sex,
                StartDate = booking.BookingStartTime,
                EndDate = booking.BookingEndTime,
                DropOffTime = booking.BookingStartTime.ToString("h:mm tt"),
                PickUpTime = booking.BookingEndTime.ToString("h:mm tt"),
                Price = booking.Price,
                Notes = booking.Notes,
                CancellationReason = booking.CancellationReason,
                CancelledAt = booking.CancelledAt,
                RefundStatus = booking.RefundStatus
            };

            return View(model);
        }

        // GET: /Bookings/CancelRequest/5
        [Authorize]
        [HttpGet]
        public ActionResult CancelRequest(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var booking = db.BookingModels.Include(b => b.Pet).FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || booking.Pet.OwnerId != owner.OwnerId)
                return new HttpUnauthorizedResult();

            if (booking.Status == "Cancelled" || booking.Status == "Completed")
                return RedirectToAction("Details", new { bookingId });

            return View(new CancelBookingViewModel
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference
            });
        }

        // POST: /Bookings/CancelRequest
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelRequest(CancelBookingViewModel model)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var booking = db.BookingModels.Include(b => b.Pet).FirstOrDefault(b => b.BookingId == model.BookingId);
            if (booking == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || booking.Pet.OwnerId != owner.OwnerId)
                return new HttpUnauthorizedResult();

            if (!ModelState.IsValid)
            {
                ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
                ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                    ? "" + user.FirstName[0] + user.LastName[0] : "?";
                model.BookingReference = booking.BookingReference;
                return View(model);
            }

            booking.Status = "Cancelled";
            booking.CancelledAt = DateTime.Now;
            booking.CancellationReason = model.CancellationReason;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Bookings/Edit/5
        [Authorize]
        [HttpGet]
        public ActionResult Edit(Guid bookingId)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var booking = db.BookingModels.Include(b => b.Pet).FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || booking.Pet.OwnerId != owner.OwnerId)
                return new HttpUnauthorizedResult();

            if (booking.Status != "Pending")
                return RedirectToAction("Details", new { bookingId });

            var model = new EditBookingViewModel
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                StartDate = booking.BookingStartTime.Date,
                EndDate = booking.BookingEndTime.Date,
                DropOffTime = booking.BookingStartTime.ToString("HH:mm"),
                PickUpTime = booking.BookingEndTime.ToString("HH:mm"),
                SpecialInstructions = booking.Notes
            };

            return View(model);
        }

        // POST: /Bookings/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBookingViewModel model)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var booking = db.BookingModels.Include(b => b.Pet).FirstOrDefault(b => b.BookingId == model.BookingId);
            if (booking == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || booking.Pet.OwnerId != owner.OwnerId)
                return new HttpUnauthorizedResult();

            if (booking.Status != "Pending")
                return RedirectToAction("Details", new { bookingId = model.BookingId });

            if (!ModelState.IsValid)
            {
                ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
                ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                    ? "" + user.FirstName[0] + user.LastName[0] : "?";
                model.BookingReference = booking.BookingReference;
                return View(model);
            }

            TimeSpan dropOff = TimeSpan.TryParse(model.DropOffTime, out var dto) ? dto : new TimeSpan(9, 0, 0);
            TimeSpan pickUp = TimeSpan.TryParse(model.PickUpTime, out var put) ? put : new TimeSpan(17, 0, 0);

            booking.BookingStartTime = model.StartDate.Date + dropOff;
            booking.BookingEndTime = model.EndDate.Date + pickUp;
            booking.Notes = model.SpecialInstructions;

            db.SaveChanges();

            return RedirectToAction("Details", new { bookingId = model.BookingId });
        }

        // POST: /Bookings/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBookingViewModel model)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            var pets = owner != null
                ? db.PetModels.Where(p => p.OwnerId == owner.OwnerId).ToList()
                : new List<PetModel>();

            model.PetOptions = pets.Select(p => new SelectListItem
            {
                Value = p.PetId.ToString(),
                Text = p.Name + " (" + (p.Species ?? "Pet") + ")"
            }).ToList();

            if (model.StartDate.Year < 2000)
                ModelState.AddModelError("StartDate", "Please enter a valid start date.");
            if (model.EndDate.Year < 2000)
                ModelState.AddModelError("EndDate", "Please enter a valid end date.");
            if (model.StartDate.Year >= 2000 && model.EndDate.Year >= 2000 && model.EndDate.Date <= model.StartDate.Date)
                ModelState.AddModelError("EndDate", "End date must be after start date.");

            if (!ModelState.IsValid)
            {
                ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
                ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                    ? "" + user.FirstName[0] + user.LastName[0] : "?";
                return View(model);
            }

            TimeSpan dropOff = TimeSpan.TryParse(model.DropOffTime, out var dto) ? dto : new TimeSpan(9, 0, 0);
            TimeSpan pickUp = TimeSpan.TryParse(model.PickUpTime, out var put) ? put : new TimeSpan(17, 0, 0);

            decimal nightlyRate;
            switch (model.ServiceType)
            {
                case "Deluxe Suite": nightlyRate = 75m; break;
                case "Daycare": nightlyRate = 35m; break;
                case "Grooming Add-on": nightlyRate = 30m; break;
                default: nightlyRate = 45m; break;
            }
            int nights = Math.Max(1, (model.EndDate.Date - model.StartDate.Date).Days);
            decimal price = nightlyRate * nights;

            string reference = "PP-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();

            var booking = new BookingModel
            {
                PetId = Guid.Parse(model.PetId),
                ServiceType = model.ServiceType,
                BookingStartTime = model.StartDate.Date + dropOff,
                BookingEndTime = model.EndDate.Date + pickUp,
                Notes = model.SpecialInstructions,
                Status = "Pending",
                Price = price,
                BookingReference = reference,
                CreatedAt = DateTime.Now
            };

            db.BookingModels.Add(booking);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
