using System;
using System.Linq;
using System.Collections.Specialized;
using OneHUDData;

namespace AGServer.Servers.DataHandlers.HeartBeat
{
    class HeartBeatDataHandler
    {
        public static HeartBeatDataHandlerResult ProcessConnectedRequest(TelemetryData telemetry, NameValueCollection postData)
        {
            HeartBeatDataHandlerResult result = new HeartBeatDataHandlerResult() { Game = null };

            if (telemetry.Game != null)
            {
                result.Game = telemetry.Game;
            }

            return result;
        }
    }
}
