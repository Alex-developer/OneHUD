using System;
using System.Linq;
using OneHUDData;

namespace AGServer.Servers.DataHandlers.Telemetry
{
    class TelemetryDataHandlerResult : DataHandlerResult
    {
        public TelemetryData Data { get; set; }
    }
}
