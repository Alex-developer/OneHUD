using System;
using System.Linq;

namespace OneHUDData.Sessions
{
    public class Lap
    {
        private int _lapNumber;
        private bool _valid;
        private float _lapTime;

        public float LapTime
        {
            get
            {
                return _lapTime;
            }
            set
            {
                _lapTime = value;
            }
        }

        public bool Valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
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

        public Lap(int lapNumber)
        {
            _lapNumber = lapNumber;
            _valid = true;
        }
    }
}
