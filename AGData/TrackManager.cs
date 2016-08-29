using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using OneHUDData.TrackInfo;
using OneHUDData.TrackRecorder;

namespace OneHUDData
{
    public class TrackManager
    {
        private TrackRecording _trackRecording;
        private Track _currentTrack;

        #region Getters and Setters
        public Track CurrentTrack
        {
            get
            {
                return _currentTrack;
            }
            set
            {
                _currentTrack = value;
            }
        }

        public TrackRecording TrackRecording
        {
            get
            {
                return _trackRecording;
            }
            set
            {
                _trackRecording = value;
            }
        }

        public TrackBounds TrackBounds
        {
            get
            {
                return _trackRecording.TrackBounds;
            }
        }
        #endregion

        #region Public Methods
        #region Start Recording
        public void StartRecording()
        {
            _trackRecording = new TrackRecording();
        }
        #endregion

        #region Add Track Point
        public void AddPoint(int driverPos, int lap, float x, float y, float z) {
            _trackRecording.AddPoint(driverPos, lap, x, y, z);
        }
        #endregion

        #region SetTrackName
        public void SetTrackname(string name)
        {
            if (_trackRecording.TrackName == null)
            {
                _trackRecording.TrackName = name;
            }
        }
        #endregion

        #region Save a Track
        public bool SaveTrack(int driverPos, int lap, string trackName)
        {
            return _trackRecording.SaveTrack(driverPos, lap, trackName);
        }
        #endregion

        #region Load a track
        public void LoadCurrentTrack(string trackName, string gameName) {
            _currentTrack = LoadTrack( trackName,  gameName);
        }

        public Track LoadTrack(string trackName, string gameName)
        {
            Track track = null;

            try
            {
                string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "OneHUD", gameName, "Tracks", trackName);

                if (File.Exists(fileName))
                {
                    string trackData = System.IO.File.ReadAllText(fileName);

                    track = new Track();

                    track = new JavaScriptSerializer().Deserialize<Track>(trackData);
                }
            }
            catch (Exception ex)
            {

            }
            return track;
        }
        #endregion
        #endregion
    }
}
