using System;
using System.Linq;
using System.Collections.Generic;

namespace OneHUDData.TrackInfo
{
    public class Track
    {
        private List<TrackPoint> _trackPoints;
        private TrackBounds _trackBounds;
        private string _trackName;


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

        public string TrackName
        {
            get
            {
                return _trackName;
            }
            set
            {
                _trackName = value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        public Track()
        {
            _trackPoints = new List<TrackPoint>();
            _trackBounds = new TrackBounds();
        }
        #endregion


    }
}
