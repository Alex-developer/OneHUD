using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using AGServer.Servers.DataHandlers.Telemetry;
using AGServer.Servers.Services;
using OneHUDData;
using WebSocketSharp;

namespace AGServer.Servers.HTTP.Services
{
    class TelemetryService : BaseService
    {
        public TelemetryService(TelemetryData telemetryData) : base(telemetryData)
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            lock (Telemetry)
            {
                TelemetryDataHandlerResult result = new TelemetryDataHandlerResult() { Data = Telemetry };
                json = new JavaScriptSerializer().Serialize(result);
                Send(json);
                result.Dispose();
            }
        }
    }
}