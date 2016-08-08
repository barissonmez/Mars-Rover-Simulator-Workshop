using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;

namespace MarsRovers.DTO
{
    public class CreateRoverResponse : Response
    {
        public CreateRoverResponse()
        {
            Data = new RoverDTO();
        }

        public RoverDTO Data { get; set; }
    }
}
