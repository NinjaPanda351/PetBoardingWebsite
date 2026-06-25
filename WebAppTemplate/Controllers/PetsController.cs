using System;
using System.Web.Mvc;
using PawesomePalace.Models;

namespace PawesomePalace.Controllers
{
    public class PetsController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
    }
}
