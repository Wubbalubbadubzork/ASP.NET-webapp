using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wubbalubbadubzork.Models;

namespace Wubbalubbadubzork
{
    public class HelperMethods
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        public static Game Create(Guid Id)
        {
            Game g = new Game();
            g.Id = Id;
            g.Name = "Game: " + g.Id;

            Server s = new Server();
            s.Id = Guid.NewGuid();
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