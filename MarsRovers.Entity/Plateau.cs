using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.Entity.BusinessRules;

namespace MarsRovers.Entity
{
    public class Plateau : EntityBase
    {
        public Plateau()
        {
            Rovers = new List<Rover>();
            BoundaryPositions = new List<BoundaryPosition>();
        }

        public Plateau(int width, int height, Enums.BoundaryCreationStrategy boundaryCreationStrategy) : this()
        {
            Height = height;
            Width = width;
            BoundaryCreationStrategy = boundaryCreationStrategy;

            Validate();

            if (BrokenRules.Any()) return;


            if (BoundaryCreationStrategy == Enums.BoundaryCreationStrategy.DetermineOnCreate)
                DetermineBoundaryPositions();
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public List<BoundaryPosition> BoundaryPositions { get; private set; }

        public Enums.BoundaryCreationStrategy BoundaryCreationStrategy { get; private set; }

        public List<Rover> Rovers { get; set; }

        public int RoverCapacity { get { return Width * Height; } }

        public bool CanAddRover()
        {
            if (RoverCapacity > Rovers.Count)
            {
                BrokenRules.Clear();
                return true;
            }

            AddBrokenRule(PlateauBusinessRules.RoverCapacityExceeds);
            return false;
        }

        public bool CanAddRover(int xPosition, int yPosition)
        {
            if (Rovers.Any(a => a.XPosition.Equals(xPosition) && a.YPosition.Equals(yPosition)))
            {
                AddBrokenRule(PlateauBusinessRules.AnotherRoverIsExistOnTheSamePosition);
                return false;
            }
            
            if (xPosition < 0 || yPosition < 0 ||  xPosition > Width || yPosition > Height)
            {
                AddBrokenRule(PlateauBusinessRules.RoverPositionIsOutOfThePlateauArea);
                return false;
            }

            BrokenRules.Clear();
            return true;
        }

        private void DetermineBoundaryPositions()
        {
            foreach (Enums.GridEdge gridEdge in Enum.GetValues(typeof(Enums.GridEdge)))
            {
                DetermineGridEdgeBoundaryPositions(gridEdge);
            }
        }

        private void DetermineGridEdgeBoundaryPositions(Enums.GridEdge gridEdge)
        {
            const int minRow = 0;
            var maxRow = Width;
            const int minColumn = 0;
            var maxColumn = Height;

            var facingTo = FacingTo(gridEdge);

            if (gridEdge == Enums.GridEdge.Bottom || gridEdge == Enums.GridEdge.Top)
            {
                var yPoint = gridEdge == Enums.GridEdge.Bottom ? minColumn : maxColumn;

                for (var column = 0; column <= maxColumn; column++)
                {
                    var boundryPosition = new BoundaryPosition(column, yPoint, facingTo);
                    BoundaryPositions.Add(boundryPosition);
                }
            }
            else if (gridEdge == Enums.GridEdge.Right || gridEdge == Enums.GridEdge.Left)
            {
                var xPoint = gridEdge == Enums.GridEdge.Left ? minRow : maxRow;

                for (var row = 0; row <= maxRow; row++)
                {
                    var boundryPosition = new BoundaryPosition(xPoint, row, facingTo);

                    BoundaryPositions.Add(boundryPosition);
                }
            }
        }

        private static Enums.CompassPoint FacingTo(Enums.GridEdge gridEdge)
        {
            return (Enums.CompassPoint)((int)gridEdge);
        }

        protected override sealed void Validate()
        {
            if (Width <= 0)
                AddBrokenRule(PlateauBusinessRules.WidthShouldBeGreaterThanZero);

            if (Height <= 0)
                AddBrokenRule(PlateauBusinessRules.HeightShouldBeGreaterThanZero);
        }

    }
}
