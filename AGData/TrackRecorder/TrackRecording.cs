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
        private List<TrackDriver> _trackDrivers;
        private TrackBounds _trackBounds;
        private string _trackName;

        #region Constructor
        public TrackRecording()
        {
            _trackBounds = new TrackBounds();
            _trackDrivers = new List<TrackDriver>();
        }
        #endregion

        #region Getters and Setters
        public List<TrackDriver> TrackDrivers
        {
            get
            {
                return _trackDrivers;
            }
            set
            {
                _trackDrivers = value;
            }
        }

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
        #endregion

        #region public methods
        #region Add new trackpoint
        public void AddPoint(int driverPos, int lap, float x, float y, float z)
        {

            TrackDriver driver = _trackDrivers.Find(p => p.Id == driverPos);
            if (driver == null)
            {
                driver = new TrackDriver() { Id = driverPos };
                _trackDrivers.Add(driver);
            }

            TrackPoint trackPoint = driver.AddPoint(lap, x, y, z);
            _trackBounds.Update(trackPoint);
        }
        #endregion

        #region Build and Save the Track Object
        public bool SaveTrack(int driverPos, int lap, string gameName)
        {
            Track track = new Track();
            track.TrackBounds = _trackBounds;
            track.TrackName = _trackName;
            track.TrackPoints = _trackDrivers.Find(p => p.Id == driverPos).Lap(lap).TrackPoints;

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
