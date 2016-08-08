using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.Common
{
    public class Enums
    {
        public enum CompassPoint
        {
            East = 0,
            North = 90,
            West = 180,
            South = 270
        }

        public enum Rotation
        {
            Left = 90,
            Right = -90
        }

        public enum GridEdge
        {
            Right = 0,
            Top = 90,
            Left = 180,
            Bottom = 270

        }

        public enum BoundaryCreationStrategy
        {
            DetermineOnCreate = 1,
            LearnFromFailure = 2
        }

        public enum MovingCommands
        {
            R = 0,
            L = 1,
            M = 2
        }

        public enum CompassDirections
        {
            N = 0,
            E = 1,
            S = 2,
            W = 3
        }
    }
}
