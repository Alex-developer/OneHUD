using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using AGServer.Servers.DataHandlers.Telemetry;
using AGServer.Servers.Services;
using AGData;
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
            NameValueCollection postData = HttpUtility.ParseQueryString(e.Data);
            TelemetryDataHandlerResult result = new TelemetryDataHandlerResult() { Data = Telemetry };
            json = new JavaScriptSerializer().Serialize(result);
            Send(json);
            result.Dispose();
        }
    }
}