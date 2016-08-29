using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using OneHUDInterface;
using OneHUDData.TrackInfo;
using OneHUDData.TrackRecorder;
using OneHUDData.AnalysisData;
using ProjectCars2.DataFormat;
using ProjectCars2.Readers;
using OneHUDData;

using System.Diagnostics;

namespace ProjectCars2
{
    public class AGProjectCars2 : GameBase, IGame
    {
        private readonly string _sharedMemoryFileName = "$pcars$";

        private readonly SharedMemoryReader _memoryReader;
        private readonly int _bufferSize = Marshal.SizeOf(typeof(ProjectCars2.DataFormat.SharedMemory));
        ProjectCars2.DataFormat.SharedMemory _data;

        private readonly CancellationTokenSource _cancel;
        private DateTime _lastTimeStamp;
        private readonly double _pollInterval = 60.0;
        private TelemetryData _telemetryData;
        private TimingData _timingData;
        private bool _connected = false;

        private TrackManager _trackManager = new TrackManager();
        private bool _recording = false;
        private float[] _lastLapDistance = new float[100];
        private int _recordingDelta = 5;

        private EGameState _lastGameState = EGameState.GameExited;

        private AnalysisManager _analysisData;

        #region Constructor
        public AGProjectCars2()
            : base()
        {
            _name = "pCars2";
            _displayName = "Project Cars2";
            _author = "Alex Greenland";
            _processNames.Add("pCARS2Gld");
            _processNames.Add("pCARS2QA");
            _memoryReader = new SharedMemoryReader(_sharedMemoryFileName, _bufferSize);
            _cancel = new CancellationTokenSource();
        }
        #endregion

        #region Public Methods
        public override bool Start(TelemetryData telemetryData, TimingData timingData, AnalysisManager analysisData)
        {
            _telemetryData = telemetryData;
            _timingData = timingData;
            _analysisData = analysisData;
            Reset();
            ReadData(_cancel.Token);
            return true;
        }

        public override bool Stop()
        {
            _cancel.Cancel();
            return true;
        }

        public void Reset()
        {
            _telemetryData.Reset();
            _timingData.Reset();
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
                            _data = (ProjectCars2.DataFormat.SharedMemory)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(ProjectCars2.DataFormat.SharedMemory));
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

            if (gameState != _lastGameState)
            {
                if (gameState == EGameState.GameFrontEnd)
                {
                    Reset();
                }
                _lastGameState = gameState;
            }

            if (gameState != EGameState.GameFrontEnd)
            {
                lock (_timingData)
                {
                    if (_timingData.RaceInfo.TrackLongName == null)
                    {
                        if (_data.MTrackLocation.Value != "")
                        {
                            _timingData.RaceInfo.TrackLongName = _data.MTrackLocation.Value + " " + _data.MTrackVariation.Value;
                            _timingData.RaceInfo.TrackShortName = _data.MTrackLocation.Value;
                            _timingData.RaceInfo.TrackName = _data.MTrackLocation.Value + " " + _data.MTrackVariation.Value;
                            _timingData.RaceInfo.TrackVariation = _data.MTrackVariation.Value;
                            _timingData.RaceInfo.TrackLength = (int)_data.MTrackLength;

                            _timingData.RaceInfo.TrackTemperature = _data.MTrackTemperature;
                            _timingData.RaceInfo.AmbientTemperature = _data.MAmbientTemperature;

                            _trackManager.LoadCurrentTrack(_timingData.RaceInfo.TrackName, _displayName);
                        }
                    }
                }

                lock (_telemetryData)
                {
                    _telemetryData.Car.InCar = true;

                    _telemetryData.Engine.RPM = _data.MRpm;
                    _telemetryData.Car.Speed = ConvertSpeedToMPH(_data.MSpeed);
                    _telemetryData.Car.Gear = _data.MGear;
                    _telemetryData.Car.FuelRemaining = _data.MFuelLevel * _data.MFuelCapacity;
                    _telemetryData.Car.FuelCapacity = _data.MFuelCapacity;

                    _telemetryData.Engine.WaterTemp = _data.MWaterTempCelsius;

                    _telemetryData.Timing.CurrentLapTime = _data.MCurrentTime;

                    if (_recording)
                    {
                        SetTrackname(_timingData.RaceInfo.TrackName);

                        for (int i = 0; i < _data.MNumParticipants; i++)
                        {
                            float lapDistance = _data.MParticipantInfo[i].mCurrentLapDistance;

                            if (_lastLapDistance[i] == -1 || (Math.Abs(_lastLapDistance[i] - lapDistance) > _recordingDelta))
                            {
                                AddTrackPoint(i, _data.MParticipantInfo[i].mCurrentLap, _data.MParticipantInfo[i].mWorldPosition);
                                _lastLapDistance[i] = lapDistance;

                            }
                        }
                    }

                    if (_trackManager.CurrentTrack != null)
                    {
                        _telemetryData.ResetPlayers();
                        for (int i = 0; i < _data.MNumParticipants; i++)
                        {
                            float x = -_data.MParticipantInfo[i].mWorldPosition[0] + Math.Abs(_trackManager.CurrentTrack.TrackBounds.MaxGameX);
                            float y = _data.MParticipantInfo[i].mWorldPosition[2] + Math.Abs(_trackManager.CurrentTrack.TrackBounds.MinGameY);
                            float z = _data.MParticipantInfo[i].mWorldPosition[1] + Math.Abs(_trackManager.CurrentTrack.TrackBounds.MinGameZ);
                            bool isMe = false;
                            if (i == _data.MViewedParticipantIndex)
                            {
                                isMe = true;
                            }
                            _telemetryData.AddPlayer(x, y, z, isMe);
                        }
                    }

                }
            }
            else
            {
                _telemetryData.Car.InCar = false;
            }
        }
        #endregion

