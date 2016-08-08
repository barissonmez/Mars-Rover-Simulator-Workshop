using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGKit.FantasyNameGenerator;
using RPGKit.FantasyNameGenerator.Generators;

namespace MarsRovers.Common
{
    public static class Helper
    {
        public static string GenerateName()
        {
            var settings = new FantasyNameSettings(Classes.Warrior, Race.None, true, true, Gender.Male);
            var generator = FantasyNameGenerator.FromSettingsInfo(settings);

            var name = generator.GetFantasyName();

            return name.FirstName;
        }

        public static Enums.CompassPoint ConvertToCompassPoint(Enums.CompassDirections compassDirections)
        {
            switch (compassDirections)
            {
                case Enums.CompassDirections.N:
                    {
                        return Enums.CompassPoint.North;
                    }
                case Enums.CompassDirections.S:
                    {
                        return Enums.CompassPoint.South;
                    }
                case Enums.CompassDirections.W:
                    {
                        return Enums.CompassPoint.West;
                    }
                case Enums.CompassDirections.E:
                    {
                        return Enums.CompassPoint.East;
                    }
            }

            return Enums.CompassPoint.North;
        }

        public static Enums.CompassDirections ConvertToCompassDirection(Enums.CompassPoint compassPoint)
        {
            switch (compassPoint)
            {
                case Enums.CompassPoint.North:
                    {
                        return Enums.CompassDirections.N;
                    }
                case Enums.CompassPoint.South:
                    {
                        return Enums.CompassDirections.S;
                    }
                case Enums.CompassPoint.West:
                    {
                        return Enums.CompassDirections.W;
                    }
                case Enums.CompassPoint.East:
                    {
                        return Enums.CompassDirections.E;
                    }
            }

            return Enums.CompassDirections.N;
        }

    }
}
