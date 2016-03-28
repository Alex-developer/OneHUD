using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AGServerInterface;
using AGData;
using RaceRoom.Readers;
using RaceRoom.DataFormat;

namespace RaceRoom
{
    public class RaceRoom : IGame
    {
        public event EventHandler GameEvent;

        private readonly string _name = "R3E";
        private readonly string _displayName = "RaceRoom Experience";
        private readonly string[] _processNames = { "RRRE" };
        private readonly string _sharedMemoryFileName = "$Race$";

        private readonly SharedMemoryReader _memoryReader;
        private readonly int _bufferSize = Marshal.SizeOf(typeof(Shared));
        Shared _data;
        private readonly CancellationTokenSource _cancel;
        private DateTime _lastTimeStamp;
        private readonly double _pollInterval = 10.0;
        private TelemetryData _telemetryData;

        #region Constructor
        public RaceRoom()
        {
            _memoryReader = new SharedMemoryReader(_sharedMemoryFileName, _bufferSize);
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
            _memoryReader.Connect();

            await Task.Factory.StartNew((Action)(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    DateTime utcNow = DateTime.UtcNow;
                    if ((utcNow - _lastTimeStamp).TotalMilliseconds >= _pollInterval)
                    {
                        _lastTimeStamp = utcNow;
                        byte[] buffer = _memoryReader.Read();

                        GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        try
                        {
                            _data = (Shared)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(Shared));
                            ProcessData();
                        }
                        finally
                        {
                            gCHandle.Free();
                        }

                    }
                    Thread.Sleep((int)_pollInterval);
                }
            }));
        }
        #endregion

        #region Process Shared memory data
        private void ProcessData()
        {
            _telemetryData.Engine.RPM = _data.EngineRps;
        }
        #endregion

    }
}
