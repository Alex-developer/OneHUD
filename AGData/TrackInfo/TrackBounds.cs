using System;
using System.Linq;

namespace OneHUDData.TrackInfo
{
    public class TrackBounds
    {
        private float _minGameX;
        private float _maxGameX;
        private float _minGameY;
        private float _maxGameY;
        private float _minGameZ;
        private float _maxGameZ;

        private float _width;
        private float _height;

        #region getters and Setters
        public float MinGameX
        {
            get
            {
                return _minGameX;
            }
            set
            {
                _minGameX = value;
            }
        }

        public float MaxGameX
        {
            get
            {
                return _maxGameX;
            }
            set
            {
                _maxGameX = value;
            }
        }

        public float MinGameY
        {
            get
            {
                return _minGameY;
            }
            set
            {
                _minGameY = value;
            }
        }

        public float MaxGameY
        {
            get
            {
                return _maxGameY;
            }
            set
            {
                _maxGameY = value;
            }
        }

        public float MinGameZ
        {
            get
            {
                return _minGameZ;
            }
            set
            {
                _minGameZ = value;
            }
        }

        public float MaxGameZ
        {
            get
            {
                return _maxGameZ;
            }
            set
            {
                _maxGameZ = value;
            }
        }

        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        #endregion

        #region public methods
        public void Update(TrackPoint trackPoint)
        {
            if (trackPoint.GameX < MinGameX)
            {
                MinGameX = trackPoint.GameX;
            }

            if (trackPoint.GameY < MinGameY)
            {
                MinGameY = trackPoint.GameY;
            }

            if (trackPoint.GameZ < MinGameZ)
            {
                MinGameZ = trackPoint.GameZ;
            }

            if (trackPoint.GameX > MaxGameX)
            {
                MaxGameX = trackPoint.GameX;
            }

            if (trackPoint.GameY > MaxGameY)
            {
                MaxGameY = trackPoint.GameY;
            }

            if (trackPoint.GameZ > MaxGameZ)
            {
                MaxGameZ = trackPoint.GameZ;
            }
        }
        #endregion

    }
}
