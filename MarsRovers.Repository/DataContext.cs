using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Entity;

namespace MarsRovers.Repository
{
    public static class DataContext
    {
        static DataContext()
        {
            Plateaus = new List<Plateau>();
            Rovers = new List<Rover>();
        }

        public static List<Plateau> Plateaus { get; set; }

        public static List<Rover> Rovers { get; set; }
    }
}
