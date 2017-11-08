using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Wubbalubbadubzork.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Wubbalubbadubzork.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(userId);
                ViewBag.Name = user.Name;
            }
            return View();
        }
    }
}