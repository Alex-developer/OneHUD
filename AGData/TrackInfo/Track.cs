using System;
using System.Linq;
using System.Collections.Generic;

namespace OneHUDInterface.TrackInfo
{
    public class Track
    {
        private List<TrackPoint> _trackPoints;
        private TrackBounds _trackBounds;

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
