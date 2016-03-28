using System;
using AGServerInterface;
using iRacingSdkWrapper;
using AGData;

namespace IRacing
{
    public class AGIRacing : IGame
    {

        public event EventHandler GameEvent;

        private readonly string _name = "iRacing";
        private readonly string _displayName = "iRacing";
        private readonly string[] _processNames = {"iRacingSim", "iRacingSim64", "iRacingSim64DX11" , "iRacingSimDX11"};

        private readonly SdkWrapper _wrapper;

        private TelemetryData _telemetryData;

        #region Constructor
        public AGIRacing()
        {
            _wrapper = new SdkWrapper();
            _wrapper.TelemetryUpdated += OnTelemetryUpdated;
            _wrapper.SessionInfoUpdated += OnSessionInfoUpdated;
            _wrapper.Disconnected += OnSimDisconnected;
        }
        #endregion

        #region Getters and Setters
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public string[] ProcessNames
        {
            get {
                return _processNames;
            }
        }
        #endregion

        #region Start Stop Methods
        public bool Start(TelemetryData telemetryData)
        {
            _telemetryData = telemetryData;
            _wrapper.Start();
            return true;
        }

        public bool Stop()
        {
            _wrapper.Stop();
            return true;
        }
        #endregion

        #region Event Handlers
        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            TelemetryInfo ti = e.TelemetryInfo;

            if (ti.IsInGarage.Value == false && ti.IsOnTrack.Value == false)
            {
                _telemetryData.Car.InCar = false;
            }
            else
            {
                _telemetryData.Car.InCar = true;
            }
            _telemetryData.Engine.RPM = ti.RPM.Value;
        }

        private void OnSimDisconnected(object sender, EventArgs e)
        {
        }
        #endregion

        public void RaiseBoo()
        {
            if (GameEvent != null)
                GameEvent(this, EventArgs.Empty);
        }

    }
}
