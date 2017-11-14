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
            var u = manager.FindById(userId);
            ViewBag.Name = u.Name;

            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.Game_Id != null)
            {
                var game = db.Games.Find(user.Game_Id);
                user.Game_Id = null;
                user.Game = null;
                var nUsers = db.Users.Where(g => g.Game_Id == game.Id).ToList().Count();
                if(nUsers == 0)
                {
                    var server = db.Servers.Find(game.Server_Id);
                    db.Servers.Remove(server);
                    db.Games.Remove(game);
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            return View();
        }

        //GET: Game/Details
        public ActionResult Details(Guid? id)
        {
            var userId = User.Identity.GetUserId();
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Name = user.Name;
            ViewBag.UserId = user.Id;
            ViewBag.GameId = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var u = db.Users.Find(userId);
            
            var game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            if(u.Game_Id == null)
            {
                u.Game_Id = id;
                u.Game = game;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
            }

            var playable_characters = db.Characters.Where(x => x.Playable == true).OrderBy(x => x.Id).ToList();
            var users_in_game = db.Users.Where(x => x.Game_Id == id).ToList();
            int i = 0;
            foreach (var y in users_in_game)
            {
                y.Character_Id = playable_characters[i].Id;
                y.Character = playable_characters[i];
                db.Entry(y).State = EntityState.Modified;
                i++;
            }
            db.SaveChanges();

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

        //GET
        public ActionResult JoinGame()
        {
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
                    user.Game = game;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("Successful");
                }
            }
            return Content("Already 4 players in game!");
        }

        // Get Game/CreateCustom
        public ActionResult CreateCustom(Guid? Id)
        {
            var userId = User.Identity.GetUserId();
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Name = user.Name;

            ApplicationUser u = db.Users.Find(userId);

            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var g = db.Games.Find(Id);

            if (g == null)
            {
                return HttpNotFound();
            }

            u.Game_Id = Id;
            u.Game = g;
            db.Entry(u).State = EntityState.Modified;
            db.SaveChanges();

            return View(g);
        }

        public ActionResult CreateGame()
        {
            Game g = HelperMethods.Create(Guid.NewGuid());

            return RedirectToAction("CreateCustom", new { g.Id });
        }

        //GET: Game/Leaderboards
        public ActionResult Leaderboards()
        {
            var userId = User.Identity.GetUserId();
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Name = user.Name;

            var users = db.Users.OrderBy(u => u.Score).ToList();
            return View(users);
        }

        [HttpPost]
        public ActionResult StartGame (Guid Id)
        {
            var g = db.Games.Find(Id);
            var nUsers = db.Users.Where(u => u.Game_Id == g.Id).ToList().Count();
            if (nUsers == 4)
            {
                return Content("Successful");
            }
            return Content("Cannot start custom game without 4 players.");
        }
    }
}