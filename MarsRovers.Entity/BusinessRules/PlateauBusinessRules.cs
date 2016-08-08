using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.Entity.BusinessRules
{
    public class PlateauBusinessRules
    {
        public static readonly string WidthShouldBeGreaterThanZero = "Plateau' s < Width > should be greater than 0!";

        public static readonly string HeightShouldBeGreaterThanZero = "Plateau' s < Height > should be greater than 0!";

        public static readonly string RoverCapacityExceeds = "Plateau' s rover capacity is full. Cannot add any new rover.";

        public static readonly string AnotherRoverIsExistOnTheSamePosition = "Cannot add rover. Because another rover is exist on the same position.";
        
        public static readonly string RoverPositionIsOutOfThePlateauArea = "Cannot add rover. Because the rover' s position is out of the Plateau area.";


    }
}
