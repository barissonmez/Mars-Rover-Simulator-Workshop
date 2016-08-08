using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;

namespace MarsRovers.Entity
{
    public class BoundaryPosition
    {
        public BoundaryPosition(int xPosition, int yPosition, Enums.CompassPoint facingTo)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            FacingTo = facingTo;
        }

        public int XPosition { get; private set; }

        public int YPosition { get; private set; }

        public Enums.CompassPoint FacingTo { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as BoundaryPosition;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;
            return true;
        }

        public static bool operator ==(BoundaryPosition a, BoundaryPosition b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(BoundaryPosition a, BoundaryPosition b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType()).GetHashCode();
        }
    }
}
