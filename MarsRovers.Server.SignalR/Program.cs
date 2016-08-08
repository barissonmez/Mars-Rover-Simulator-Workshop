using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace MarsRovers.Server.SignalR
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SignalR Server";

            var signalRHubURL = ConfigurationManager.AppSettings["SignalRHubURL"];

            using (WebApp.Start<Startup>(signalRHubURL))
            {
                Console.WriteLine("Server running at {0}", signalRHubURL);
                Console.ReadLine();
            }

        }
    }
}
