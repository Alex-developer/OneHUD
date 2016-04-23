using System;
using System.Linq;
using AGData;

namespace AGServer.Servers.DataHandlers.Telemetry
{
    class TelemetryDataHandlerResult : DataHandlerResult
    {
        public TelemetryData Data { get; set; }
    }
}
