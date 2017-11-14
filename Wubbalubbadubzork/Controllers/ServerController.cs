using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wubbalubbadubzork.Models;

namespace Wubbalubbadubzork.Controllers
{
    public class ServerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult LoadScene(int scene_id)
        {
            Scene scene = db.Scenes.Find(scene_id);
            return Content(scene.Description);
        }
    }
}