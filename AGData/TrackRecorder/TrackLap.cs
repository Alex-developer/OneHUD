using System;
using System.Collections.Generic;
using System.Linq;
using OneHUDData.TrackInfo;

namespace OneHUDData.TrackRecorder
{
    public class TrackLap
    {
        private int _lap;
        private List<TrackPoint> _trackPoints;

        #region Constructor
        public TrackLap()
        {
            _trackPoints = new List<TrackPoint>();
        }
        #endregion

        #region Getters and Setters
        public List<TrackPoint> TrackPoints
        {
            get
            {
                return _trackPoints;
            }
            set
            {
                _trackPoints = value;
            }
        }

        public int Lap
        {
            get
            {
                return _lap;
            }
            set
            {
                _lap = value;
            }
        }

        #endregion

        #region Methods
        public void AddPoint(TrackPoint point) {
            _trackPoints.Add(point);
        }
        #endregion
    }
}
