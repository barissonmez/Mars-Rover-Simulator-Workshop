using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;

namespace MarsRovers.IOC
{
    public class IOC
    {
        public static WindsorContainer Install()
        {
            var container = new WindsorContainer();
            container.Install(new Installer());

            return container;
        }
    }
}
