using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.DTO;

namespace MarsRovers.Service
{
    public interface IPlateauService
    {
        CreatePlateauResponse Create(CreatePlateauRequest request);
        PlateauDTO GetById(Guid id);
    }
}
