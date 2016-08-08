﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRovers.Common;

namespace MarsRovers.DTO
{
    public class CreateRoverRequest
    {

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public Enums.CompassPoint  FacingTo{ get; set; }

        public Guid PlateauId { get; set; }

    }
}
