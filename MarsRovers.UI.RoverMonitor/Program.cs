using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace MarsRovers.UI.RoverMonitor
{
    class Program
    {
        private static IHubProxy _hubProxy;

        static void Main(string[] args)
        {
            ArrangeApplicationTitle();

            ConnectToServerHub();

            Console.ReadLine();
        }

        private static void ArrangeApplicationTitle()
        {
            Console.Title = "Rovers Monitor";

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Monitoring Rovers On The Plateau");
            Console.WriteLine("Waiting for operations on MARS ROVERS APPLICATION...");
            Console.ResetColor();
        }

        private static void ConnectToServerHub()
        {
            var signalRHubURL = ConfigurationManager.AppSettings["SignalRHubURL"];
            var hubConnection = new HubConnection(signalRHubURL);
            _hubProxy = hubConnection.CreateHubProxy("ServerConnection");

            _hubProxy.On<PrivateMessage>("notifyRoverMonitor", msg => Console.WriteLine("{0}", msg.Message));

            hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());

            }).Wait();

            _hubProxy.Invoke("join", "ROVERMONITOR").ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("!!! There was an error opening the connection:{0} \n", task.Exception.GetBaseException());

            }).Wait();
        }

        private struct PrivateMessage
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Message { get; set; }
            public Guid MsgId { get; set; }
        }
    }
}
