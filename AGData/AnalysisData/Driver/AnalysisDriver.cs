using System;
using System.Collections.Generic;
using System.Linq;

namespace OneHUDData.AnalysisData
{
    public class AnalysisDriver
    {
        private List<AnalysisLap> _laps;
        private int _id;

        public AnalysisDriver()
        {
            _laps = new List<AnalysisLap>();
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public AnalysisLap GetLap(int lapNumber)
        {
            AnalysisLap lap = _laps.Find(l => l.LapNumber == lapNumber);
            if (lap == null)
            {
                lap = new AnalysisLap() { LapNumber = lapNumber };
                _laps.Add(lap);
            }
            return lap;
        }


    }
}
