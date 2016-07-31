using System;
using System.Linq;
using System.Collections.Generic;
using OneHUDData.Sessions.Drivers;

namespace OneHUDData.Sessions
{
    public class RaceInfo
    {
        private List<Sector> _sectors;
 
        private string _trackName;
        private string _trackLongName;
        private string _trackShortName;
        private string _trackVariation;
        private int _trackLength;

        private int _sessionId; // iRacing only
        private int _subSessionId; // iRacing only

        private double _ambientTemperature;
        private double _trackTemperature;

        private List<Driver> _drivers;

        private double _sessionTime;

        #region Getters/Setters
        #region Drivers
        public List<Driver> Drivers
        {
            get
            {
                return _drivers;
            }
        }

        #endregion

        #region Weather Info Getters/Setters
        public double TrackTemperature
        {
            get
            {
                return _trackTemperature;
            }
            set
            {
                _trackTemperature = value;
            }
        }

        public double AmbientTemperature
        {
            get
            {
                return _ambientTemperature;
            }
            set
            {
                _ambientTemperature = value;
            }
        }
        #endregion

        #region Session Getters/Setters
        public double SessionTime
        {
            get
            {
                return _sessionTime;
            }
            set
            {
                _sessionTime = value;
            }
        }
        public int SubSessionId
        {
            get
            {
                return _subSessionId;
            }
            set
            {
                _subSessionId = value;
            }
        }

        public int SessionId
        {
            get
            {
                return _sessionId;
            }
            set
            {
                _sessionId = value;
            }
        }
        #endregion

        #region Track Info Getters/Setters
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

        public string TrackLongName
        {
            get
            {
                return _trackLongName;
            }
            set
            {
                _trackLongName = value;
            }
        }

        public string TrackShortName
        {
            get
            {
                return _trackShortName;
            }
            set
            {
                _trackShortName = value;
            }
        }

        public string TrackVariation
        {
            get
            {
                return _trackVariation;
            }
            set
            {
                _trackVariation = value;
            }
        }

        public int TrackLength
        {
            get
            {
                return _trackLength;
            }
            set
            {
                _trackLength = value;
            }
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RaceInfo"/> class.
        /// </summary>
        public RaceInfo()
        {
            _drivers = new List<Driver>();
            ResetSectors();
        }
        #endregion

        #region Sector Methods
        private void ResetSectors()
        {
            _sectors = new List<Sector>();
        }

        /// <summary>
        /// Adds the sector.
        /// </summary>
        /// <param name="sectorNumber">The sector number.</param>
        /// <param name="sectorStart">The sector start point in meteres.</param>
        /// <returns></returns>
        public Sector AddSector(int sectorNumber, float sectorStart)
        {
            Sector sector = new Sector() { SectorNumber = sectorNumber, SectorStart = sectorStart };
            _sectors.Add(sector);

            return sector;
        }

        public void SetupTwoSectors()
        {
            SetupNumberOfSectors(2);
        }

        public void SetupThreeSectors()
        {
            SetupNumberOfSectors(3);
        }

        public void SetupFourSectors()
        {
            SetupNumberOfSectors(4);
        }

        public void SetupFiveSectors()
        {
            SetupNumberOfSectors(5);
        }
        private void SetupNumberOfSectors(int numberOfSectors)
        {
            ResetSectors();

            for (int i = 0; i < numberOfSectors; i++)
            {
                AddSector(i + 1, 0);
            }
        }
        #endregion

    }
}
