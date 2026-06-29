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

            var owner = db.PetOwnerModels.FirstOrDefault(o => o.Email == user.Email);

            var pets = owner != null ? db.PetModels.Where(p => p.OwnerId == owner.OwnerId).ToList() : new List<PetModel>();

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.HeaderInitials = (user.FirstName != null && user.FirstName.Length > 0 && user.LastName != null && user.LastName.Length > 0)
                ? "" + user.FirstName[0] + user.LastName[0] : "?";

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
            ViewBag.HeaderInitials = (user.FirstName != null && user.FirstName.Length > 0 && user.LastName != null && user.LastName.Length > 0)
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
                MedicalNotes = model.Notes,
                Age = model.DateOfBirth.HasValue ? (int)((DateTime.Today - model.DateOfBirth.Value).TotalDays / 365) : 0
            };

            db.PetModels.Add(pet);
            db.SaveChanges();


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

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName);
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0) ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var model = new EditPetViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                Age = pet.Age,
                Sex = pet.Sex,
                Notes = pet.MedicalNotes
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

            pet.Name = model.Name;
            pet.Species = model.Species;
            pet.Breed = model.Breed;
            pet.Age = model.Age ?? pet.Age;
            pet.Sex = model.Sex;
            pet.MedicalNotes = model.Notes;

            db.SaveChanges();

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

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName);
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0) ? "" + user.FirstName[0] + user.LastName[0] : "?";

            var model = new PetDetailsViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                Age = pet.Age,
                Sex = pet.Sex,
                Notes = pet.MedicalNotes
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

            ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName);
            ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0) ? "" + user.FirstName[0] + user.LastName[0] : "?";

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

