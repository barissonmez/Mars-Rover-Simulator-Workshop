using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.Entity;

namespace MarsRovers.Repository
{
    public interface IRoverRepository 
    {
        Rover Create(Plateau plateau, int xPosition, int yPosition, Enums.CompassPoint facingTo);

        Rover GetById(Guid id);
    }
}
