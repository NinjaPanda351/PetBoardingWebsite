using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PawesomePalace.Models;
using PawesomePalace.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PawesomePalace.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {

        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);

            var pets = owner != null ? db.PetModels.Where(p => p.OwnerId == owner.OwnerId).ToList() : new List<PetModel>();

            var viewModel = new PetsIndexViewModel
            {
                Pets = pets
            };

            return View(viewModel);
        }

        // GET: /Pets/Add?petName=Steve&breed=GermanShepherd&age=3&ownerId=...
        public ActionResult Add(string petName, string breed, int age = 0, Guid? ownerId = null)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            var pet = new PetModel();

            pet.OwnerId = ownerId;
            pet.Name = petName;
            pet.Breed = breed;
            pet.Age = age;

            dbContext.PetModels.Add(pet);
            dbContext.SaveChanges();

            return Content($"Pet '{pet.Name}' added with ID: {pet.PetId}");

        }

        // GET: /Pets/Remove?petId=...
        public ActionResult Remove(Guid petId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            var pet = dbContext.PetModels.Find(petId);

            if (pet == null)
            {
                return Content("Error: Pet not found.");
            }

            dbContext.PetModels.Remove(pet);
            dbContext.SaveChanges();

            return Content($"Pet '{pet.Name}' removed.");
        }

        // GET: /Pets/Create
        [HttpGet]
        public ActionResult Create()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());
            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";
            return View();
        }

        // POST: /Pets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePetViewModel model)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);


            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var pet = new PetModel
            {
                Name = model.Name,
                Species = model.Species,
                Breed = model.Breed,
                Sex = model.Sex,
                OwnerId = owner?.OwnerId,
                Age = model.DateOfBirth.HasValue ? (int)((DateTime.Today - model.DateOfBirth.Value).TotalDays / 365) : 0,
                Color = model.Color,
                SecondaryColor = model.SecondaryColor,
                VetName = model.VetName,
                VetPhone = model.VetPhone,
                MedicalNotes = model.MedicalNotes,
                Medication = model.Medication,
                FeedingsPerDay = model.FeedingsPerDay < 1 ? 1 : model.FeedingsPerDay,
                FeedAmount = model.FeedAmount,
                FeedingInstructions = model.FeedingInstructions,
                SpecialInstructions = model.SpecialInstructions
            };

            db.PetModels.Add(pet);
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        ModelState.AddModelError("", ve.ErrorMessage);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /Pets/Edit/PET_GUID
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var pet = db.PetModels.Find(id);
            if (pet == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || pet.OwnerId != owner.OwnerId) return new HttpUnauthorizedResult();

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var model = new EditPetViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                Age = pet.Age,
                Sex = pet.Sex,
                Color = pet.Color,
                SecondaryColor = pet.SecondaryColor,
                VetName = pet.VetName,
                VetPhone = pet.VetPhone,
                MedicalNotes = pet.MedicalNotes,
                Medication = pet.Medication,
                FeedingsPerDay = pet.FeedingsPerDay,
                FeedAmount = pet.FeedAmount,
                FeedingInstructions = pet.FeedingInstructions,
                SpecialInstructions = pet.SpecialInstructions
            };

            return View(model);
        }

        // POST: /Pets/Edit/PET_GUID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPetViewModel model)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var pet = db.PetModels.Find(model.PetId);
            if (pet == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || pet.OwnerId != owner.OwnerId) return new HttpUnauthorizedResult();

            if (!ModelState.IsValid)
            {
                ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
                ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                    ? "" + user.FirstName[0] + user.LastName[0] : "?";
                return View(model);
            }

            pet.Name = model.Name;
            pet.Species = model.Species;
            pet.Breed = model.Breed;
            pet.Age = model.Age ?? pet.Age;
            pet.Sex = model.Sex;
            pet.Color = model.Color;
            pet.SecondaryColor = model.SecondaryColor;
            pet.VetName = model.VetName;
            pet.VetPhone = model.VetPhone;
            pet.MedicalNotes = model.MedicalNotes;
            pet.Medication = model.Medication;
            pet.FeedingsPerDay = model.FeedingsPerDay < 1 ? 1 : model.FeedingsPerDay;
            pet.FeedAmount = model.FeedAmount;
            pet.FeedingInstructions = model.FeedingInstructions;
            pet.SpecialInstructions = model.SpecialInstructions;

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        ModelState.AddModelError("", ve.ErrorMessage);
                ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
                ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                    ? "" + user.FirstName[0] + user.LastName[0] : "?";
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /Pets/Details/PET_GUID
        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var pet = db.PetModels.Find(id);
            if (pet == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || pet.OwnerId != owner.OwnerId) return new HttpUnauthorizedResult();

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var model = new PetDetailsViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                Age = pet.Age,
                Sex = pet.Sex,
                Color = pet.Color,
                SecondaryColor = pet.SecondaryColor,
                VetName = pet.VetName,
                VetPhone = pet.VetPhone,
                MedicalNotes = pet.MedicalNotes,
                Medication = pet.Medication,
                FeedingsPerDay = pet.FeedingsPerDay,
                FeedAmount = pet.FeedAmount,
                FeedingInstructions = pet.FeedingInstructions,
                SpecialInstructions = pet.SpecialInstructions
            };

            return View(model);
        }

        //
        // GET: /Account/DeletePet/PET_GUID
        [HttpGet]
        public ActionResult DeletePet(Guid id)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var pet = db.PetModels.Find(id);
            if (pet == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || pet.OwnerId != owner.OwnerId) return new HttpUnauthorizedResult();

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var model = new PetDetailsViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                Age = pet.Age,
                Sex = pet.Sex
            };

            return View(model);
        }

        // POST: /Pets/DeletePet/PET_GUID
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeletePet")]
        public ActionResult DeletePetConfirmation(Guid id)
        {
            var db = new ApplicationDbContext();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            var pet = db.PetModels.Find(id);
            if (pet == null) return HttpNotFound();

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);
            if (owner == null || pet.OwnerId != owner.OwnerId) return new HttpUnauthorizedResult();

            db.PetModels.Remove(pet);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

