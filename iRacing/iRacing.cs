using System;

using AGServerInterface;
using iRacingSdkWrapper;
using AGData;

namespace iRacing
{
    public class iRacing : GameBase,IGame
    {

        private readonly SdkWrapper _wrapper;

        private TelemetryData _telemetryData;

        #region Constructor
        public iRacing() : base()
        {
            _name = "iRacing";
            _displayName = "iRacing";
            _displayName = "iRacing";
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
        public override bool Start(TelemetryData telemetryData)
        {
            _telemetryData = telemetryData;
            _wrapper.Start();
            return true;
        }

        public override bool Stop()
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
            _telemetryData.Car.Speed = ConvertSpeedToMPH(ti.Speed.Value);
            _telemetryData.Car.Gear = ti.Gear.Value;
            _telemetryData.Car.FuelRemaining = ti.FuelLevel.Value;
            if (ti.FuelLevelPct.Value != 0)
            {
                _telemetryData.Car.FuelCapacity = (100 / (ti.FuelLevelPct.Value * 100)) * ti.FuelLevel.Value;
            }
            else
            {
                _telemetryData.Car.FuelCapacity = 0;
            }

        }

        private void OnSimDisconnected(object sender, EventArgs e)
        {
        }
        #endregion

        #region Helper functions
        private float ConvertSpeedToMPH(float Speed)
        {
            return Speed * (float)2.236936;
        }
        #endregion

    }
}
