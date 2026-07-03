using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PawesomePalace.Models;
using PawesomePalace.ViewModels;

namespace PawesomePalace.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);

            int petCount = 0;
            int activeBookingCount = 0;

            if (owner != null)
            {
                petCount = db.PetModels.Count(p => p.OwnerId == owner.OwnerId);
                activeBookingCount = db.BookingModels
                    .Count(b => b.Pet.OwnerId == owner.OwnerId
                        && b.Status != "Cancelled"
                        && b.BookingEndTime >= DateTime.Now);
            }

            var viewModel = new MyAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                PetCount = petCount,
                ActiveBookingCount = activeBookingCount
            };

            return View(viewModel);
        }
    }
}
