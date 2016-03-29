using System;
using System.Linq;
using WebSocketSharp;
using AGData;

namespace AGServer.Servers.Services
{
    class TestService : BaseService
    {
        public TestService(TelemetryData telemetryData)
            : base(telemetryData)
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Send("Hello World");
        }
    }
}