        #region Helper functions
        private float ConvertSpeedToMPH(float Speed)
        {
            return Speed * (float)2.236936;
        }
        #endregion

        #region Track recorder
        public override bool SupportsTrackRecorder()
        {
            return true;
        }

        public override bool StartTrackRecorder()
        {

            _trackManager.StartRecording();

            _recording = true;
            for (int i = 0; i < _lastLapDistance.Length; i++)
            {
                _lastLapDistance[i] = -1;
            }
            return true;
        }

        public override TrackRecording StopTrackRecorder()
        {
            _recording = false;
            ConvertPoints();
            return _trackManager.TrackRecording;
        }

        public override TrackRecording GetTrackRecording()
        {
            ConvertPoints();
            return _trackManager.TrackRecording;
        }

        public virtual Track GetTrack()
        {
            if (_recording)
            {
                StopTrackRecorder();
            }
            return new Track();
        }

        public bool SaveTrack(int driverPos, int lap)
        {
            return _trackManager.SaveTrack(driverPos, lap, _displayName);
        }

        public override Track LoadTrack()
        {
            return _trackManager.LoadTrack(_timingData.RaceInfo.TrackName, _displayName);
        }

        private void SetTrackname(string name)
        {
            _trackManager.SetTrackname(name);
        }

        private void AddTrackPoint(int driverPos, int lap, float[] fPos)
        {
            _trackManager.AddPoint(driverPos, lap, fPos[0], fPos[2], fPos[1]);
        }

        private void ConvertPoints()
        {
            int drivers = _trackManager.TrackRecording.TrackDrivers.Count;

            TrackBounds trackBounds = _trackManager.TrackBounds;
            _trackManager.TrackBounds.Width = Math.Abs(_trackManager.TrackBounds.MinGameX) + Math.Abs(_trackManager.TrackBounds.MaxGameX) + (trackBounds.Margin * 2);
            _trackManager.TrackBounds.Height = Math.Abs(_trackManager.TrackBounds.MinGameY) + Math.Abs(_trackManager.TrackBounds.MaxGameY) + (trackBounds.Margin * 2);

            for (int i = 0; i < drivers; i++)
            {
                List<TrackLap> trackLaps = _trackManager.TrackRecording.TrackDrivers.Find(p => p.Id == i).TrackLaps;

                foreach (TrackLap trackLap in trackLaps)
                {
                    List<TrackPoint> trackPoints = trackLap.TrackPoints;

                    foreach (TrackPoint trackPoint in trackPoints)
                    {
                        trackPoint.X = -trackPoint.GameX + Math.Abs(trackBounds.MaxGameX) + trackBounds.Margin;
                        trackPoint.Y = trackPoint.GameY + Math.Abs(trackBounds.MinGameY) + trackBounds.Margin;
                        trackPoint.Z = trackPoint.GameZ + Math.Abs(trackBounds.MinGameZ);
                    }
                }

            }
        }
        #endregion

    }
}