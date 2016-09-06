using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using OneHUDInterface;
using OneHUDData.TrackInfo;
using OneHUDData.TrackRecorder;
using OneHUDData.AnalysisData;
using ProjectCars.DataFormat;
using ProjectCars.Readers;
using OneHUDData;

namespace ProjectCars
{
    public class AGProjectCars : GameBase, IGame
    {
        private readonly string _sharedMemoryFileName = "$pcars$";

        private readonly SharedMemoryReader _memoryReader;
        private UDPReader _udpReader;
        private readonly int _bufferSize = Marshal.SizeOf(typeof(ProjectCars.DataFormat.SharedMemory));
        ProjectCars.DataFormat.SharedMemory _data;

        private PageTypes _supports = PageTypes.Dash | PageTypes.TrackMap | PageTypes.Telemetry | PageTypes.ServerOptions;

        private readonly CancellationTokenSource _cancel;
        private DateTime _lastTimeStamp;
        private DateTime _lastAnalysisTimeStamp;
        private readonly int _pollInterval = 60;
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
        public AGProjectCars()
            : base()
        {
            _name = "pCars";
            _displayName = "Project Cars";
            _author = "Alex Greenland";
            _processNames.Add("pCARS");
            _processNames.Add("pCARS64");
            _connectionType = OneHUDInterface.ConnectionType.BOTH;
            _memoryReader = new SharedMemoryReader(_sharedMemoryFileName, _bufferSize);
            _cancel = new CancellationTokenSource();
        }
        #endregion

        #region Public Methods
        public override PageTypes Supports
        {
            get
            {
                return _supports;
            }
        }

