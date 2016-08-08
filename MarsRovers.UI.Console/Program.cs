using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using MarsRovers.Common;
using MarsRovers.DTO;
using MarsRovers.Service;
using Microsoft.AspNet.SignalR.Client;

namespace MarsRovers.UI.ConsoleApp
{
    class Program
    {
        private static WindsorContainer _container;
        private static IPlateauService _plateauService;
        private static IRoverService _roverService;
        private static IHubProxy _hubProxy;


        static void Main(string[] args)
        {
            _container = IOC.IOC.Install();
            _plateauService = _container.Resolve<IPlateauService>();
            _roverService = _container.Resolve<IRoverService>();

            ArrangeApplicationTitle();

            ConnectToServerHub();

            var plateau = CallCreatePlateauView();

            while (true)
            {
                CallRoverManagementView(plateau.Data.Id);
            }

        }

        private static void ConnectToServerHub()
        {
            var signalRHubURL = ConfigurationManager.AppSettings["SignalRHubURL"];
            var hubConnection = new HubConnection(signalRHubURL);
            _hubProxy = hubConnection.CreateHubProxy("ServerConnection");

            hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());

            }).Wait();

            _hubProxy.Invoke("join", "MARSROVERSAPP").ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("!!! There was an error opening the connection:{0} \n", task.Exception.GetBaseException());

            }).Wait();
        }

        private static void SendMessageToMonitor(string message)
        {
            var msg = new PrivateMessage { From = "MARSROVERSAPP", MsgId = Guid.NewGuid(), Message = message, To = "ROVERMONITOR" };

            _hubProxy.Invoke<PrivateMessage>("notifyRoverMonitor", msg).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("!!! There was an error opening the connection:{0} \n",task.Exception.GetBaseException());

            }).Wait();
        }

        private static void ArrangeApplicationTitle()
        {
            Console.Title = "Control Panel";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("MARS ROVERS APPLICATION");
            ResetColor();
        }

        private static void CallRoverManagementView(Guid plateauId)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You can control your rover(s) from the menu below.");

            var menuItems = GenerateMenuItems(plateauId);

            var selectedMenu = string.Empty;

            var isSelectedMenuValid = false;

            while (!isSelectedMenuValid)
            {
                selectedMenu = Console.ReadLine();

                isSelectedMenuValid = CheckIfSelectedMenuIsValid(selectedMenu, menuItems.Count);

                if (isSelectedMenuValid) continue;

                ShowErrorMessage("Please select only one of the options from menu above");

            }

            var selectedMenuIndex = int.Parse(selectedMenu);

            if (selectedMenuIndex < menuItems.Count)
            {
                var roverItem = menuItems.ElementAt(selectedMenuIndex - 1); //Because index is zero-based

                var rover = _roverService.GetById(roverItem.Key);

                ManageRover(rover);

            }
            else
                CreateRover(plateauId);


            ResetColor();
        }

        private static void ManageRover(RoverDTO rover)
        {
            var movingCommandValuesAreValid = false;

            var movingCommands = string.Empty;

            while (!movingCommandValuesAreValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine("Rover <{0}> is waiting for your command(s) to move. Commands: L->Turn Left, R->Turn Right, M->Move", rover.Name);

                Console.WriteLine("Rover <{0}>' s current position: {1} {2} {3}", rover.Name, rover.XPosition, rover.YPosition, Helper.ConvertToCompassDirection(rover.FacingTo));

                movingCommands = Console.ReadLine();

                movingCommandValuesAreValid = CheckIfMovingCommandValuesAreValid(movingCommands);

                if (movingCommandValuesAreValid) continue;

                ShowErrorMessage("Please enter the values in appropriate format. Commands: L->Turn Left, R->Turn Right, M->Move");

            }

            MoveRover(rover.Id, movingCommands);
        }

        private static void MoveRover(Guid roverId, string movingCommands)
        {
            var request = new MoveRoverRequest
                {
                    Id = roverId,
                    Commands = movingCommands

                };

            var response = _roverService.Move(request);

            if (!response.Success)
                ShowErrorMessages(response.Messages);
            else
                ShowSuccessMessage(string.Format("Rover <{0}> successfully moved to new position: {1} {2} {3}", response.NewPosition.Name, response.NewPosition.XPosition, response.NewPosition.YPosition, Helper.ConvertToCompassDirection(response.NewPosition.FacingTo)));

            var msg = string.Format("Rover <{0}> moved to: {1} {2} {3}",
                                        response.NewPosition.Name, response.NewPosition.XPosition,
                                        response.NewPosition.YPosition,
                                        Helper.ConvertToCompassDirection(response.NewPosition.FacingTo));
            SendMessageToMonitor(msg);
        }

        private static bool CheckIfMovingCommandValuesAreValid(string movingCommands)
        {
            var charsToRemove = new[] { 'L', 'l', 'R', 'r', 'M', 'm' };

            movingCommands = charsToRemove.Aggregate(movingCommands, (current, c) => current.Replace(c.ToString(), string.Empty));

            return string.IsNullOrEmpty(movingCommands);
        }

        private static void CreateRover(Guid plateauId)
        {

            var isRoverCreatedSuccessfully = false;

            while (!isRoverCreatedSuccessfully)
            {

                var roverPosition = GetRoverPositionFromUser();

                var request = new CreateRoverRequest
                {
                    PlateauId = plateauId,
                    XPosition = int.Parse(roverPosition.FirstOrDefault(a => a.Key.Equals("XPosition")).Value),
                    YPosition = int.Parse(roverPosition.FirstOrDefault(a => a.Key.Equals("YPosition")).Value),
                    FacingTo = Helper.ConvertToCompassPoint((Enums.CompassDirections)Enum.Parse(typeof(Enums.CompassDirections), roverPosition.FirstOrDefault(a => a.Key.Equals("FacingTo")).Value))
                };

                var response = _roverService.Create(request);

                if (!response.Success)
                    ShowErrorMessages(response.Messages);
                else
                {
                    isRoverCreatedSuccessfully = true;
                    ShowSuccessMessage(string.Format("Rover <{0}>  created and located on position <{1} {2} {3}>", response.Data.Name, response.Data.XPosition, response.Data.YPosition, Helper.ConvertToCompassDirection(response.Data.FacingTo)));

                    var msg = string.Format("Rover <{0}> located on: <{1} {2} {3}>", response.Data.Name, response.Data.XPosition, response.Data.YPosition, Helper.ConvertToCompassDirection(response.Data.FacingTo));
                    SendMessageToMonitor(msg);
                }

            }

            ResetColor();
        }

        private static bool CheckIfSelectedMenuIsValid(string selectedMenu, int menuCount)
        {
            if (string.IsNullOrEmpty(selectedMenu)) return false;

            int selectedMenuItem;

            var canBeParsedToInt = int.TryParse(selectedMenu, out selectedMenuItem);

            if (!canBeParsedToInt) return false;

            return selectedMenuItem > 0 && selectedMenuItem <= menuCount;

        }

        private static List<KeyValuePair<Guid, string>> GenerateMenuItems(Guid plateauId)
        {
            var plateau = _plateauService.GetById(plateauId);

            var menuItems = plateau.Rovers
                                   .Select((x, index) => new {x.Id, x.Name, index})
                                   .Select(a => new KeyValuePair<Guid, string>(a.Id, string.Format("{0} - Control Rover <{1}>", a.index + 1, a.Name))).ToList();

            menuItems.Add( new KeyValuePair<Guid, string>(Guid.NewGuid(), string.Format("{0} - Create a new rover", menuItems.Count + 1)));

            menuItems.ForEach(a=> Console.WriteLine(a.Value));

            return menuItems;
        }

        private static CreatePlateauResponse CallCreatePlateauView()
        {
            var response = new CreatePlateauResponse();

            var isPlateauCreated = false;

            while (!isPlateauCreated)
            {
                var plateauObject = CreatePlateau();

                if (plateauObject.Success)
                {
                    isPlateauCreated = true;
                    response = plateauObject;
                    ShowSuccessMessage("Plateau is ready to be discovered.");
                    ShowSuccessMessage("You need at least one rover to discover the plateau.");

                    var msg = string.Format("Plateau({0}X{1}) is created and ready to be discovered.", plateauObject.Data.Width, plateauObject.Data.Height);
                    SendMessageToMonitor(msg);

                }
                else
                    ShowErrorMessages(plateauObject.Messages);
            }

            return response;
        }

        private static void ShowErrorMessages(List<string> messages)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            messages.ForEach(Console.WriteLine);
            ResetColor();
        }

        private static void ResetColor()
        {
            Console.ResetColor();
        }

        private static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            ResetColor();
        }

        private static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            ResetColor();
        }

        private static CreatePlateauResponse CreatePlateau()
        {
            var widthAndHeight = CreatePlateauThenReturnWidthAndHeightValues();

            var boundaryCreationStrategy = DetermineBoundaryCreationStrategyAndReturnValue();

            var request = new CreatePlateauRequest
            {
                Width = widthAndHeight.FirstOrDefault(a=> a.Key.Equals("Width")).Value,
                Height = widthAndHeight.FirstOrDefault(a => a.Key.Equals("Height")).Value,
                BoundaryCreationStrategy = boundaryCreationStrategy
            };

            var response = _plateauService.Create(request);

            ResetColor();

            return response;

        }

        private static Enums.BoundaryCreationStrategy DetermineBoundaryCreationStrategyAndReturnValue()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Please select one of the options from menu below:");
            Console.WriteLine("1-Determine plateau' s boundaries on create.");
            Console.WriteLine("2-Learn plateau' s boundaries from failure.");

            var boundaryCreationStrategy = string.Empty;

            var boundaryCreationStrategyIsValid = false;

            while (!boundaryCreationStrategyIsValid)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                boundaryCreationStrategy = Console.ReadLine();

                boundaryCreationStrategyIsValid = CheckIfBoundaryCreationStrategyIsValid(boundaryCreationStrategy);

                if (boundaryCreationStrategyIsValid) continue;

                ShowErrorMessage("Please select only one of the options from menu above");

            }

            ResetColor();

            return (Enums.BoundaryCreationStrategy) int.Parse(boundaryCreationStrategy);
        }

        private static List<KeyValuePair<string, string>> GetRoverPositionFromUser()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            var result = new List<KeyValuePair<string, string>>();

            var roverCreationInputsAreValid = false;

            Console.WriteLine("Please enter the position for the rover. (e.g: 1 2 N)");

            var roverXPositionYPositionFacingTo = string.Empty;

            while (!roverCreationInputsAreValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                roverXPositionYPositionFacingTo = Console.ReadLine();

                roverCreationInputsAreValid = CheckIfBoundaryRoverCreationInputsAreValidValid(roverXPositionYPositionFacingTo);

                if (roverCreationInputsAreValid) continue;

                ShowErrorMessage("Please enter the values in appropriate format by seperating with <space> character(e.g: 1 2 N/S/W/E)");
            }

            var splittedValues = roverXPositionYPositionFacingTo.Split(' ');

            result.Add(new KeyValuePair<string, string>("XPosition", splittedValues[0]));
            result.Add(new KeyValuePair<string, string>("YPosition", splittedValues[1]));
            result.Add(new KeyValuePair<string, string>("FacingTo", splittedValues[2].ToUpper()));

            return result;
        }

        private static bool CheckIfBoundaryRoverCreationInputsAreValidValid(string roverXPositionYPositionFacingTo)
        {
            if (string.IsNullOrEmpty(roverXPositionYPositionFacingTo)) return false;

            var splittedValues = roverXPositionYPositionFacingTo.Split(' ');

            if (splittedValues.Length == 3)
            {
                var xPositionValue = splittedValues[0];
                var yPositionValue = splittedValues[1];
                var facingToValue = splittedValues[2];


                if (string.IsNullOrEmpty(xPositionValue) || string.IsNullOrEmpty(yPositionValue) || string.IsNullOrEmpty(facingToValue)) return false;

                int xPosition;
                int yPosition;
                int facingToInt;
                Enums.CompassDirections facingTo;

                if (int.TryParse(facingToValue, out facingToInt)) return false;

                return int.TryParse(xPositionValue, out xPosition) && int.TryParse(yPositionValue, out yPosition) && Enum.TryParse(facingToValue.ToUpper(), true, out facingTo);
            }

            return false;
        }

        private static List<KeyValuePair<string, int>> CreatePlateauThenReturnWidthAndHeightValues()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            var result = new List<KeyValuePair<string, int>>();

            var plateauCreationInputsAreValid = false;

            Console.WriteLine("Please enter the plateau' s width and height:");

            var plateauWidthAndHeight = string.Empty;

            while (!plateauCreationInputsAreValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                plateauWidthAndHeight = Console.ReadLine();

                plateauCreationInputsAreValid = CheckIfPlateauCreationInputsAreValid(plateauWidthAndHeight);

                if (plateauCreationInputsAreValid) continue;

                ShowErrorMessage("Please enter the values in appropriate format by seperating with <space> character(integerValue integerValue) e.g: 5 5");
            }

            ResetColor();

            var splittedValue = plateauWidthAndHeight.Split(' ');

            result.Add(new KeyValuePair<string, int>("Width", int.Parse(splittedValue[0])));
            result.Add(new KeyValuePair<string, int>("Height", int.Parse(splittedValue[1])));

            return result;
        }

        private static bool CheckIfBoundaryCreationStrategyIsValid(string boundaryCreationStrategy)
        {
            if (string.IsNullOrEmpty(boundaryCreationStrategy)) return false;

            int creationStrategy;

            var canBeParsedToInt = int.TryParse(boundaryCreationStrategy, out creationStrategy);

            if (!canBeParsedToInt) return false;

            var boundaryCreationStrategies = Enum.GetValues(typeof(Enums.BoundaryCreationStrategy))
                              .OfType<Enums.BoundaryCreationStrategy>()
                              .Select(s => (int)s);

            return boundaryCreationStrategies.Contains(creationStrategy);

        }

        private static bool CheckIfPlateauCreationInputsAreValid(string plateauWidthAndHeight)
        {
            if (string.IsNullOrEmpty(plateauWidthAndHeight)) return false;

            var splittedValue = plateauWidthAndHeight.Split(' ');

            if (splittedValue.Length == 2)
            {
                var widthValue = splittedValue[0];
                var heightValue = splittedValue[1];

                if (string.IsNullOrEmpty(widthValue) || string.IsNullOrEmpty(heightValue)) return false;

                int width;
                int height;

                return int.TryParse(widthValue, out width) && int.TryParse(heightValue, out height);
            }

            return false;
        }

        private struct PrivateMessage
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Message { get; set; }
            public Guid MsgId { get; set; }
        }
    }
}
