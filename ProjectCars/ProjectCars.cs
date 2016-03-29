using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AGServerInterface;
using ProjectCars.DataFormat;
using ProjectCars.Readers;
using AGData;

namespace ProjectCars
{
    public class AGProjectCars : IGame
    {
        public event EventHandler GameEvent;

        private readonly string _name = "pCars";
        private readonly string _displayName = "Project Cars";
        private readonly string[] _processNames = { "pCARS", "pCARS64", "pCARS2Gld" };
        private readonly string _sharedMemoryFileName = "$pcars$";

        private readonly SharedMemoryReader _memoryReader;
        private readonly int _bufferSize = Marshal.SizeOf(typeof (ProjectCars.DataFormat.SharedMemory));
        ProjectCars.DataFormat.SharedMemory _data;

        private readonly CancellationTokenSource _cancel;
        private DateTime _lastTimeStamp;
        private readonly double _pollInterval = 10.0;
        private TelemetryData _telemetryData;

        #region Constructor
        public AGProjectCars()
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
                            _data = (ProjectCars.DataFormat.SharedMemory)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(ProjectCars.DataFormat.SharedMemory));
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
            EGameState gameState = (EGameState)_data.MGameState;
            ESessionState sessionState = (ESessionState)_data.MSessionState;

            if (gameState == EGameState.GameIngamePlaying)
            {
                _telemetryData.Car.InCar = true;
            }
            else
            {
                _telemetryData.Car.InCar = false;
            }
            _telemetryData.Engine.RPM = _data.MRpm;
        }
        #endregion
    }
}
