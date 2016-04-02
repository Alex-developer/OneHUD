using System;
using System.Linq;
using System.Collections.Specialized;
using AGData;

namespace AGServer.Servers.DataHandlers.Actions
{
    static class ActionsDataHandler
    {

        public static ActionsDataHandlerResult ProcessFileRequest(TelemetryData telemetry, NameValueCollection postData)
        {
            if (postData.AllKeys.Contains("action"))
            {

            }

            return new ActionsDataHandlerResult();
        }
    }
}
