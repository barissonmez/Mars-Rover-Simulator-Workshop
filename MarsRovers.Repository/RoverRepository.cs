using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.Entity;

namespace MarsRovers.Repository
{
    public class RoverRepository : IRoverRepository
    {
        public Rover Create(Plateau plateau, int xPosition, int yPosition, Enums.CompassPoint facingTo)
        {
            var rover = new Rover(plateau, xPosition, yPosition, facingTo);

            DataContext.Rovers.Add(rover);

            return rover;
        }


        public Rover GetById(Guid id)
        {
            return DataContext.Rovers.FirstOrDefault(a => a.Id.Equals(id));
        }
    }
}
