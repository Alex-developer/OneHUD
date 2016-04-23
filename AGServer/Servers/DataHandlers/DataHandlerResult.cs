using System;
using System.Linq;

namespace AGServer.Servers.DataHandlers
{
    abstract class DataHandlerResult : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
