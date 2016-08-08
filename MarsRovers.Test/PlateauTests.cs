using System;
using System.Linq;
using MarsRovers.Common;
using MarsRovers.Entity;
using MarsRovers.Entity.BusinessRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsRovers.Test
{
    [TestClass]
    public class PlateauTests
    {
        [TestMethod]
        public void Should_create_plateau_correctly_if_width_and_height_are_greater_than_zero()
        {
            var plateau = new Plateau(5, 3, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            Assert.IsTrue(!plateau.BrokenRules.Any() && plateau.Width == 5 && plateau.Height == 3 && plateau.BoundaryCreationStrategy.Equals(Enums.BoundaryCreationStrategy.DetermineOnCreate));
        }

        [TestMethod]
        public void Should_not_create_plateau_if_width_and_height_are_not_greater_than_zero()
        {
            var plateau = new Plateau(0, 0, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            Assert.IsTrue(plateau.BrokenRules.Any() && plateau.BrokenRules.Contains(PlateauBusinessRules.WidthShouldBeGreaterThanZero) && plateau.BrokenRules.Contains(PlateauBusinessRules.HeightShouldBeGreaterThanZero));
        }

        [TestMethod]
        public void Creating_plateau_with_DetermineOnCreate_BoundaryCreationStrategy_should_work_correctly()
        {
            var plateau = new Plateau(3, 3, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            Assert.IsTrue(!plateau.BrokenRules.Any() && plateau.BoundaryPositions.Count > 0);
        }

        [TestMethod]
        public void Determined_boundary_positions_should_contain_the_position_0_0_West_and_0_0_South()
        {
            var plateau = new Plateau(5, 5, Enums.BoundaryCreationStrategy.DetermineOnCreate);

            var lowerLeftWestPosition = new BoundaryPosition(0, 0, Enums.CompassPoint.West); // 0,0,West position should be in boundaries list

            var lowerLeftSouthPosition = new BoundaryPosition(0, 0, Enums.CompassPoint.South); // 0,0,South position should be in boundaries list

            Assert.IsTrue(!plateau.BrokenRules.Any() && plateau.BoundaryPositions.Count > 0 && plateau.BoundaryPositions.Contains(lowerLeftWestPosition) && plateau.BoundaryPositions.Contains(lowerLeftSouthPosition));
        }

        [TestMethod]
        public void Creating_plateau_with_LearnFromFailure_BoundaryCreationStrategy_should_work_correctly()
        {
            var plateau = new Plateau(3, 3, Enums.BoundaryCreationStrategy.LearnFromFailure);

            var failedBoundaryPosition = new BoundaryPosition(0, 0, Enums.CompassPoint.South);

            plateau.BoundaryPositions.Add(failedBoundaryPosition);

            Assert.IsTrue(!plateau.BrokenRules.Any() && plateau.BoundaryPositions.Contains(failedBoundaryPosition));

        }
    }
}
