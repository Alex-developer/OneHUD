using System;
using System.Linq;
using System.Collections.Generic;
using OneHUDInterface.TrackInfo;

namespace OneHUDInterface.TrackRecorder
{
    public class TrackRecording
    {
        private int _lastLap = -99;
        private List<TrackLap> _trackLaps;
        private TrackBounds _trackBounds;

        #region Constructor
        public TrackRecording()
        {
            _trackBounds = new TrackBounds();
            _trackLaps = new List<TrackLap>();
        }
        #endregion

        #region Getters and Setters
        public TrackBounds TrackBounds
        {
            get
            {
                return _trackBounds;
            }
            set
            {
                _trackBounds = value;
            }
        }

        public List<TrackLap> TrackLaps
        {
            get
            {
                return this._trackLaps;
            }
            set
            {
                this._trackLaps = value;
            }
        }

        #endregion

        #region public methods
        #region Add new trackpoint
        public void AddPoint(int lap, float x, float y, float z)
        {
            if (lap != _lastLap) {
                _trackLaps.Add(new TrackLap() { Lap = lap});
                _lastLap = lap;
            }

            TrackLap currentLap = _trackLaps.Last();
            TrackPoint trackPoint = new TrackPoint() { GameX = x, GameY = y, GameZ = z };
            currentLap.AddPoint(trackPoint);
            _trackBounds.Update(trackPoint);
        }

        #endregion

        #region Lap Info
        public int GetTotalLaps()
        {
            return _trackLaps.Count;
        }

        public TrackLap GetLap(int lap)
        {
            TrackLap trackLap = _trackLaps.Find(x => x.Lap == lap);

            return trackLap;
        }
        #endregion
        #endregion

    }
}
