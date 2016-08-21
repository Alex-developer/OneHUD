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

        #region Getters and Setters
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

        public List<TrackLap> TrackLaps
        {
            get
            {
                return _trackRecording.TrackLaps;
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
        public void AddPoint(int lap, float x, float y, float z) {
            _trackRecording.AddPoint(lap, x, y, z);
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
        public bool SaveTrack(int lap, string trackName)
        {
            return _trackRecording.SaveTrack(lap, trackName);
        }
        #endregion

        #region Load a track
        public Track LoadTrack(string trackName, string gameName)
        {

            string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "OneHUD", gameName, "Tracks", trackName);

            Track track = null;

            if (File.Exists(fileName))
            {
                string trackData = System.IO.File.ReadAllText(fileName);

                track = new Track();

                track = new JavaScriptSerializer().Deserialize<Track>(trackData);
            }

            return track;
        }
        #endregion
        #endregion
    }
}
