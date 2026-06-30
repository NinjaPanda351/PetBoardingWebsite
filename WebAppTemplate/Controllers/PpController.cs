using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PawesomePalace.Models;
using System.Web.Mvc;

namespace PawesomePalace.Controllers
{
    [Authorize]
    public class PpController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User.Identity.IsAuthenticated)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager?.FindById(User.Identity.GetUserId());

                if (user != null)
                {
                    ViewBag.HeaderFullName = (user.FirstName + " " + user.LastName).Trim();
                    ViewBag.HeaderInitials = (user.FirstName?.Length > 0 && user.LastName?.Length > 0)
                        ? "" + user.FirstName[0] + user.LastName[0] : "?";
                }
            }
        }
    }
}
