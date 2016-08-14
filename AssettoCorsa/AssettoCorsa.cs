using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using OneHUDInterface;
using AssettoCorsa.Readers;
using AssettoCorsa.DataFormat;
using OneHUDData;

namespace AssettoCorsa
{
    public class AssettoCorsa : GameBase, IGame
    {
        private TelemetryData _telemetryData;
        private TimingData _timingData;
        private readonly CancellationTokenSource _physicsCancel;
        private readonly CancellationTokenSource _staticCancel; 
        private DateTime _physicsLastTimeStamp;
        private DateTime _staticLastTimeStamp;

        private string _physicsFileName = "Local\\acpmf_physics";
        private readonly SharedMemoryReader _physicsMemoryReader;
        private readonly int _physicsBufferSize = Marshal.SizeOf(typeof(Physics));
        private Physics _physicsData;
        private readonly double _physicsPollInterval = 10.0;
        private bool _connected = false;

        private string _staticFileName = "Local\\acpmf_static";
        private readonly SharedMemoryReader _staticMemoryReader;
        private readonly int _staticBufferSize = Marshal.SizeOf(typeof(StaticInfo));
        private StaticInfo _staticData;
        private readonly double _staticPollInterval = 1000.0;
        private bool _staticConnected = false;

        #region Constructor
        public AssettoCorsa() : base()
        {
            _name = "AC";
            _displayName = "Assetto Corsa";
            _author = "Alex Greenland";

            _processNames.Add("acs");

            _physicsMemoryReader = new SharedMemoryReader(_physicsFileName, _physicsBufferSize);
            _physicsCancel = new CancellationTokenSource();
            _staticMemoryReader = new SharedMemoryReader(_staticFileName, _staticBufferSize);
            _staticCancel = new CancellationTokenSource();
        }
        #endregion

        #region Public Methods
        public override bool Start(TelemetryData telemetryData, TimingData timingData)
        {
            _telemetryData = telemetryData;
            _timingData = timingData;
            ReadPhysicsData(_physicsCancel.Token);
            ReadStaticData(_staticCancel.Token);
            return true;
        }

        public override bool Stop()
        {
            _physicsCancel.Cancel();
            return true;
        }
        #endregion

        #region Shared Memory Data Reader
        private async void ReadPhysicsData(CancellationToken token)
        {
            await Task.Factory.StartNew((Action)(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (!_connected)
                    {
                        _connected = _physicsMemoryReader.Connect();
                    }
                    DateTime utcNow = DateTime.UtcNow;
                    if ((utcNow - _physicsLastTimeStamp).TotalMilliseconds >= _physicsPollInterval)
                    {
                        _physicsLastTimeStamp = utcNow;
                        byte[] buffer = _physicsMemoryReader.Read();

                        GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        try
                        {
                            _physicsData = (Physics)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(Physics));
                            ProcessPhysicsData();
                        }
                        finally
                        {
                            gCHandle.Free();
                        }

                    }
                    Thread.Sleep((int)_physicsPollInterval);
                }
            }));
        }

        private async void ReadStaticData(CancellationToken token)
        {
            await Task.Factory.StartNew((Action)(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (!_staticConnected)
                    {
                        _staticConnected = _staticMemoryReader.Connect();
                    }
                    DateTime utcNow = DateTime.UtcNow;
                    if ((utcNow - _staticLastTimeStamp).TotalMilliseconds >= _staticPollInterval)
                    {
                        _staticLastTimeStamp = utcNow;
                        byte[] buffer = _staticMemoryReader.Read();

                        GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        try
                        {
                            _staticData = (StaticInfo)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(StaticInfo));
                            ProcessStaticData();
                        }
                        finally
                        {
                            gCHandle.Free();
                        }

                    }
                    Thread.Sleep((int)_staticPollInterval);
                }
            }));
        }
        #endregion

        #region Process Shared memory data
        private void ProcessPhysicsData()
        {
            _telemetryData.Engine.RPM = _physicsData.Rpms;
            _telemetryData.Car.Speed = ConvertSpeedToMPH(_physicsData.SpeedKmh);
            _telemetryData.Car.Gear = _physicsData.Gear - 1;
            _telemetryData.Car.FuelRemaining =  _physicsData.Fuel;
            _telemetryData.Car.FuelCapacity = 100; // TODO: Fix this when reading static data

            _telemetryData.Timing.CurrentLapTime = 0; // TODO: Fix when Graphics data is available

            _telemetryData.Engine.WaterTemp = 0; // NOT SUPPORTED
        }

        private void ProcessStaticData()
        {
            _telemetryData.Car.FuelCapacity = _staticData.MaxFuel;
            _timingData.RaceInfo.TrackLongName = _staticData.Track;
            _timingData.RaceInfo.TrackShortName = _staticData.Track;
            _timingData.RaceInfo.TrackName = _staticData.Track;
            _timingData.RaceInfo.TrackVariation = "";

            _timingData.RaceInfo.TrackTemperature = 0; // Not supported
            _timingData.RaceInfo.AmbientTemperature = 0; // Not Supported
        }
        #endregion

        #region Helper functions
        private float ConvertSpeedToMPH(float Speed)
        {
            return Speed * (float)0.621371;
        }
        #endregion
    }
}
