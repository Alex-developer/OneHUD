using System;
using System.Collections.Generic;
using System.Linq;
using OneHUDData.Sessions;

namespace OneHUDData
{
    public class TimingData
    {
        public RaceInfo RaceInfo { get; set; }

        public TimingData()
        {
            Reset();
        }

        public void Reset()
        {
            RaceInfo = new RaceInfo();
        }

    }

    public enum SessionType {
        Invalid = 0,
        OfflinePractice,
        Practice,
        Qualifying,
        Race
    }
}
