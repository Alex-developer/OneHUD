using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using AGServer.Servers.DataHandlers.Connected;
using AGServer.Servers.Services;
using AGData;
using WebSocketSharp;

namespace AGServer.Servers.HTTP.Services
{
    class ConnectedService : BaseService
    {
        public ConnectedService(TelemetryData telemetryData)
            : base(telemetryData)
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            NameValueCollection postData = HttpUtility.ParseQueryString(e.Data);
            ConnectedDataHandlerResult result = ConnectedDataHandler.ProcessConnectedRequest(Telemetry, postData);
            json = new JavaScriptSerializer().Serialize(result);
            Send(json);
            result.Dispose();
        }
    }
}
