using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.Entity;

namespace MarsRovers.Repository
{
    public interface IPlateauRepository
    {
        Plateau Create(int width, int height, Enums.BoundaryCreationStrategy boundaryCreationStrategy);

        Plateau GetById(Guid id);
    }
}
