using System;
using System.Linq;
using System.Collections.Specialized;
using AGData;

namespace AGServer.Servers.DataHandlers.Connected
{
    class ConnectedDataHandler
    {
        public static ConnectedDataHandlerResult ProcessConnectedRequest(TelemetryData telemetry, NameValueCollection postData)
        {
            ConnectedDataHandlerResult result = new ConnectedDataHandlerResult() { Result = false };

            if (telemetry.Game != null)
            {
                result.Connected = true;
                result.Name = telemetry.Game;
                result.Description = telemetry.Description;
            }

            return result;
        }
    }
}

