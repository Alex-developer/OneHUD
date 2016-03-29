using System;
using System.Linq;
using AGData.Vehicle;

namespace AGData
{
    public class TelemetryData
    {
        public string Game { get; set; }
        public Engine Engine { get; set; }
        public Chassis Chassis { get; set; }
        public Car Car { get; set; }

        private readonly Object _lock = new Object();

        public TelemetryData()
        {
            Reset();
        }

        public void Reset()
        {
            lock (_lock)
            {
                Game = null;
                Engine = new Engine();
                Chassis = new Chassis();
                Car = new Car();
            }
        }
    }
}
