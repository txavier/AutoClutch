using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

#if DEBUG
            ViewBag.Title = "| Staging Version";
#endif

            // Fix bug that causes an error to occur with angular # hash sign.
            if (!Request.Path.EndsWith("/"))
            {
                return RedirectPermanent(Request.Url.ToString() + "/");
            }

            return View();
        }
    }
}
