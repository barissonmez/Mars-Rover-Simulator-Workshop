using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;
using MarsRovers.DTO;
using MarsRovers.Entity;
using MarsRovers.Repository;

namespace MarsRovers.Service
{
    public class RoverService : IRoverService
    {
        private readonly IRoverRepository _roverRepository;
        private readonly IPlateauRepository _plateauRepository;

        public RoverService(IRoverRepository roverRepository, IPlateauRepository plateauRepository)
        {
            _roverRepository = roverRepository;
            _plateauRepository = plateauRepository;
        }

        public CreateRoverResponse Create(CreateRoverRequest request)
        {
            var response = new CreateRoverResponse();

            var plateau = _plateauRepository.GetById(request.PlateauId);

            //var rover = new Rover(plateau, request.XPosition, request.YPosition, request.FacingTo);

            var rover = _roverRepository.Create(plateau, request.XPosition, request.YPosition, request.FacingTo);

            if (plateau.BrokenRules.Any())
            {
                response.Success = false;
                plateau.BrokenRules.ForEach(a => response.Messages.Add(a));
            }
            else if (rover.BrokenRules.Any())
            {
                response.Success = false;
                rover.BrokenRules.ForEach(a => response.Messages.Add(a));
            }
            else
            {
                response.Data.Id = rover.Id;
                response.Data.Name = rover.Name;
                response.Data.XPosition = rover.XPosition;
                response.Data.YPosition = rover.YPosition;
                response.Data.FacingTo = rover.FacingTo;
            }

            return response;
        }

        public RoverDTO GetById(Guid id)
        {
            var response = new RoverDTO();

            var rover = _roverRepository.GetById(id);

            if (rover != null)
            {
                response.Id = rover.Id;
                response.Name = rover.Name;
                response.XPosition = rover.XPosition;
                response.YPosition = rover.YPosition;
                response.FacingTo = rover.FacingTo;
            }

            return response;

        }

        public MoveRoverResponse Move(MoveRoverRequest request)
        {
            var response = new MoveRoverResponse();

            var rover = _roverRepository.GetById(request.Id);

            var commands = request.Commands.ToArray();

            foreach (var command in commands)
            {
                if (command.Equals('l') || command.Equals('L'))
                    TurnLeft(ref rover);

                if (command.Equals('r') || command.Equals('R'))
                    TurnRight(ref rover);

                if (command.Equals('m') || command.Equals('M'))
                {
                    var plateauBoundaries = rover.Plateau.BoundaryPositions;

                    switch (rover.FacingTo)
                    {
                        case Enums.CompassPoint.North:
                            MoveToNorth(ref rover, ref plateauBoundaries, ref response);
                            break;
                        case Enums.CompassPoint.South:
                            MoveToSouth(ref rover, ref plateauBoundaries, ref response);
                            break;
                        case Enums.CompassPoint.East:
                            MoveToEast(ref rover, ref plateauBoundaries, ref response);
                            break;
                        case Enums.CompassPoint.West:
                            MoveToWest(ref rover, ref plateauBoundaries, ref response);
                            break;
                    }

                }
            }

            response.NewPosition = new RoverDTO
            {
                Id = rover.Id,
                Name = rover.Name,
                XPosition = rover.XPosition,
                YPosition = rover.YPosition,
                FacingTo = rover.FacingTo
            };

            return response;
        }

        private void MoveToWest(ref Rover rover, ref List<BoundaryPosition> plateauBoundaries, ref MoveRoverResponse response)
        {
            var boundaryCreationStrategy = rover.Plateau.BoundaryCreationStrategy;

            if (Enums.BoundaryCreationStrategy.LearnFromFailure.Equals(boundaryCreationStrategy))
                IfRoverOutOfBoundaryThenRecordCurrentPosition(ref plateauBoundaries, rover);

            if (rover.XPosition <= 0 || IsRoverOnBoundaryPoint(rover, plateauBoundaries))
                OutOfPlateauFailure(ref response, rover);
            else if (IsThereAnyOtherRoverOnTheRoute(rover, Enums.CompassPoint.West))
                ThereIsAnyOtherRoverOnTheRouteFailure(ref response, rover);
            else
                rover.XPosition = rover.XPosition - 1;
        }

