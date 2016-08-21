using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using OneHUDInterface;
using OneHUDInterface.TrackInfo;
using OneHUDInterface.TrackRecorder;
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

        private TrackRecording _trackRecording;
        private bool _recording = false;
        private float _lastLapDistance = 0;
        private int _recordingDelta = 5;

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

            if (gameState == EGameState.GameIngamePlaying)
            {
                _telemetryData.Car.InCar = true;
            }
            else
            {
                _telemetryData.Car.InCar = false;
            }

            if (_timingData.RaceInfo.TrackLongName == null)
            {
                if (_data.MTrackLocation.Value != "")
                {
                    _timingData.RaceInfo.TrackLongName = _data.MTrackLocation.Value + " " + _data.MTrackVariation.Value;
                    _timingData.RaceInfo.TrackShortName = _data.MTrackLocation.Value;
                    _timingData.RaceInfo.TrackName = _data.MTrackLocation.ToString() + " " + _data.MTrackVariation.Value;
                    _timingData.RaceInfo.TrackVariation = _data.MTrackVariation.Value;
                    _timingData.RaceInfo.TrackLength = (int)_data.MTrackLength;

                    _timingData.RaceInfo.TrackTemperature = _data.MTrackTemperature;
                    _timingData.RaceInfo.AmbientTemperature = _data.MAmbientTemperature;
                }
            }

            if (_telemetryData.Car.InCar)
            {
                _telemetryData.Engine.RPM = _data.MRpm;
                _telemetryData.Car.Speed = ConvertSpeedToMPH(_data.MSpeed);
                _telemetryData.Car.Gear = _data.MGear;
                _telemetryData.Car.FuelRemaining = _data.MFuelLevel * _data.MFuelCapacity;
                _telemetryData.Car.FuelCapacity = _data.MFuelCapacity;

                _telemetryData.Engine.WaterTemp = _data.MWaterTempCelsius;

                _telemetryData.Timing.CurrentLapTime = _data.MCurrentTime;

                if (_recording)
                {
                    float myLapDistance = _data.MParticipantInfo[_data.MViewedParticipantIndex].mCurrentLapDistance;


                    if (_lastLapDistance == -1 || (Math.Abs(_lastLapDistance - myLapDistance) > _recordingDelta))
                    {
                        AddTrackPoint(_data.MParticipantInfo[_data.MViewedParticipantIndex].mCurrentLap, _data.MParticipantInfo[_data.MViewedParticipantIndex].mWorldPosition);
                        _lastLapDistance = myLapDistance;
                    }
                }
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
            _trackRecording = new TrackRecording();
            _recording = true;
            _lastLapDistance = -1;
            return true;
        }

        public override TrackRecording StopTrackRecorder()
        {
            _recording = false;
            ConvertPoints();
            return _trackRecording;
        }

        public override TrackRecording GetTrackRecording()
        {
            ConvertPoints();
            return _trackRecording;
        }

        public virtual Track GetTrack()
        {
            if (_recording)
            {
                StopTrackRecorder();
            }
            return new Track();
        }

        private void AddTrackPoint(int lap, float[] fPos)
        {
            _trackRecording.AddPoint(lap, fPos[0], fPos[2], fPos[1]);
        }

        private void ConvertPoints()
        {
            List<TrackLap> trackLaps = _trackRecording.TrackLaps;
            TrackBounds trackBounds = _trackRecording.TrackBounds;

            foreach (TrackLap trackLap in trackLaps)
            {
                List<TrackPoint> trackPoints = trackLap.TrackPoints;

                foreach (TrackPoint trackPoint in trackPoints)
                {
                    trackPoint.X = -trackPoint.GameX + Math.Abs(trackBounds.MinGameX);
                    trackPoint.Y = trackPoint.GameY + Math.Abs(trackBounds.MinGameY);
                    trackPoint.Z = trackPoint.GameZ + Math.Abs(trackBounds.MinGameZ);
                }
            }

            _trackRecording.TrackBounds.Width = Math.Abs(_trackRecording.TrackBounds.MinGameX) + Math.Abs(_trackRecording.TrackBounds.MaxGameX);
            _trackRecording.TrackBounds.Height = Math.Abs(_trackRecording.TrackBounds.MinGameY) + Math.Abs(_trackRecording.TrackBounds.MaxGameY);

        }
        #endregion

    }
}