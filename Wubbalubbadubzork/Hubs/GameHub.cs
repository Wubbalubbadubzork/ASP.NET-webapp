using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Wubbalubbadubzork.Models;

namespace Wubbalubbadubzork.Hubs
{
    public class GameHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public static Dictionary<string, Dictionary<int, Character>> Characters = new Dictionary<string, Dictionary<int, Character>>();

        public static Dictionary<string, Dictionary<Character, string>> User_Characters = new Dictionary<string, Dictionary<Character, string>>();

        public void JoinGame(string gameId, string user_id)
        {
            string connectionId = Context.ConnectionId;
            Groups.Add(connectionId, gameId);

            if (User_Characters.Keys.Contains(gameId) == true)
            {
                for(int i = 3; i <= 6; i++)
                {
                    if (User_Characters[gameId].Keys.Contains(Characters[gameId][i]) == false)
                    {
                        User_Characters[gameId].Add(Characters[gameId][i], connectionId);
                        break;
                    }
                }
            }
            else
            {
                User_Characters.Add(gameId, new Dictionary<Character, string>());
                User_Characters[gameId].Add(Characters[gameId][3], connectionId);
            }
        }

        public void EstablishCharacters(string gameId)
        {
            var All_Characters = db.Characters.OrderBy(u => u.Id).ToList();
            foreach (var j in All_Characters)
            {
                if(Characters.Keys.Contains(gameId) == true)
                {
                    if(Characters[gameId].Keys.Contains(j.Id) == false)
                    {
                        Characters[gameId].Add(j.Id, j);
                    }
                }
                else
                {
                    Characters.Add(gameId, new Dictionary<int, Character>());
                    Characters[gameId].Add(j.Id, j);
                }
            }
        }

        public void CheckCurrentScene(int scene_id, string gameId)
        {
            var enemies_in_scene= db.Characters.Where(u => u.Scene_Id == scene_id).ToList();
            if (enemies_in_scene.Count > 0)
            {
                Clients.Group(gameId).printScene("Narrador: ", "Aún no has acabado con los enemigos.");
                foreach (var j in enemies_in_scene)
                {
                    Clients.Group(gameId).printScene("Narrador: ", "A " + j.Name + " le quedan " + j.Health + "puntos de vida.");
                }
            }
            else
            {
                Clients.Group(gameId).printScene("Narrador: ", "Has acabado con los enemigos. ");
                Clients.Group(gameId).nextScene();
            }
        }

        public void SceneDisplay(string description, string gameId)
        {
            Clients.Group(gameId).printScene("Narrador: ", description);
        }

        public void SendMessage(string username, string message, string gameId)
        {
            Clients.Group(gameId).showMessage(username, message);
        }

        public void PrintDice(int result, string gameId)
        {
            Clients.Group(gameId).rollDice(result);
        }
    }
}