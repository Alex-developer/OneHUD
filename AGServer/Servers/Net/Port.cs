using System;

namespace AGServer.Servers.Net
{
    public class Port
    {
        public string name
        {
            get
            {
                return string.Format("{0} ({1} port {2})", this.process_name, this.protocol, this.port_number);
            }
            set
            {
            }
        }

        public int port_number { get; set; }

        public string process_name { get; set; }

        public string protocol { get; set; }
    }
}
