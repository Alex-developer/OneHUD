using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using OneHUDInterface;
using OneHUDData;
using RaceRoom.Readers;
using RaceRoom.DataFormat;

namespace RaceRoom
{
    public class RaceRoom : GameBase, IGame
    {
        private readonly string _sharedMemoryFileName = "$Race$";

        private readonly SharedMemoryReader _memoryReader;
        private readonly int _bufferSize = Marshal.SizeOf(typeof(Shared));
        Shared _data;
        private readonly CancellationTokenSource _cancel;
        private DateTime _lastTimeStamp;
        private readonly double _pollInterval = 10.0;
        private TelemetryData _telemetryData;
        private TimingData _timingData;
        private bool _connected = false;
        private GameData _gameData;

        #region Constructor
        public RaceRoom() : base()
        {
            _gameData = new GameData();

            var path = AppDomain.CurrentDomain.BaseDirectory;
            var jsonString = File.ReadAllText(path + @"Plugins\\R3E-GameData.json");
            _gameData = JsonConvert.DeserializeObject<GameData>(jsonString);


            _author = "Alex Greenland";
            _name = "R3E";
            _displayName = "RaceRoom Experience";
            _processNames.Add("RRRE");
            _memoryReader = new SharedMemoryReader(_sharedMemoryFileName, _bufferSize);
            _cancel = new CancellationTokenSource();
        }
        #endregion

        #region GameData Methods
        public Car GetCarData(int id)
        {
            return _gameData != null && id != -1 ? _gameData.Cars.Where(c => c.Id == id).FirstOrDefault() : new Car() { Name = "" };
        }

        public Class GetCarClass(int id)
        {
            return _gameData != null && id != -1 ? _gameData.Classes.Where(c => c.Id == id).FirstOrDefault() : new Class() { Name = "" };
        }

        public Track GetTrackData(int id)
        {
            return _gameData != null && id != -1 ? _gameData.Tracks.Where(t => t.Id == id).FirstOrDefault() : new Track() { Name = "", Layouts = new Layout[] { } };
        }

        public Team GetTeamData(int id)
        {
            return _gameData != null && id != -1 ? _gameData.Teams.Where(t => t.Id == id).FirstOrDefault() : new Team() { Name = "" };
        }

        public Layout GetLayoutData(int id)
        {
            return _gameData != null && id != -1 ? _gameData.Layouts.Where(t => t.Id == id).FirstOrDefault() : new Layout() { Name = "" };
        }
        #endregion

        #region Public Methods
        public override bool Start(TelemetryData telemetryData, TimingData timingData)
        {
            _telemetryData = telemetryData;
            _timingData = timingData;
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
            await Task.Factory.StartNew((Action)(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (!_connected)
                    {
                        _connected = _memoryReader.Connect();
                    }
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

            int trackid = _data.TrackInfo.TrackId;
            int layoutid = _data.TrackInfo.LayoutId;

            _timingData.RaceInfo.TrackLongName = GetTrackData(trackid).Name;
            _timingData.RaceInfo.TrackShortName = GetTrackData(trackid).Name;
            _timingData.RaceInfo.TrackVariation = GetLayoutData(layoutid).Name;
            _timingData.RaceInfo.TrackName = _timingData.RaceInfo.TrackLongName + " " + _timingData.RaceInfo.TrackVariation;
            _timingData.RaceInfo.TrackLength = 0;
            _timingData.RaceInfo.TrackTemperature = 0; // Not supported
            _timingData.RaceInfo.AmbientTemperature = 0; // Not Supported

            _telemetryData.Engine.RPM = _data.EngineRps;
            _telemetryData.Car.Gear = _data.Gear;
            _telemetryData.Car.Speed = _data.CarSpeed;
            _telemetryData.Car.FuelRemaining = _data.FuelLeft;
            _telemetryData.Car.FuelCapacity = _data.FuelCapacity;

            _telemetryData.Engine.WaterTemp = _data.EngineWaterTemp;

            _telemetryData.Timing.CurrentLapTime = _data.LapTimeCurrentSelf;
        }
        #endregion

    }
}
