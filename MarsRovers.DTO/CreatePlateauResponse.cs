using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;

namespace MarsRovers.DTO
{
    public class CreatePlateauResponse : Response
    {
        public CreatePlateauResponse()
        {
            Data = new PlateauDTO();
        }

        public PlateauDTO Data { get; set; }
    }
}
