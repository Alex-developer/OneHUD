using System;
using System.Collections.Generic;
using System.Linq;

namespace AGServer.Servers.DataHandlers.Startup
{
    class StartupDataHandlerResult : DataHandlerResult
    {
        public bool Result { get; set; }
        public string Version { get; set; }
        public List<SartupDataHandlerPlugin> Plugins { get; set; }
        public List<StartupDataPageInfo> Pages { get; set; }
        public List<StartupDataWidgetInfo> Widgets { get; set; }
    }
}
