using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wubbalubbadubzork.Models;

namespace Wubbalubbadubzork.Controllers
{

    [Authorize]
    public class GameController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Game
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Name = user.Name;
            return View();
        }

        //GET: Game/Details
        public ActionResult Details(Guid? id, string name)
        {
            var userId = User.Identity.GetUserId();
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Name = user.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            return View(game);
        } 

        //GET: Game/Queue
        public ActionResult Queue()
        {
            var userId = User.Identity.GetUserId();
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Name = user.Name;

            return View();
        }

        [HttpPost]
        public ActionResult JoinGame(Guid game_id)
        {
            var game = db.Games.Find(game_id);
            if(game != null)
            {
                var users_in_game = db.Users.Where(u => u.Game_Id == game_id).ToList();
                if(users_in_game.Count() < 4)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    user.Game_Id = game_id;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("CreateCustom", new { game } ); //Modify
                }
            }
            return View();
        }

        // Get Game/CreateCustom
        public ActionResult CreateCustom(Game game)
        {
            var userId = User.Identity.GetUserId();
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Name = user.Name;

            return View(game);
        }

        [HttpPost]
        public ActionResult CreateCustom()
        {
            Game g = Create();
           return  RedirectToAction("CreateCustom", new { g });
        }

        [HttpPost]
        public ActionResult StartGame (Game g)
        {
            return RedirectToAction("Details", new { g.Id, g.Name } );
        }

        public Game Create()
        {
            Game g = new Game();
            g.Id = new Guid();
            g.Name = "Game: " + g.Id;

            Server s = new Server();
            s.Id = new Guid();
            s.Name = "Server: " + s.Id;
            s.Scene_id = 1;
            s.Scene = db.Scenes.Where(u => u.Id == 1).FirstOrDefault();

            g.Server_Id = s.Id;
            g.Server = s;

            db.Servers.Add(s);
            db.Games.Add(g);
            db.SaveChanges();

            return g;
        }
    }
}