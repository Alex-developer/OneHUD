using System;
using System.Collections.Generic;
using System.Linq;

namespace OneHUDData.AnalysisData
{
    public class AnalysisManager
    {
        private List<AnalysisDriver> _drivers;

        public AnalysisManager()
        {
            _drivers = new List<AnalysisDriver>();
        }

        private AnalysisDriver GetDriver(int driverNumber)
        {
            AnalysisDriver driver = _drivers.Find(d => d.Id == driverNumber);
            if (driver == null)
            {
                driver = new AnalysisDriver() { Id = driverNumber };
                _drivers.Add(driver);
            }
            return driver;
        }

        public void AddDataPoint(int driverNumber, int lapNumber, float speed, float rpm)
        {
            AnalysisDriver driver = GetDriver(driverNumber);
            AnalysisLap lap = driver.GetLap(lapNumber);

            lap.AddData(speed, rpm);
        }
    }
}
