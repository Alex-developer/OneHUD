using System;
using System.Collections.Generic;
using System.Linq;

namespace OneHUDData.AnalysisData
{
    public class AnalysisLap
    {
        private List<AnalysisData> _data;
        private int _lapNumber;

        public AnalysisLap()
        {
            _data = new List<AnalysisData>();
        }

        public int LapNumber
        {
            get
            {
                return _lapNumber;
            }
            set
            {
                _lapNumber = value;
            }
        }

        public void AddData(float speed, float rpm) {
            AnalysisData dataPoint = new AnalysisData() { Speed = speed, Rpm = rpm };
            _data.Add(dataPoint);
        }
    }
}
