using System;
using System.Linq;
using WebSocketSharp.Server;
using OneHUDData;

namespace AGServer.Servers.Services
{
    public class BaseTimingService : WebSocketBehavior
    {
        protected TimingData Timing { get; set; }
        protected string json;

        public BaseTimingService(TimingData timingData)
        {
            Timing = timingData;
        }
    }
}
