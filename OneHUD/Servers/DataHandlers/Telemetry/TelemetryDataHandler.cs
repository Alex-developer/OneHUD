using System;
using System.Collections.Specialized;
using System.Linq;
using OneHUDData;

namespace AGServer.Servers.DataHandlers.Telemetry
{
    class TelemetryDataHandler
    {
        public static TelemetryDataHandlerResult ProcessConnectedRequest(TelemetryData telemetry, NameValueCollection postData)
        {
            TelemetryDataHandlerResult result;
            lock (telemetry)
            {
                result = new TelemetryDataHandlerResult() { Data = telemetry };
            }
            return result;
        }
    }
}
