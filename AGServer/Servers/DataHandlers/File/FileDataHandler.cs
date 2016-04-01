using System;
using System.Linq;
using System.Collections.Specialized;
using AGData;

namespace AGServer.Servers.DataHandlers.File
{
    static class FileDataHandler
    {

        public static FileDataHandlerResult ProcessFileRequest(TelemetryData telemetry, NameValueCollection postData)
        {
            if (postData.AllKeys.Contains("action"))
            {

            }    

            return new FileDataHandlerResult();
        }
    }
}
