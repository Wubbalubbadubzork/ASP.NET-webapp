using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Wubbalubbadubzork.Hubs
{
    public class GameHub : Hub
    {
        public void JoinGame(string gameId)
        {
            string connectionId = Context.ConnectionId;
            Groups.Add(connectionId, gameId);
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