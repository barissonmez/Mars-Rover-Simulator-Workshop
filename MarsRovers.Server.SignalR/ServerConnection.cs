using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace MarsRovers.Server.SignalR
{
    public class ServerConnection : Hub
    {
        public Task Join(string groupName)
        {
            Console.WriteLine("{0} successfully connected.", groupName);
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task Leave(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        public void NotifyRoverMonitor(PrivateMessage msg)
        {
            Clients.Group(msg.To).notifyRoverMonitor(msg);
        }

        public struct PrivateMessage
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Message { get; set; }
            public Guid MsgId { get; set; }
        }
    }

    
}
