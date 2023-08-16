using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinochle.Server.DataTransferObjects
{
    public class StatusDto
    {
        public int Games { get; set; }

        public int Lobbies { get; set; }
    }
}