        private void ThereIsAnyOtherRoverOnTheRouteFailure(ref MoveRoverResponse response, Rover rover)
        {
            response.Success = false;
            response.Messages.Add(string.Format("Rover <{0}> cannot move to new position. Because there is another rover on the route.", rover.Name));
            response.Messages.Add(string.Format("Rover <{0}> 's last position: {1} {2} {3}", rover.Name, rover.XPosition, rover.YPosition, Helper.ConvertToCompassDirection(rover.FacingTo)));
        }

        private void OutOfPlateauFailure(ref MoveRoverResponse response, Rover rover)
        {
            response.Success = false;
            response.Messages.Add(string.Format("Rover <{0}> cannot move to new position. Because new position is out of the plateau area.", rover.Name));
            response.Messages.Add(string.Format("Rover <{0}> 's last position: {1} {2} {3}", rover.Name, rover.XPosition, rover.YPosition, Helper.ConvertToCompassDirection(rover.FacingTo)));
        }

        private void MoveToEast(ref Rover rover, ref List<BoundaryPosition> plateauBoundaries, ref MoveRoverResponse response)
        {
            var boundaryCreationStrategy = rover.Plateau.BoundaryCreationStrategy;

            if (Enums.BoundaryCreationStrategy.LearnFromFailure.Equals(boundaryCreationStrategy))
                IfRoverOutOfBoundaryThenRecordCurrentPosition(ref plateauBoundaries, rover);

            if (rover.XPosition >= rover.Plateau.Width || IsRoverOnBoundaryPoint(rover, plateauBoundaries))
                OutOfPlateauFailure(ref response, rover);
            else if (IsThereAnyOtherRoverOnTheRoute(rover, Enums.CompassPoint.East))
                ThereIsAnyOtherRoverOnTheRouteFailure(ref response, rover);
            else
                rover.XPosition = rover.XPosition + 1;
        }

        private void MoveToSouth(ref Rover rover, ref List<BoundaryPosition> plateauBoundaries, ref MoveRoverResponse response)
        {
            var boundaryCreationStrategy = rover.Plateau.BoundaryCreationStrategy;

            if (Enums.BoundaryCreationStrategy.LearnFromFailure.Equals(boundaryCreationStrategy))
                IfRoverOutOfBoundaryThenRecordCurrentPosition(ref plateauBoundaries, rover);

            if (rover.YPosition <= 0 || IsRoverOnBoundaryPoint(rover, plateauBoundaries))
                OutOfPlateauFailure(ref response, rover);
            else if (IsThereAnyOtherRoverOnTheRoute(rover, Enums.CompassPoint.South))
                ThereIsAnyOtherRoverOnTheRouteFailure(ref response, rover);
            else
                rover.YPosition = rover.YPosition - 1;
        }

        private void MoveToNorth(ref Rover rover, ref List<BoundaryPosition> plateauBoundaries, ref MoveRoverResponse response)
        {
            var boundaryCreationStrategy = rover.Plateau.BoundaryCreationStrategy;

            if (Enums.BoundaryCreationStrategy.LearnFromFailure.Equals(boundaryCreationStrategy))
                IfRoverOutOfBoundaryThenRecordCurrentPosition(ref plateauBoundaries, rover);

            if (rover.YPosition >= rover.Plateau.Height || IsRoverOnBoundaryPoint(rover, plateauBoundaries))
                OutOfPlateauFailure(ref response, rover);
            else if (IsThereAnyOtherRoverOnTheRoute(rover, Enums.CompassPoint.North))
                ThereIsAnyOtherRoverOnTheRouteFailure(ref response, rover);
            else
                rover.YPosition = rover.YPosition + 1;
        }

