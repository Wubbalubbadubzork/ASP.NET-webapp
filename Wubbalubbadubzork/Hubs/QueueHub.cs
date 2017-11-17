using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using System.Timers;
using Wubbalubbadubzork.Models;
using System.Threading.Tasks;

namespace Wubbalubbadubzork.Hubs
{
    public class QueueHub : Hub
    {
        public static ConcurrentQueue<string> Queue { get; set; } = new ConcurrentQueue<string>();

        public static Timer timer;

        public QueueHub()
        {
            timer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
        }

        public override Task OnConnected()
        {
            string connectionId = Context.ConnectionId;
            lock (Queue)
            {
                Queue.Enqueue(connectionId);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Queue = new ConcurrentQueue<string>(Queue.Except(new string[] { Context.ConnectionId }));
            return base.OnDisconnected(stopCalled);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Guid Id = Guid.NewGuid();
            if(Queue.Count >= 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    Queue.TryDequeue(out var connectionId);
                    Groups.Add(connectionId, Id.ToString());
                }
                Game g = HelperMethods.Create(Id);
                Clients.Group(Id.ToString()).Redirect(Id);
            }
        }
    }
}