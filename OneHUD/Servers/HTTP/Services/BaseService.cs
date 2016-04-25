using System;
using System.Linq;
using WebSocketSharp.Server;
using OneHUDData;

namespace AGServer.Servers.Services
{
    public class BaseService : WebSocketBehavior
    {
        protected TelemetryData Telemetry { get; set; }
        protected string json;

        public BaseService(TelemetryData telemetryData)
        {
            Telemetry = telemetryData;
        }
    }
}
