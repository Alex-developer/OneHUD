using System;
using System.Linq;
using AGData.Vehicle;

namespace AGData
{
    public class TelemetryData
    {
        public Engine Engine { get; set; }
        public Chassis Chassis { get; set; }
        public Car Car { get; set; }

        public TelemetryData()
        {
            Reset();
        }

        private void Reset()
        {
            Engine = new Engine();
            Chassis = new Chassis();
            Car = new Car();
        }
    }
}
