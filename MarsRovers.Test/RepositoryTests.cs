using System;
using System.Linq;
using Castle.Windsor;
using MarsRovers.Common;
using MarsRovers.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsRovers.Test
{
    [TestClass]
    public class RepositoryTests
    {
        private readonly WindsorContainer _container;

        public RepositoryTests()
        {
            _container = IOC.IOC.Install();
        }

        [TestMethod]
        public void Trying_to_create_a_plateau_should_return_created_plateau_entity()
        {
            var plateauRepository = _container.Resolve<IPlateauRepository>();

            var plateau = plateauRepository.Create(5, 5, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            Assert.IsTrue(plateau != null && !plateau.BrokenRules.Any() && plateau.Width == 5 && plateau.Height == 5 && plateau.BoundaryCreationStrategy == Enums.BoundaryCreationStrategy.DetermineOnCreate);
        }

        [TestMethod]
        public void Trying_to_create_a_rover_should_return_created_rover_entity()
        {
            var plateauRepository = _container.Resolve<IPlateauRepository>();

            var plateau = plateauRepository.Create(5, 5, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            var roverRepository = _container.Resolve<IRoverRepository>();

            var rover = roverRepository.Create(plateau, 2, 2, Enums.CompassPoint.North);

            Assert.IsTrue(rover != null && !rover.BrokenRules.Any() && rover.XPosition == 2 && rover.YPosition == 2 && rover.FacingTo == Enums.CompassPoint.North && plateau.Rovers.Contains(rover));
        }

    }
}
