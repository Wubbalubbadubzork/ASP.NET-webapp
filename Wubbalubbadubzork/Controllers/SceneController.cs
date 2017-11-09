using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Wubbalubbadubzork.Models;

namespace Wubbalubbadubzork.Controllers
{
    [Authorize]
    public class SceneController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        LoadedScene LoadScene(int sceneId) {

            Scene scene = db.Scenes.Where<Scene>(u => u.Id == sceneId).FirstOrDefault();
            List<Options> options = db.Options.Where<Options>(u => u.Scene_id == sceneId).ToList<Options>();

            return new LoadedScene(scene.Description, options);
        }
    }
}
