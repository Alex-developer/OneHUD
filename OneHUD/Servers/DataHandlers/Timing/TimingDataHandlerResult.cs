using System;
using System.Linq;
using AGServer.Servers.DataHandlers;
using OneHUDData;

namespace OneHUD.Servers.DataHandlers.Timing
{
    class TimingDataHandlerResult : DataHandlerResult
    {
        public TimingData Data { get; set; }
    }
}
