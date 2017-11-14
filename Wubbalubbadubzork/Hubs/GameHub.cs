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

        public void WhoseTurn(string gameId)
        {
            var keys = Characters[gameId].Keys;
            foreach(var i in keys)
            {
                if(Characters[gameId][i].Is_Turn == true)
                {
                    Clients.Group(gameId).isTurn(Characters[gameId][i].Id, Characters[gameId][i].Playable);
                    break;
                }
            }
        }

        public void NextTurn(string gameId, int scene_id, int currentTurn)
        {
            var keys = Characters[gameId].Keys.ToList();
            Random rnd = new Random();
            int random = rnd.Next(0, keys.Count);
            var id = keys[random];

            if(Characters[gameId][id].Health > 0)
            {
                if(Characters[gameId][id].Scene_Id == scene_id)
                {
                    Characters[gameId][currentTurn].Is_Turn = false;
                    Characters[gameId][id].Is_Turn = true;
                }
            }

            Clients.Group(gameId).isTurn(Characters[gameId][id].Id, Characters[gameId][id].Playable);
            Clients.Group(gameId).runTurn();
        }

        public void Turn(string gameId, int characterId)
        {
            var character = db.Characters.Find(characterId);
            if (character.Playable == true)
            {
                Clients.Group(gameId).printScene("Narrador ", "Es turno de: " + character.Name);
                Clients.Group(gameId).playerTurn();
            }
            else
            {
                Clients.Group(gameId).printScene("Narrador ", "Es turno de: " + character.Name);
                Clients.Group(gameId).enemyTurn();
            }

        }

        public void EnemyTurn(string gameId)
        {
            Clients.Group(gameId).printScene("Narrador ", "Not implemented yet.");
            Clients.Group(gameId).whoIsNext();
        }

        public void ExecuteSkill(string gameId, int scene_id, int characterId, int skillId, string skillName, string targetId, string type, int dice)
        {
            var character = Characters[gameId][characterId];
            var connectionId = User_Characters[gameId][character];
            var target = new List<Character>();
            if (targetId == "selfOption")
            {
                target.Add(character);
            }
            else if(targetId == "enemiesOption")
            {
                var enemies_in_scene = db.Characters.Where(u => u.Scene_Id == scene_id).ToList();
                target = new List<Character>();
                var i = 1;
                foreach (var j in enemies_in_scene)
                {
                    if (Characters[gameId][j.Id].Health > 0)
                    {
                        target.Add(Characters[gameId][j.Id]);
                    }
                }
            }
            else if(targetId == "alliesOption")
            {
                 target = Characters[gameId].Values.Where(u => u.Playable == true).ToList();
            }
            else if (targetId.EndsWith("ally"))
            {
                target.Add(Characters[gameId][Int32.Parse(targetId.Substring(0, 1))]);
            }
            else
            {
                target.Add(Characters[gameId][Int32.Parse(targetId)]);
            }

            if(type == "damage")
            {
                foreach(var j in target)
                {
                    j.Health = j.Health - db.Skills.Find(skillId).Base_Power + dice;
                    Clients.Group(gameId).printScene("Narrador ", j.Name + " perdio " + db.Skills.Find(skillId).Base_Power + dice + " puntos de vida, ahora tiene: " + j.Health);
                }
            }
            else
            {
                foreach (var j in target)
                {
                    j.Health = j.Health + db.Skills.Find(skillId).Base_Power + dice;
                    Clients.Group(gameId).printScene("Narrador ", j.Name + " gano " + db.Skills.Find(skillId).Base_Power + dice + " puntos de vida, ahora tiene: " + j.Health);
                }
            }

            Clients.Group(gameId).whoIsNext();
        }

        public void EstablishSkills(string gameId, int characterId, string name, string targetId)
        {
            var character = Characters[gameId][characterId];
            var connectionId = User_Characters[gameId][character];
            if(character.Name == "Darius")
            {
                if(name == "Si mismo")
                {
                    var target = character;
                    Clients.Client(connectionId).setSkills(3, 1, db.Skills.Find(3).Name, targetId, "cure");
                }
                else
                {
                    var id = Int32.Parse(targetId);
                    var target = Characters[gameId][id];
                    for(int i = 1; i <= 2; i++)
                    {
                        Clients.Client(connectionId).setSkills(i, i, db.Skills.Find(i).Name, targetId, "damage");
                    }
                }
            }
            if (character.Name == "Veigar")
            {
                if(name == "Todos enemigos")
                {
                    Clients.Client(connectionId).setSkills(5, 1, db.Skills.Find(5).Name, targetId, "damage");
                }
                else
                {
                    Clients.Client(connectionId).setSkills(4, 1, db.Skills.Find(4).Name, targetId, "damage");
                    Clients.Client(connectionId).setSkills(6, 1, db.Skills.Find(6).Name, targetId, "damage");
                }
            }
            if (character.Name == "Ashe")
            {
                if (name == "Todos enemigos")
                {
                    Clients.Client(connectionId).setSkills(8, 1, db.Skills.Find(8).Name, targetId, "damage");
                }
                else if(name == "Todos aliados")
                {
                    Clients.Client(connectionId).setSkills(9, 1, db.Skills.Find(9).Name, targetId, "cure");
                }
                else
                {
                    Clients.Client(connectionId).setSkills(7, 1, db.Skills.Find(9).Name, targetId, "damage");
                }
            }
            if (character.Name == "Torin")
            {
                if (name == "Si mismo")
                {
                    var target = character;
                    Clients.Client(connectionId).setSkills(3, 1, db.Skills.Find(3).Name, targetId, "cure");
                }
                if (name == "Todos enemigos")
                {
                    Clients.Client(connectionId).setSkills(8, 1, db.Skills.Find(8).Name, targetId, "damage");
                }
                else if (targetId.EndsWith("ally"))
                {
                    Clients.Client(connectionId).setSkills(9, 1, db.Skills.Find(9).Name, targetId, "cure");
                }
                else
                {
                    Clients.Client(connectionId).setSkills(7, 1, db.Skills.Find(9).Name, targetId, "damage");
                }
            }
        }

        public void EstablishOptions(string gameId, int characterId, int scene_id)
        {
            var character = Characters[gameId][characterId];
            var connectionId = User_Characters[gameId][character];
            var enemies_in_scene = db.Characters.Where(u => u.Scene_Id == scene_id).ToList();
            var i = 1;
            foreach (var j in enemies_in_scene)
            {
                if (Characters[gameId][j.Id].Health > 0)
                {
                    Clients.Client(connectionId).setOption(i, j.Name, j.Id);
                    i++;
                }
            }
            if (character.Name == "Darius")
            {
                Clients.Client(connectionId).setOption(i, "Si mismo", "Si mismo");
            }
            else if (character.Name == "Veigar")
            {
                Clients.Client(connectionId).setOption(i, "Todos enemigos", "Todos enemigos");
            }
            else if (character.Name == "Ashe")
            {
                Clients.Client(connectionId).setOption(i, "Todos enemigos", "Todos enemigos");
                i++;
                Clients.Client(connectionId).setOption(i, "Todos aliados", "Todos aliados");
            }
            else if (character.Name == "Torin")
            {
                Clients.Client(connectionId).setOption(i, "Todos enemigos", "Todos enemigos");
                i++;
                Clients.Client(connectionId).setOption(i, "Darius", db.Characters.Where(u=>u.Name == "Darius").FirstOrDefault().Id);
                i++;
                Clients.Client(connectionId).setOption(i, "Veigar", db.Characters.Where(u => u.Name == "Veigar").FirstOrDefault().Id);
                i++;
                Clients.Client(connectionId).setOption(i, "Ashe", db.Characters.Where(u => u.Name == "Ashe").FirstOrDefault().Id);
                i++;
                Clients.Client(connectionId).setOption(i, "Si mismo", "Si mismo");

            }
        }

        public void PlayerTurn(string gameId, int characterId)
        {
            if (Characters[gameId][characterId].Health > 0)
            {
                var character = Characters[gameId][characterId];
                var connectionId = User_Characters[gameId][character];
                Clients.Client(connectionId).announceTurn();
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
            Clients.Group(gameId).printScene("Narrador", description);
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