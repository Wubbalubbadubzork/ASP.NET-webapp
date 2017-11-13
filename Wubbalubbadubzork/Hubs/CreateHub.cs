using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Wubbalubbadubzork.Hubs
{
    public class CreateHub : Hub
    {
        public static Dictionary<string, int> Dictionary = new Dictionary<string, int>();
        public void JoinGame(string gameID)
        {
            var connectionId = Context.ConnectionId;
            Groups.Add(connectionId, gameID);
            if (Dictionary.Keys.Contains(gameID) == true)
            {
                Dictionary[gameID] = Dictionary[gameID] + 1;
                Clients.Group(gameID).CurrCount(Dictionary[gameID]);
            }
            else
            {
                Dictionary.Add(gameID, 0);
            }

            Clients.Group(gameID).CurrCount(Dictionary[gameID]);
            Clients.Group(gameID).UpdateStrings();
        }

        public void LeaveGame(string gameID)
        {
            var connectionId = Context.ConnectionId;
            Groups.Remove(connectionId, gameID);
            if (Dictionary.Keys.Contains(gameID) == true)
            {
                Dictionary[gameID] = Dictionary[gameID] - 1;
                Clients.Group(gameID).CurrCount(Dictionary[gameID]);
            }
            Clients.Group(gameID).CurrCount(Dictionary[gameID]);
            Clients.Group(gameID).UpdateStrings();
        }
    }
}