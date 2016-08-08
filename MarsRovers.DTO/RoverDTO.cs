using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;

namespace MarsRovers.DTO
{
    public class RoverDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public Enums.CompassPoint FacingTo { get; set; }
    }
}
