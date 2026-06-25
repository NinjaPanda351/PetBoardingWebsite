using System;
using System.Web.Mvc;
using PawesomePalace.Models;

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
    }
}