        public override bool Start(TelemetryData telemetryData, TimingData timingData, AnalysisManager analysisData)
        {
            _telemetryData = telemetryData;
            _timingData = timingData;
            _analysisData = analysisData;
            Reset();

            ConnectionType connectionType = (ConnectionType)Properties.Settings.Default.connectionType;

            if (connectionType == ProjectCars.ConnectionType.SharedMemory)
            {
                ReadData(_cancel.Token);
            }
            else
            {
                ReadUDPData(_cancel.Token);
            }
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

        #region Data Readers
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
                            _data = (ProjectCars.DataFormat.SharedMemory)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(ProjectCars.DataFormat.SharedMemory));
                            ProcessData();
                        }
                        finally
                        {
                            gCHandle.Free();
                        }

                    }
                    Thread.Sleep(_pollInterval);
                }
            }));
        }
        #endregion

        #region UDP Data Reader
        private async void ReadUDPData(CancellationToken token)
        {
            _udpReader = new UDPReader();

            await Task.Factory.StartNew((Action)(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (_udpReader.Connected)
                    {
                        ProcessUDPData();
                    }
                    Thread.Sleep(_pollInterval);
                }
            }));
            _udpReader.Stop();
            _udpReader = null;
        }
        #endregion
        #endregion


        #region Process UDP Data. Merge into normal shared memory packet
        private void ProcessUDPData()
        {
            lock (_udpReader) // YUK !!!!
            {
                UDPDataFormat.sTelemetryData _tempTelemetryData = _udpReader.TelemetryData;
                UDPDataFormat.sParticipantInfoStrings _tempParticipantInfoString = _udpReader.ParticipantInfoStrings;

                _data.MGameState = (int)_tempTelemetryData.sGameSessionState & 7;
                _data.MSessionState = (int)_tempTelemetryData.sGameSessionState >> 4;
                _data.MRaceState = (int)_tempTelemetryData.sRaceStateFlags & 7;

                _data.MViewedParticipantIndex = _tempTelemetryData.sViewedParticipantIndex;
                _data.MNumParticipants = _tempTelemetryData.sNumParticipants;

                _data.MUnfilteredThrottle = _tempTelemetryData.sUnfilteredThrottle;
                _data.MUnfilteredBrake = _tempTelemetryData.sUnfilteredBrake;
                _data.MUnfilteredSteering = _tempTelemetryData.sUnfilteredSteering;
                _data.MUnfilteredClutch = _tempTelemetryData.sUnfilteredClutch;

                SetValueIfNotNULL(_data.MCarName, _tempParticipantInfoString.sCarName);
                SetValueIfNotNULL(_data.MCarClassName, _tempParticipantInfoString.sCarClassName);

                _data.MLapsInEvent = _tempTelemetryData.sLapsInEvent;

                SetValueIfNotNULL(_data.MTrackLocation, _tempParticipantInfoString.sTrackLocation);
                SetValueIfNotNULL(_data.MTrackVariation, _tempParticipantInfoString.sTrackVariation);
                _data.MTrackLength = _tempTelemetryData.sTrackLength;

                _data.MLapInvalidated = (_tempTelemetryData.sRaceStateFlags >> 3 & 1) == 1;

                _data.MBestLapTime = _tempTelemetryData.sBestLapTime;
                _data.MLastLapTime = _tempTelemetryData.sLastLapTime;
                _data.MCurrentTime = _tempTelemetryData.sCurrentTime;
                _data.MSplitTimeAhead = _tempTelemetryData.sSplitTimeAhead;
                _data.MSplitTimeBehind = _tempTelemetryData.sSplitTimeBehind;
                _data.MSplitTime = _tempTelemetryData.sSplitTime;
                _data.MEventTimeRemaining = _tempTelemetryData.sEventTimeRemaining;
                _data.MPersonalFastestLapTime = _tempTelemetryData.sPersonalFastestLapTime;
                _data.MWorldFastestLapTime = _tempTelemetryData.sWorldFastestLapTime;
                _data.MCurrentSector1Time = _tempTelemetryData.sCurrentSector1Time;
                _data.MCurrentSector2Time = _tempTelemetryData.sCurrentSector2Time;
                _data.MCurrentSector3Time = _tempTelemetryData.sCurrentSector3Time;
                _data.MFastestSector1Time = _tempTelemetryData.sFastestSector1Time;
                _data.MFastestSector2Time = _tempTelemetryData.sFastestSector2Time;
                _data.MFastestSector3Time = _tempTelemetryData.sFastestSector3Time;
                _data.MPersonalFastestSector1Time = _tempTelemetryData.sPersonalFastestSector1Time;
                _data.MPersonalFastestSector2Time = _tempTelemetryData.sPersonalFastestSector2Time;
                _data.MPersonalFastestSector3Time = _tempTelemetryData.sPersonalFastestSector3Time;
                _data.MWorldFastestSector1Time = _tempTelemetryData.sWorldFastestSector1Time;
                _data.MWorldFastestSector2Time = _tempTelemetryData.sWorldFastestSector2Time;
                _data.MWorldFastestSector3Time = _tempTelemetryData.sWorldFastestSector3Time;

                _data.MHighestFlagColour = (int)_tempTelemetryData.sHighestFlag & 7; 
                _data.MHighestFlagReason = (int)_tempTelemetryData.sHighestFlag >> 3 & 3;

                _data.MPitMode = (int)_tempTelemetryData.sPitModeSchedule & 7;
                _data.MPitSchedule = (int)_tempTelemetryData.sPitModeSchedule >> 3 & 3;

                _data.MCarFlags = _tempTelemetryData.sCarFlags;
                _data.MOilTempCelsius = _tempTelemetryData.sOilTempCelsius;
                _data.MOilPressureKPa = _tempTelemetryData.sOilPressureKPa;
                _data.MWaterTempCelsius = _tempTelemetryData.sWaterTempCelsius;
                _data.MWaterPressureKPa = _tempTelemetryData.sWaterPressureKpa;
                _data.MFuelPressureKPa = _tempTelemetryData.sFuelPressureKpa;
                _data.MFuelLevel = _tempTelemetryData.sFuelLevel;
                _data.MFuelCapacity = _tempTelemetryData.sFuelCapacity;
                _data.MSpeed = _tempTelemetryData.sSpeed;
                _data.MRpm = _tempTelemetryData.sRpm;
                _data.MMaxRpm = _tempTelemetryData.sMaxRpm;
                _data.MBrake = (float)_tempTelemetryData.sBrake / 255f;
                _data.MThrottle = (float)_tempTelemetryData.sThrottle / 255f;
                _data.MClutch = (float)_tempTelemetryData.sClutch / 255f;
                _data.MSteering = (float)_tempTelemetryData.sSteering / 127f;
                _data.MGear = _tempTelemetryData.sGearNumGears & 15;
                _data.MNumGears = _tempTelemetryData.sGearNumGears >> 4;
                _data.MOdometerKm = _tempTelemetryData.sOdometerKM;
                _data.MAntiLockActive = (_tempTelemetryData.sRaceStateFlags >> 4 & 1) == 1;
                _data.MBoostActive = (_tempTelemetryData.sRaceStateFlags >> 5 & 1) == 1;
                _data.MBoostAmount = _tempTelemetryData.sBoostAmount;

                _data.MOrientation = _tempTelemetryData.sOrientation;
                _data.MLocalVelocity = _tempTelemetryData.sLocalVelocity;
                _data.MWorldVelocity = _tempTelemetryData.sWorldVelocity;
                _data.MAngularVelocity = _tempTelemetryData.sAngularVelocity;
                _data.MLocalAcceleration = _tempTelemetryData.sLocalAcceleration;
                _data.MWorldAcceleration = _tempTelemetryData.sWorldAcceleration;
                _data.MExtentsCentre = _tempTelemetryData.sExtentsCentre;

                _data.MTyreFlags = ConvertByteToIntArray(_tempTelemetryData.sTyreFlags); 
            }

            ProcessData();
        }

        private int[] ConvertByteToIntArray(byte[] bytes)
        {
            List<int> intArray = new List<int>();
            foreach (byte i in bytes)
            {
                intArray.Add((byte)i);
            }
            return intArray.ToArray();
        }

        private void SetValueIfNotNULL(SmString property, SmString value)
        {
            if (value.Value != null)
            {
                property = value;
            }
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

                if (gameState != EGameState.GameIngamePaused)
                {
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

                        DateTime utcNow = DateTime.UtcNow;
                        if ((utcNow - _lastAnalysisTimeStamp).TotalMilliseconds >= 2000)
                        {
                            ParticipantInfo pi = _data.MParticipantInfo[_data.MViewedParticipantIndex];
                            _analysisData.AddDataPoint(_data.MViewedParticipantIndex, pi.mCurrentLap, _telemetryData.Car.Speed, _telemetryData.Engine.RPM);
                            _lastAnalysisTimeStamp = utcNow;
                        }

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
            for (int i=0;i< _lastLapDistance.Length; i++) {
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

            for (int i = 0 ;i < drivers; i++) {
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

        #region Default Options Dialog
        public override void ShowOptions()
        {
            Options optionsDialog = new Options();

            ConnectionType connectionType = (ConnectionType)Properties.Settings.Default.connectionType;
            string ipAddress = Properties.Settings.Default.ipAddress;
            string port = Properties.Settings.Default.port;

            optionsDialog.ConnectionType = connectionType;
            optionsDialog.IPAddress = ipAddress;
            optionsDialog.Port = port;


            optionsDialog.Text = DisplayName + " Options";
            DialogResult result = optionsDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                connectionType = optionsDialog.ConnectionType;
                ipAddress = optionsDialog.IPAddress;
                port = optionsDialog.Port;

                Properties.Settings.Default.connectionType = (int)connectionType;
                Properties.Settings.Default.ipAddress = ipAddress;
                Properties.Settings.Default.port = port;

                Properties.Settings.Default.Save();
            }
        }
        #endregion
    }

    #region Enums
    public enum ConnectionType
    {
        SharedMemory = 1,
        UDP = 2
    }
    #endregion

}