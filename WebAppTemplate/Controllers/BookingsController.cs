using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PawesomePalace.Models;
using PawesomePalace.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace PawesomePalace.Controllers
{
    public class BookingsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

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
    }
}
