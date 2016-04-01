using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGServer.Servers.DataHandlers
{
    class DataHandlerError : DataHandlerResult
    {
        public string Status = "Error";

        public string Description { get; set; }
    }
}
