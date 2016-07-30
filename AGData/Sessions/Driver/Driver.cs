using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHUDData.Sessions.Drivers
{
    public class Driver
    {
        private DriverCar _driverCar;
        private string id;
        private string _name;
        private DriverType _driverType;
        private int _position;
        private int _lapsDown;
        private int _fastestLap;
        private float _fastestLapTime;
        private float _lastLapTime;
        private float _deltaTime;
        private int _lap;

        #region Constructor
        public Driver()
        {
            _driverCar = new DriverCar();
        }
        #endregion

        #region Getters and Setters
        #region Position and Timing
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

        public float DeltaTime
        {
            get
            {
                return _deltaTime;
            }
            set
            {
                _deltaTime = value;
            }
        }

        public int LapsDown
        {
            get
            {
                return _lapsDown;
            }
            set
            {
                _lapsDown = value;
            }
        }

        public float LastLapTime
        {
            get
            {
                return _lastLapTime;
            }
            set
            {
                _lastLapTime = value;
            }
        }

        public float FastestLapTime
        {
            get
            {
                return _fastestLapTime;
            }
            set
            {
                _fastestLapTime = value;
            }
        }

        public int FastestLap
        {
            get
            {
                return _fastestLap;
            }
            set
            {
                _fastestLap = value;
            }
        }

        public int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        #endregion

        #region Driver Info
        public DriverType DriverType
        {
            get
            {
                return _driverType;
            }
            set
            {
                _driverType = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        #endregion

        #region Car
        public DriverCar Car
        {
            get
            {
                return _driverCar;
            }
            set
            {
                _driverCar = value;
            }
        }
        #endregion
        #endregion
    }

    public enum DriverType
    {
        Driver,
        Spectator
    }
}
