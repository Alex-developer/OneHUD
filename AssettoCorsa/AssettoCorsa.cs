using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using AGServerInterface;
using AssettoCorsa.Readers;
using AssettoCorsa.DataFormat;
using AGData;

namespace AssettoCorsa
{
    public class AssettoCorsa : GameBase, IGame
    {
        private TelemetryData _telemetryData;
        private readonly CancellationTokenSource _cancel;
        private DateTime _lastTimeStamp;

        private string _physicsFileName = "Local\\acpmf_physics";
        private readonly SharedMemoryReader _physicsMemoryReader;
        private readonly int _physicsBufferSize = Marshal.SizeOf(typeof(Physics));
        private Physics _physicsData;
        private readonly double _physicsPollInterval = 10.0;


        #region Constructor
        public AssettoCorsa() : base()
        {
            _name = "AC";
            _displayName = "Assetto Corsa";
            _processNames.Add("acs");

            _physicsMemoryReader = new SharedMemoryReader(_physicsFileName, _physicsBufferSize);
            _cancel = new CancellationTokenSource();
        }
        #endregion

        #region Public Methods
        public override bool Start(TelemetryData telemetryData)
        {
            _telemetryData = telemetryData;
            ReadData(_cancel.Token);
            return true;
        }

        public override bool Stop()
        {
            _cancel.Cancel();
            return true;
        }
        #endregion

        #region Shared Memory Data Reader
        private async void ReadData(CancellationToken token)
        {
            _physicsMemoryReader.Connect();

            await Task.Factory.StartNew((Action)(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    DateTime utcNow = DateTime.UtcNow;
                    if ((utcNow - _lastTimeStamp).TotalMilliseconds >= _physicsPollInterval)
                    {
                        _lastTimeStamp = utcNow;
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
        #endregion

        #region Process Shared memory data
        private void ProcessPhysicsData()
        {
            _telemetryData.Engine.RPM = _physicsData.Rpms;
            _telemetryData.Car.Speed = ConvertSpeedToMPH(_physicsData.SpeedKmh);
            _telemetryData.Car.Gear = _physicsData.Gear;
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
