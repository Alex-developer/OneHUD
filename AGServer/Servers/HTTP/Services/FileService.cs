using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using AGServer.Servers.DataHandlers.Actions;
using AGServer.Servers.Services;
using AGData;
using WebSocketSharp;

namespace AGServer.Servers.HTTP.Services
{
    class FileService : BaseService
    {
        public FileService(TelemetryData telemetryData)
            : base(telemetryData)
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            NameValueCollection postData = HttpUtility.ParseQueryString(e.Data);
            ActionsDataHandlerResult result = ActionsDataHandler.ProcessFileRequest(Telemetry, postData);
            json = new JavaScriptSerializer().Serialize(result);
            Send(json);
            result.Dispose();
        }
    }
}
