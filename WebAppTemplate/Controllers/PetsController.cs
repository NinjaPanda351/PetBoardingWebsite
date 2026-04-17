using System.Web.Mvc;

namespace WebAppTemplate.Controllers
{
    public class PetsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
