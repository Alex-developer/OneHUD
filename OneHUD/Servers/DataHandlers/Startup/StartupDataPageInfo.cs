using System;
using System.Linq;

namespace AGServer.Servers.DataHandlers.Startup
{
    class StartupDataPageInfo
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Menuicon { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public int Order { get; set; }
    }
}
