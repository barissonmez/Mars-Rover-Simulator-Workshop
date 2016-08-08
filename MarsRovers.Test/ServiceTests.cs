using System;
using Castle.Windsor;
using MarsRovers.Common;
using MarsRovers.DTO;
using MarsRovers.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsRovers.Test
{
    [TestClass]
    public class ServiceTests
    {
        private readonly WindsorContainer _container;

        public ServiceTests()
        {
            _container = IOC.IOC.Install();
        }

        [TestMethod]
        public void Trying_to_create_plateau_should_return_created_plateau_response()
        {
            var plateauService = _container.Resolve<IPlateauService>();
            var request = new CreatePlateauRequest
                {
                    Width = 5,
                    Height = 5,
                    BoundaryCreationStrategy = Enums.BoundaryCreationStrategy.DetermineOnCreate
                };

            var response = plateauService.Create(request);

            Assert.IsTrue(response.Success && response.Data.Width == 5 && response.Data.Height == 5);
        }
    }
}
