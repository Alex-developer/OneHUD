using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using AGServer.Servers.Services;
using OneHUD.Servers.DataHandlers.Timing;
using OneHUDData;
using WebSocketSharp;

namespace AGServer.Servers.HTTP.Services
{
    class TimingService : BaseTimingService
    {
        public TimingService(TimingData timingData)
            : base(timingData)
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            NameValueCollection postData = HttpUtility.ParseQueryString(e.Data);
            TimingDataHandlerResult result = new TimingDataHandlerResult() { Data = Timing };
            json = new JavaScriptSerializer().Serialize(result);
            Send(json);
            result.Dispose();
        }
    }
}
