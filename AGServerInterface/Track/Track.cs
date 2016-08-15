using System;
using System.Linq;
using System.Collections.Generic;

namespace OneHUDInterface.Track
{
    class Track
    {
        private List<TrackPoint> _trackPoints;

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        public Track()
        {
            _trackPoints = new List<TrackPoint>();
        }
        #endregion
    }
}
