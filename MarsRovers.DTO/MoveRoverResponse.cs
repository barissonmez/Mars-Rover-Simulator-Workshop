using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.DTO
{
    public class MoveRoverResponse : Response
    {
        public MoveRoverResponse()
        {
            NewPosition = new RoverDTO();
        }

        public RoverDTO NewPosition { get; set; }
    }
}
