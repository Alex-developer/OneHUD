using System;
using System.Linq;
using AGServer.Servers.Services;
using WebSocketSharp;
using OneHUDData;

namespace AGServer.Servers.HTTP.Services
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
