using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.DTO;

namespace MarsRovers.Service
{
    public interface IRoverService
    {
        CreateRoverResponse Create(CreateRoverRequest request);

        RoverDTO GetById(Guid id);

        MoveRoverResponse Move(MoveRoverRequest request);
    }
}
