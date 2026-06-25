using System.Web.Mvc;

namespace PawesomePalace.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