        private void IfRoverOutOfBoundaryThenRecordCurrentPosition(ref List<BoundaryPosition> plateauBoundaries, Rover rover)
        {
            switch (rover.FacingTo)
            {
                case  Enums.CompassPoint.North :
                    {
                        var topMostYPosition = rover.Plateau.Height;
                        var yPositionAfterMove = rover.YPosition + 1;

                        if (yPositionAfterMove > topMostYPosition)
                            plateauBoundaries.Add(new BoundaryPosition(rover.XPosition, rover.YPosition, Enums.CompassPoint.North));

                        break;
                    }
                case Enums.CompassPoint.South:
                    {
                        const int bottomMostYPosition = 0;
                        var yPositionAfterMove = rover.YPosition -1;

                        if (yPositionAfterMove < bottomMostYPosition)
                            plateauBoundaries.Add(new BoundaryPosition(rover.XPosition, rover.YPosition, Enums.CompassPoint.South));

                        break;
                    }
                case Enums.CompassPoint.East:
                    {
                        var topMostXPosition = rover.Plateau.Width;
                        var xPositionAfterMove = rover.XPosition + 1;

                        if (xPositionAfterMove > topMostXPosition)
                            plateauBoundaries.Add(new BoundaryPosition(rover.XPosition, rover.YPosition, Enums.CompassPoint.East));

                        break;
                    }
                case Enums.CompassPoint.West:
                    {
                        const int bottomMostXPosition = 0;
                        var xPositionAfterMove = rover.XPosition - 1;

                        if (xPositionAfterMove < bottomMostXPosition)
                            plateauBoundaries.Add(new BoundaryPosition(rover.XPosition, rover.YPosition, Enums.CompassPoint.West));

                        break;
                    }
            }
        }

        private bool IsThereAnyOtherRoverOnTheRoute(Rover rover, Enums.CompassPoint compassPoint)
        {
            var rovers = rover.Plateau.Rovers.Where(a => a.Id != rover.Id);

            var movingTo = new Position();

            switch (compassPoint)
            {
                case Enums.CompassPoint.North:
                    {
                        movingTo.X = rover.XPosition;
                        movingTo.Y = rover.YPosition + 1;
                        break;
                    }
                case Enums.CompassPoint.South:
                    {
                        movingTo.X = rover.XPosition;
                        movingTo.Y = rover.YPosition - 1;
                        break;
                    }
                case Enums.CompassPoint.East:
                    {
                        movingTo.X = rover.XPosition + 1;
                        movingTo.Y = rover.YPosition;
                        break;
                    }
                case Enums.CompassPoint.West:
                    {
                        movingTo.X = rover.XPosition -1;
                        movingTo.Y = rover.YPosition;
                        break;
                    }
                }

            return rovers.Any(
                    a =>
                    a.XPosition.Equals(movingTo.X) && a.YPosition.Equals(movingTo.Y));
        }

        private bool IsRoverOnBoundaryPoint(Rover rover,IEnumerable<BoundaryPosition> plateauBoundaries)
        {
 	
            return plateauBoundaries.Any(
                    a =>
                    a.XPosition.Equals(rover.XPosition) && a.YPosition.Equals(rover.YPosition) &&
                    a.FacingTo.Equals(rover.FacingTo));

        }

        private void TurnRight(ref Rover rover)
        {
            if (rover.FacingTo == Enums.CompassPoint.East)
                rover.FacingTo = Enums.CompassPoint.South;
            else
                rover.FacingTo -= 90;
        }

        private void TurnLeft(ref Rover rover)
        {
            if (rover.FacingTo == Enums.CompassPoint.South)
                rover.FacingTo = Enums.CompassPoint.East;
            else
                rover.FacingTo += 90;
        }

        private struct Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

    }
}
