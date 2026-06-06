using System.Web.Mvc;
using WebAppTemplate.Models;
using WebAppTemplate.ViewModels;

namespace WebAppTemplate.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: /ContactUs
        public ActionResult Index()
        {
            var vm = new ContactUsSubmissionVM();
            return View(vm);
        }

        // POST: /ContactUs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactUsSubmissionVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var submission = new ContactUsModel
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                Phone = vm.Phone,
                Subject = vm.Subject,
                Message = vm.Message
            };

            ApplicationDbContext dbContext = new ApplicationDbContext();
            dbContext.ContactUsModels.Add(submission);
            dbContext.SaveChanges();

            return RedirectToAction("ThankYou");
        }

        // GET: /ContactUs/ThankYou
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
