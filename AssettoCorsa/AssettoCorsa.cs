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
    public class AssettoCorsa : IGame
    {

        public event EventHandler GameEvent;

        private readonly string _name = "AC";
        private readonly string _displayName = "Assetto Corsa";
        private readonly string[] _processNames = { "acs" };

        private TelemetryData _telemetryData;
        private readonly CancellationTokenSource _cancel;
        private DateTime _lastTimeStamp;

        private string _physicsFileName = "Local\\acpmf_physics";
        private readonly SharedMemoryReader _physicsMemoryReader;
        private readonly int _physicsBufferSize = Marshal.SizeOf(typeof(Physics));
        private Physics _physicsData;
        private readonly double _physicsPollInterval = 10.0;


        #region Constructor
        public AssettoCorsa()
        {
            _physicsMemoryReader = new SharedMemoryReader(_physicsFileName, _physicsBufferSize);
            _cancel = new CancellationTokenSource();
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
            get
            {
                return _processNames;
            }
        }
        #endregion

        #region Public Methods
        public bool Start(TelemetryData telemetryData)
        {
            _telemetryData = telemetryData;
            ReadData(_cancel.Token);
            return true;
        }

        public bool Stop()
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
            var tt = 56;
        }
        #endregion

    }
}
