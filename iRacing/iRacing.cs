using System;
using System.Globalization;
using OneHUDInterface;
using iRacingSdkWrapper;
using OneHUDData;
using OneHUDData.Sessions;
using OneHUDData.Sessions.Drivers;

namespace iRacing
{
    public class iRacing : GameBase,IGame
    {

        private readonly SdkWrapper _wrapper;

        private TelemetryData _telemetryData;
        private TimingData _timingData;
        private TelemetryInfo _ti;
        private SessionInfo _si;

        private int? _currentSessionNumber = null;
        private bool _forceSessionUpdate = true;

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="iRacing"/> class.
        /// </summary>
        public iRacing() : base()
        {
            _name = "iRacing";
            _displayName = "iRacing";
            _displayName = "iRacing";
            _author = "Alex Greenland";
            _url = "http://onehud.co.uk/";
            _processNames.Add("iRacingSim");
            _processNames.Add("iRacingSim64");
            _processNames.Add("iRacingSim64DX11");
            _processNames.Add("iRacingSimDX11");

            _wrapper = new SdkWrapper();
            _wrapper.TelemetryUpdated += OnTelemetryUpdated;
            _wrapper.SessionInfoUpdated += OnSessionInfoUpdated;
            _wrapper.Disconnected += OnSimDisconnected;
        }
        #endregion

        #region Getters and Setters



        #endregion

        #region Start Stop Methods        
        /// <summary>
        /// Starts the specified telemetry data.
        /// </summary>
        /// <param name="telemetryData">The telemetry data.</param>
        /// <returns>true !</returns>
        public override bool Start(TelemetryData telemetryData, TimingData timingData)
        {
            _telemetryData = telemetryData;
            _timingData = timingData;
            _wrapper.Start();
            return true;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns>true !</returns>
        public override bool Stop()
        {
            _wrapper.Stop();
            return true;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Called when [session information updated].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SdkWrapper.SessionInfoUpdatedEventArgs"/> instance containing the event data.</param>
        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            // We have no session so wait a bit
            if (_currentSessionNumber == null)
            {
                return;
            }

            // Cache the Session Info
            _si = e.SessionInfo;

            // Grab the Session Type
            _timingData.RaceInfo.SessionType = GetSessionType(_si["SessionInfo"]["Sessions"]["SessionNum",_currentSessionNumber]["SessionType"].GetValue());

            /*
             * If the current session data is empty then do an initial parse of the session data. Since this
             * data never changes between praccy / qualy / race there is no point parsing it more than once.
            */
            if (_timingData.RaceInfo.TrackName == null)
            {
                InitialSessionParse(_si);
                _forceSessionUpdate = false;
            }
            
            /*
             * If the session type has changed then force an update of the track conditions etc
            */
            if (_forceSessionUpdate)
            {
                ParseTrackStatus(_si);
            }

            UpdateDrivers(_si);
        }

        /// <summary>
        /// Called when [telemetry updated].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SdkWrapper.TelemetryUpdatedEventArgs"/> instance containing the event data.</param>
        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            _ti = e.TelemetryInfo;

            if (_currentSessionNumber == null || (_currentSessionNumber.Value != _ti.SessionNum.Value))
            {
                _forceSessionUpdate = true;
            }

            _currentSessionNumber = _ti.SessionNum.Value;

            if (_ti.IsInGarage.Value == false && _ti.IsOnTrack.Value == false)
            {
                _telemetryData.Car.InCar = false;
            }
            else
            {
                _telemetryData.Car.InCar = true;
            }
            _telemetryData.Engine.RPM = _ti.RPM.Value;
            _telemetryData.Car.Speed = ConvertSpeedToMPH(_ti.Speed.Value);
            _telemetryData.Car.Gear = _ti.Gear.Value;
            _telemetryData.Car.FuelRemaining = _ti.FuelLevel.Value;
            if (_ti.FuelLevelPct.Value != 0)
            {
                _telemetryData.Car.FuelCapacity = (100 / (_ti.FuelLevelPct.Value * 100)) * _ti.FuelLevel.Value;
            }
            else
            {
                _telemetryData.Car.FuelCapacity = 0;
            }

            _telemetryData.Timing.CurrentLapTime = _wrapper.GetTelemetryValue<float>("LapCurrentLapTime").Value;

            _telemetryData.Engine.WaterTemp = _ti.WaterTemp.Value;

            _timingData.RaceInfo.SessionTime = _ti.SessionTime.Value;
        }

