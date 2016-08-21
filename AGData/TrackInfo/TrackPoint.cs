using System;
using System.Linq;

namespace OneHUDData.TrackInfo
{
    public class TrackPoint
    {
        private float _gameX;
        private float _gameY;
        private float _gameZ;

        private float _x;
        private float _y;
        private float _z;

        #region getters and setters
        #region screen coordinates
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public float Z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
            }
        }
        #endregion

        #region game coordinates
        public float GameZ
        {
            get
            {
                return _gameZ;
            }
            set
            {
                _gameZ = value;
            }
        }

        public float GameY
        {
            get
            {
                return _gameY;
            }
            set
            {
                _gameY = value;
            }
        }

        public float GameX
        {
            get
            {
                return _gameX;
            }
            set
            {
                _gameX = value;
            }
        }
        #endregion
        #endregion
    }
}
