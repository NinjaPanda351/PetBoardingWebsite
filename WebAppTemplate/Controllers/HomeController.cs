using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PawesomePalace.Models;
using PawesomePalace.ViewModels;

namespace PawesomePalace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomeContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HomeContact(HomeContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var nameParts = (model.Name ?? "").Trim().Split(new[] { ' ' }, 2);
            var submission = new ContactUsModel
            {
                FirstName = nameParts[0],
                LastName = nameParts.Length > 1 ? nameParts[1] : "-",
                Email = model.Email,
                Subject = "Website Inquiry",
                Message = model.Message
            };

            var db = new ApplicationDbContext();
            db.ContactUsModels.Add(submission);
            db.SaveChanges();

            return RedirectToAction("ThankYou", "ContactUs");
        }

        public ActionResult About()
        {
            ViewBag.Message = "The application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My contact page.";

            return View();
        }

        public ActionResult GitBasics()
        {
            return Content("Git Basics");
        }

        public ActionResult EndpointA()
        {
            return Content("This is endpoint A");
        }

        public ActionResult Create(String email, String firstName, String lastName)
        {
            // Connect to DB
            ApplicationDbContext dbContext = new ApplicationDbContext();

            // Create Our Object
            ContactUsModel contactUsModel = new ContactUsModel();
            contactUsModel.Email = email;
            contactUsModel.FirstName = firstName;
            contactUsModel.LastName = lastName;
            contactUsModel.IsResolved = false;
            contactUsModel.ResolvedAt = null;

            // Add Object to DbSet
            dbContext.ContactUsModels.Add(contactUsModel);

            // Finalize transaction 
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ;
            }

            return Content("Created");
        }

        public ActionResult Read()
        {
            return Content("Read");
        }

        public ActionResult Update()
        {
            return Content("Updated");
        }

        public ActionResult Delete()
        {
            return Content("Deleted");
        }
    }
}