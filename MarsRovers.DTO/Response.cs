using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.DTO
{
    public abstract class Response
    {
        protected Response()
        {
            Success = true;
            Messages = new List<string>();
        }

        public bool Success { get; set; }

        public List<string> Messages { get; set; }
    }
}
