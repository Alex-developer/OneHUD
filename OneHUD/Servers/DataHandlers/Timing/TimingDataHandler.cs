using System;
using System.Collections.Specialized;
using System.Linq;
using OneHUDData;

namespace OneHUD.Servers.DataHandlers.Timing
{
    class TimingDataHandler
    {
        public static TimingDataHandlerResult ProcessConnectedRequest(TimingData timingData, NameValueCollection postData)
        {
            TimingDataHandlerResult result = new TimingDataHandlerResult() { Data = timingData };
            return result;
        }
    }
}
