using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;

namespace MarsRovers.DTO
{
    public class PlateauDTO
    {
        public PlateauDTO()
        {
            BoundaryPositions = new List<BoundaryPositionDTO>();
        }

        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Enums.BoundaryCreationStrategy BoundaryCreationStrategy { get; set; }

        public List<BoundaryPositionDTO> BoundaryPositions { get; set; }

        public List<RoverDTO> Rovers { get; set; }
    }
}
