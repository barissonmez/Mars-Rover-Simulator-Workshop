using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.DTO;
using MarsRovers.Repository;

namespace MarsRovers.Service
{
    public class PlateauService : IPlateauService
    {
        private readonly IPlateauRepository _plateauRepository;

        public PlateauService(IPlateauRepository plateauRepository)
        {
            _plateauRepository = plateauRepository;
        }

        public CreatePlateauResponse Create(CreatePlateauRequest request)
        {
            var plateau = _plateauRepository.Create(request.Width, request.Height, request.BoundaryCreationStrategy);

            var response = new CreatePlateauResponse();

            if (plateau.BrokenRules.Any())
            {
                response.Success = false;
                plateau.BrokenRules.ForEach(a=> response.Messages.Add(a));
            }
            else
            {
                response.Data.Id = plateau.Id;
                response.Data.Width = plateau.Width;
                response.Data.Height = plateau.Height;
                response.Data.BoundaryCreationStrategy = plateau.BoundaryCreationStrategy;
                response.Data.BoundaryPositions = plateau.BoundaryPositions.Select(a => new BoundaryPositionDTO
                    {
                        XPosition = a.XPosition,
                        YPosition = a.YPosition,
                        FacingTo = a.FacingTo
                    }).ToList();
                response.Data.Rovers = plateau.Rovers.Select(a => new RoverDTO
                    {
                        Id =  a.Id,
                        Name =  a. Name,
                        XPosition = a.XPosition,
                        YPosition = a.YPosition,
                        FacingTo = a.FacingTo
                    }).ToList();
            }

            return response;
        }




        public PlateauDTO GetById(Guid id)
        {
            var response = new PlateauDTO();

            var plateau = _plateauRepository.GetById(id);

            if (plateau != null)
            {
                response.Id = plateau.Id;
                response.Width = plateau.Width;
                response.Height = plateau.Height;
                response.BoundaryCreationStrategy = plateau.BoundaryCreationStrategy;
                response.BoundaryPositions = plateau.BoundaryPositions.Select(a => new BoundaryPositionDTO
                {
                    XPosition = a.XPosition,
                    YPosition = a.YPosition,
                    FacingTo = a.FacingTo
                }).ToList();
                response.Rovers = plateau.Rovers.Select(a => new RoverDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    XPosition = a.XPosition,
                    YPosition = a.YPosition,
                    FacingTo = a.FacingTo
                }).ToList();
            }

            return response;
        }
    }
}
