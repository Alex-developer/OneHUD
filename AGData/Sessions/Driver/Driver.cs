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
        private int _id = -1;
        private string _name;
        private DriverType _driverType;
        private int _position;
        private int _lapsDown;
        private int _fastestLap;
        private float _fastestLapTime;
        private float _lastLapTime;
        private float _deltaTime;
        private int _lap;

        private float _practiceFastestLap;
        private int _practicePosition;

        private float _qualifyFastestLap;
        private int _qualifyPosition;

        private float _raceFastestLap;
        private int _racePosition;

        private float _x;
        private float _y;
        private float _z;

        #region Constructor
        public Driver()
        {
            _driverCar = new DriverCar();
        }
        #endregion

        #region Getters and Setters
        #region Track coordinates
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

        #region Position and Timing
        public int RacePosition
        {
            get
            {
                return _racePosition;
            }
            set
            {
                _racePosition = value;
            }
        }

        public float RaceFastestLap
        {
            get
            {
                return _raceFastestLap;
            }
            set
            {
                _raceFastestLap = value;
            }
        }

        public int QualifyPosition
        {
            get
            {
                return _qualifyPosition;
            }
            set
            {
                _qualifyPosition = value;
            }
        }

        public float QualifyFastestLap
        {
            get
            {
                return _qualifyFastestLap;
            }
            set
            {
                _qualifyFastestLap = value;
            }
        }

        public int PracticePosition
        {
            get
            {
                return _practicePosition;
            }
            set
            {
                _practicePosition = value;
            }
        }

        public float PracticeFastestLap
        {
            get
            {
                return _practiceFastestLap;
            }
            set
            {
                _practiceFastestLap = value;
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

        #region Timing Methods        
        /// <summary>
        /// Sets the lap time for a specified session
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="time">The time.</param>
        /// <param name="position">The position.</param>
        public void SetLapTime(SessionType type, float time, int position)
        {
            switch (type)
            {
                case SessionType.Practice:
                    _practiceFastestLap = time;
                    _practicePosition = position;
                    break;

                case SessionType.Qualifying:
                    _qualifyFastestLap = time;
                    _qualifyPosition = position;
                    break;

                case SessionType.Race:
                    _raceFastestLap = time;
                    _racePosition = position;
                    break;
            }
        }
        #endregion

    }

    public enum DriverType
    {
        Driver,
        Spectator
    }
}
