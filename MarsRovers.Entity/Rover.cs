using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.Entity.BusinessRules;

namespace MarsRovers.Entity
{
    public class Rover : EntityBase
    {
        public Rover()
        {
        }

        public Rover(Plateau plateau, int xPosition, int yPosition, Enums.CompassPoint facingTo)
            : this()
        {
            if (!plateau.CanAddRover() || !plateau.CanAddRover(xPosition, yPosition)) return;

            Plateau = plateau;
            XPosition = xPosition;
            YPosition = yPosition;
            FacingTo = facingTo;
            Name = Helper.GenerateName();
            Plateau.Rovers.Add(this);
        }

        public Plateau Plateau { get; private set; }

        public string Name { get; private set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public Enums.CompassPoint FacingTo { get; set; }

        protected override void Validate()
        {
            if (XPosition < 0)
                AddBrokenRule(RoverBusinessRules.XPositionShouldNotBeLowerThanZero);

            if (YPosition < 0)
                AddBrokenRule(RoverBusinessRules.YPositionShouldNotBeLowerThanZero);


        }
    }
}
