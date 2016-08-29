using System;
using System.Collections.Generic;
using System.Linq;
using OneHUDData.TrackInfo;

namespace OneHUDData.TrackRecorder
{
    public class TrackDriver
    {
        private List<TrackLap> _trackLaps;
        private int _lastLap = -99;
        private int _id;

        #region Constructor
        public TrackDriver()
        {
            _trackLaps = new List<TrackLap>();
        }
        #endregion

        #region Getters and Setters
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

        public int LastLap
        {
            get
            {
                return _lastLap;
            }
            set
            {
                _lastLap = value;
            }
        }

        public List<TrackLap> TrackLaps
        {
            get
            {
                return _trackLaps;
            }
            set
            {
                _trackLaps = value;
            }
        }
        #endregion

        #region public methods
        #region Add new trackpoint
        public TrackPoint AddPoint(int lap, float x, float y, float z)
        {
            if (lap != _lastLap)
            {
                _trackLaps.Add(new TrackLap() { Lap = lap });
                _lastLap = lap;
            }

            TrackLap currentLap = _trackLaps.Last();
            TrackPoint trackPoint = new TrackPoint() { GameX = x, GameY = y, GameZ = z };
            currentLap.AddPoint(trackPoint);

            return trackPoint;
        }
        #endregion

        #region Get a Lap
        public TrackLap Lap(int lap)
        {
            return _trackLaps[lap];
        }
        #endregion
        #endregion
    }
}
