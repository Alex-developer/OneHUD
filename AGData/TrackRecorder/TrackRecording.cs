using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using OneHUDData.TrackInfo;

namespace OneHUDData.TrackRecorder
{
    public class TrackRecording
    {
        private int _lastLap = -99;
        private List<TrackLap> _trackLaps;
        private TrackBounds _trackBounds;
        private string _trackName;

        #region Constructor
        public TrackRecording()
        {
            _trackBounds = new TrackBounds();
            _trackLaps = new List<TrackLap>();
        }
        #endregion

        #region Getters and Setters
        public string TrackName
        {
            get
            {
                return _trackName;
            }
            set
            {
                _trackName = value;
            }
        }

        public TrackBounds TrackBounds
        {
            get
            {
                return _trackBounds;
            }
            set
            {
                _trackBounds = value;
            }
        }

        public List<TrackLap> TrackLaps
        {
            get
            {
                return this._trackLaps;
            }
            set
            {
                this._trackLaps = value;
            }
        }

        #endregion

        #region public methods
        #region Add new trackpoint
        public void AddPoint(int lap, float x, float y, float z)
        {
            if (lap != _lastLap) {
                _trackLaps.Add(new TrackLap() { Lap = lap});
                _lastLap = lap;
            }

            TrackLap currentLap = _trackLaps.Last();
            TrackPoint trackPoint = new TrackPoint() { GameX = x, GameY = y, GameZ = z };
            currentLap.AddPoint(trackPoint);
            _trackBounds.Update(trackPoint);
        }

        #endregion

        #region Lap Info
        public int GetTotalLaps()
        {
            return _trackLaps.Count;
        }

        public TrackLap GetLap(int lap)
        {
            TrackLap trackLap = _trackLaps.Find(x => x.Lap == lap);

            return trackLap;
        }
        #endregion

        #region Build and Save the Track Object
        public bool SaveTrack(int lap, string gameName)
        {
            Track track = new Track();
            track.TrackBounds = _trackBounds;
            track.TrackName = _trackName;
            track.TrackPoints = _trackLaps[lap].TrackPoints;

            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder​.MyDocuments),"My Games", "OneHUD", gameName, "Tracks");
            
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string json = new JavaScriptSerializer().Serialize(track);

            string fileName = System.IO.Path.Combine(path, _trackName);

            File.WriteAllText(fileName, json);
            return true;
        }
        #endregion
        #endregion

    }
}
