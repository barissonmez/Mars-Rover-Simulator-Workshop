using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;

namespace MarsRovers.DTO
{
    public class CreatePlateauRequest
    {

        public int Width { get; set; }

        public int Height { get; set; }

        public Enums.BoundaryCreationStrategy BoundaryCreationStrategy { get; set; }
    }
}
