using System;
using System.Linq;

namespace AGServer.Servers.DataHandlers.Connected
{
    class ConnectedDataHandlerResult : DataHandlerResult
    {
        public bool Result { get; set; }
        public bool Connected { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