        /// <summary>
        /// Called when [sim disconnected].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSimDisconnected(object sender, EventArgs e)
        {
        }
        #endregion

        #region Session Helper Functions        
        /// <summary>
        /// Initial session parse.
        /// </summary>
        /// <param name="si">The SessionInfo.</param>
        private void InitialSessionParse(SessionInfo si)
        {
            ParseWeekendInformation(si);
            ParseTrackStatus(si);
            ParseSectors(si);
            ParseAllDriverSessionData(si);
        }

        /// <summary>
        /// Parses the track information.
        /// </summary>
        /// <param name="si">The SessionInfo.</param>
        private void ParseWeekendInformation(SessionInfo si)
        {
            YamlQuery weekendInfo = si["WeekendInfo"];

            _timingData.RaceInfo.SessionId = ParseInt(weekendInfo["SessionID"].GetValue());
            _timingData.RaceInfo.SessionId = ParseInt(weekendInfo["SubSessionID"].GetValue());

            _timingData.RaceInfo.AmbientTemperature = ParseTemp(weekendInfo["TrackAirTemp"].GetValue());
            _timingData.RaceInfo.TrackTemperature = ParseTemp(weekendInfo["TrackSurfaceTemp"].GetValue());

            _timingData.RaceInfo.TrackName = weekendInfo["TrackName"].GetValue();
            _timingData.RaceInfo.TrackLongName = weekendInfo["TrackDisplayName"].GetValue();
            _timingData.RaceInfo.TrackShortName = weekendInfo["TrackDisplayShortName"].GetValue();
            _timingData.RaceInfo.TrackVariation = weekendInfo["TrackConfigName"].GetValue();

            string trackLengthString = weekendInfo["TrackLength"].GetValue();
            _timingData.RaceInfo.TrackLength = (int)(ParseTrackLength(trackLengthString) * 1000);
            var tt = 56;
        }

        /// <summary>
        /// Parses the sectors.
        /// </summary>
        /// <param name="si">The session info.</param>
        private void ParseSectors(SessionInfo si)
        {
            YamlQuery sectors = si["SplitTimeInfo"]["Sectors"];

            int sector = 0;
            while (sector >= 0)
            {
                string percentageStart = sectors["SectorNum", sector]["SectorStartPct"].GetValue();
                float startPoint;
                if (string.IsNullOrWhiteSpace(percentageStart) || !float.TryParse(percentageStart, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out startPoint))
                {
                    break;
                }

                float sectorStart = (startPoint / 100) * _timingData.RaceInfo.TrackLength;
                _timingData.RaceInfo.AddSector(sector, sectorStart);
                sector++;
            }
        }

        /// <summary>
        /// Parses the track status.
        /// </summary>
        /// <param name="si">The SessionInfo.</param>
        private void ParseTrackStatus(SessionInfo si)
        {

        }

        /// <summary>
        /// Parses all driver session data.
        /// </summary>
        /// <param name="si">The SessionInfo.</param>
        private void ParseAllDriverSessionData(SessionInfo si)
        {
            UpdateDrivers(si);

            for (int i = 0; i < 5; i++)
            {
                YamlQuery session = si["SessionInfo"]["Sessions"]["SessionNum", i];
                SessionType sessionType = GetSessionType(session["SessionType"].GetValue());

                string sessionLaps = session["SessionLaps"].GetValue();

                if (sessionLaps != null)
                {
                    for (int j = 0; j < 70; j++)
                    {
                        YamlQuery positionData = session["ResultsPositions"]["Position", j + 1];
                        string car = positionData["CarIdx"].GetValue();
                        if (car == null)
                        {
                            break;
                        }
                        int carIndex = ParseInt(car, 0);
                        Driver driver = _timingData.RaceInfo.Drivers.Find(d => d.Id == carIndex);
                        string fastestLap = positionData["FastestTime"].GetValue();
                        if (fastestLap != null)
                        {
                            driver.SetLapTime(sessionType, ParseFloat(fastestLap, 0), j + 1);
                        }
                    }
                }

            }
            var tt = 56;
        }

