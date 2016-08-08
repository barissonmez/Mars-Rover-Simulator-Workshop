using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.Entity;

namespace MarsRovers.Repository
{
    public class PlateauRepository : IPlateauRepository
    {
        public Plateau Create(int width, int height, Enums.BoundaryCreationStrategy boundaryCreationStrategy)
        {
            var plateau = new Plateau(width, height, boundaryCreationStrategy);

            DataContext.Plateaus.Add(plateau);

            return plateau;
        }


        public Plateau GetById(Guid id)
        {
            return DataContext.Plateaus.FirstOrDefault(a => a.Id.Equals(id));
        }
    }
}
