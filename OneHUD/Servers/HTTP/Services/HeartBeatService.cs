using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using AGServer.Servers.DataHandlers.HeartBeat;
using AGServer.Servers.Services;
using OneHUDData;
using WebSocketSharp;

namespace AGServer.Servers.HTTP.Services
{
    class HeartBeatService : BaseService
    {
        public HeartBeatService(TelemetryData telemetryData)
            : base(telemetryData)
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            NameValueCollection postData = HttpUtility.ParseQueryString(e.Data);
            HeartBeatDataHandlerResult result = HeartBeatDataHandler.ProcessConnectedRequest(Telemetry, postData);
            json = new JavaScriptSerializer().Serialize(result);
            Send(json);
            result.Dispose();
        }

    }
}