        /// <summary>
        /// Updates the drivers.
        /// </summary>
        /// <param name="si">The SessionInfo.</param>
        private void UpdateDrivers(SessionInfo si)
        {
            int paceCarPosition = ParseInt(si["DriverInfo"]["PaceCarIdx"].GetValue(), -99);

            for (int id = 0; id < 70; id++)
            {
                if (id != paceCarPosition)
                {
                    YamlQuery driverInfo = si["DriverInfo"]["Drivers"]["CarIdx", id];
                    string driverIdString = driverInfo["UserID"].GetValue();

                    if (driverIdString != null)
                    {

                        Driver driver = _timingData.RaceInfo.Drivers.Find(d => d.Id == id);
                        if (driver == null)
                        {
                            driver = new Driver();
                            _timingData.RaceInfo.Drivers.Add(driver);
                        }

                        if (driver.Id != id)
                        {
                            string name = driverInfo["TeamName"].GetValue();
                            int isSpectating = ParseInt(driverInfo["IsSpectator"].GetValue(), 0);
                            DriverType type = DriverType.Driver;
                            if (isSpectating == 1)
                            {
                                type = DriverType.Spectator;
                            }
                            driver.DriverType = type;
                            driver.Id = id;
                            driver.Name = name;
                        }

                        YamlQuery driverSessionInfo = si["SessionInfo"]["Sessions"]["SessionNum", _currentSessionNumber]["ResultsPositions"]["CarIdx", id];
                        string position = driverSessionInfo["Position"].GetValue();
                        if (position != null)
                        {
                            driver.Position = ParseInt(position, 0);
                        }
                        string lap = driverSessionInfo["LapsComplete"].GetValue();
                        if (lap != null)
                        {
                            driver.Lap = ParseInt(lap, -1);
                        }
                        string lapsDown = driverSessionInfo["Lap"].GetValue();
                        if (lapsDown != null)
                        {
                            driver.LapsDown = ParseInt(lapsDown, -1);
                        }
                        string deltaTime = driverSessionInfo["Time"].GetValue();
                        if (deltaTime != null)
                        {
                            driver.DeltaTime = ParseFloat(deltaTime, -1);
                        }
                        string fastestLap = driverSessionInfo["FastestLap"].GetValue();
                        if (fastestLap != null)
                        {
                            driver.FastestLap = ParseInt(fastestLap, -1);
                        }
                        string fastestLapTime = driverSessionInfo["FastestTime"].GetValue();
                        if (fastestLapTime != null)
                        {
                            driver.FastestLapTime = ParseFloat(fastestLapTime, 0);
                        }
                        string lastLapTime = driverSessionInfo["LastTime"].GetValue();
                        if (lastLapTime != null)
                        {
                            driver.LastLapTime = ParseFloat(lastLapTime, 0);
                        }
                        var gg = 56;
                        //_currentSessionNumber


                    }
                }
            }
        }
        #endregion

        #region Helper functions        
        /// <summary>
        /// Converts the speed to MPH.
        /// </summary>
        /// <param name="Speed">The speed.</param>
        /// <returns>The converted speed</returns>
        private float ConvertSpeedToMPH(float Speed)
        {
            return Speed * (float)2.236936;
        }

        /// <summary>
        /// Parses an integer from a string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        public int ParseInt(string value, int @default = 0)
        {
            int val;
            if (int.TryParse(value, out val)) return val;
            return @default;
        }

        public float ParseFloat(string value, float @default = 0f)
        {
            float val;
            if (float.TryParse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingWhite,
                CultureInfo.InvariantCulture, out val)) return val;
            return @default;
        }

        public SessionType GetSessionType(string sessionType)
        {
            SessionType type = SessionType.Invalid;

            switch (sessionType)
            {
                case "Invalid":
                    type = SessionType.Invalid;
                    break;

                case "Offline Testing":
                    type = SessionType.OfflinePractice;
                    break;

                case "Lone Practice":
                case "Practice":
                    type = SessionType.Practice;
                    break;

                case "Lone Qualify":
                case "Open Qualify":
                    type = SessionType.Qualifying;
                    break;

                case "Race":
                    type = SessionType.Race;
                    break;
            }

            return type;
        }

        public double ParseTrackLength(string value)
        {
            double length = 0;

            var indexOfKm = value.IndexOf("km");
            if (indexOfKm > 0) value = value.Substring(0, indexOfKm);

            if (double.TryParse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out length))
            {
                return length;
            }
            return 0;
        }

        public double ParseTemp(string value)
        {
            double temp = 0;

            var indexOfC = value.IndexOf(" C");
            if (indexOfC > 0) value = value.Substring(0, indexOfC);

            if (double.TryParse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out temp))
            {
                return temp;
            }
            return 0;
        }
        #endregion

    }
}
