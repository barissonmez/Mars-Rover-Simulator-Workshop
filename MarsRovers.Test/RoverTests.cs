using System;
using System.Linq;
using MarsRovers.Common;
using MarsRovers.Entity;
using MarsRovers.Entity.BusinessRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsRovers.Test
{
    [TestClass]
    public class RoverTests
    {
        [TestMethod]
        public void Creating_rover_with_values_1_2_N_should_work_correctly()
        {
            var plateau = new Plateau(5, 5, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            var rover = new Rover(plateau, 1, 2, Enums.CompassPoint.North);

            Assert.IsTrue(!plateau.BrokenRules.Any() && plateau.Rovers.Count == 1 && plateau.Rovers.Contains(rover) && rover.Plateau == plateau && rover.XPosition == 1 && rover.YPosition == 2 && rover.FacingTo.Equals(Enums.CompassPoint.North));

        }

        [TestMethod]
        public void Trying_to_create_rover_over_capacity_should_fail()
        {
            var plateau = new Plateau(2, 2, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            var rover1 = new Rover(plateau, 1, 2, Enums.CompassPoint.North);
            Assert.IsTrue(!plateau.BrokenRules.Any());

            var rover2 = new Rover(plateau, 1, 1, Enums.CompassPoint.North);
            Assert.IsTrue(!plateau.BrokenRules.Any());

            var rover3 = new Rover(plateau, 0, 0, Enums.CompassPoint.North);
            Assert.IsTrue(!plateau.BrokenRules.Any());

            var rover4 = new Rover(plateau, 2, 2, Enums.CompassPoint.North);
            Assert.IsTrue(!plateau.BrokenRules.Any());

            var rover5 = new Rover(plateau, 2, 1, Enums.CompassPoint.North);
            Assert.IsTrue(plateau.BrokenRules.Any() && plateau.BrokenRules.Contains(PlateauBusinessRules.RoverCapacityExceeds)); //Should fail here

        }

        [TestMethod]
        public void Trying_to_create_rover_on_the_same_coordinates_should_fail()
        {
            var plateau = new Plateau(2, 2, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            var rover1 = new Rover(plateau, 1, 2, Enums.CompassPoint.North);
            Assert.IsTrue(!plateau.BrokenRules.Any());

            var rover2 = new Rover(plateau, 1, 2, Enums.CompassPoint.North);
            Assert.IsTrue(plateau.BrokenRules.Any() && plateau.BrokenRules.Contains(PlateauBusinessRules.AnotherRoverIsExistOnTheSamePosition));//Should fail here

        }

        [TestMethod]
        public void Trying_to_create_rover_out_of_the_plateau_should_fail()
        {
            var plateau = new Plateau(2, 2, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            var rover1 = new Rover(plateau, 3, 3, Enums.CompassPoint.North);
            Assert.IsTrue(plateau.BrokenRules.Any() && plateau.BrokenRules.Contains(PlateauBusinessRules.RoverPositionIsOutOfThePlateauArea));
        }
    }
}
