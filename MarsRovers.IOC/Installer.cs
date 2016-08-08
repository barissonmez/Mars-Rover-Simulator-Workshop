using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MarsRovers.Repository;
using MarsRovers.Service;

namespace MarsRovers.IOC
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPlateauRepository>().ImplementedBy<PlateauRepository>());
            container.Register(Component.For<IRoverRepository>().ImplementedBy<RoverRepository>());
            container.Register(Component.For<IPlateauService>().ImplementedBy<PlateauService>());
            container.Register(Component.For<IRoverService>().ImplementedBy<RoverService>());
        }
    }
}
