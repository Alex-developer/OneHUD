using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using OneHUDData;
using OneHUDData.AnalysisData;
using OneHUDData.TrackInfo;
using OneHUDData.TrackRecorder;

namespace OneHUDInterface
{
    public abstract class GameBase : IGame
    {

        protected string _name = "";
        protected string _displayName = "";
        protected List<string> _processNames;
        protected string _author;
        protected string _url;
        protected ConnectionType _connectionType = ConnectionType.BYPROCESS;

        public GameBase()
        {
            _processNames = new List<string>();
        }

        public ConnectionType ConnectionType
        {
            get
            {
                return _connectionType;
            }
            set
            {
                _connectionType = value;
            }
        }

        public string URL
        {
            get
            {
                return _url;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
        }

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

        public List<string> ProcessNames
        {
            get
            {
                return _processNames;
            }
        }

        public string Version
        {
            get
            {
                Assembly me = Assembly.GetAssembly(this.GetType());
                return me.GetName().Version.ToString();
            }
        }

        public Bitmap Icon
        {
            get
            {
                Bitmap icon = GetImageByName("icon");
                return icon;
            }
        }

        public virtual bool Start(TelemetryData telemetryData, TimingData timingData, AnalysisManager analysisData)
        {
            return true;
        }

        public virtual bool Stop()
        {
            return true;
        }

        private Bitmap GetImageByName(string imageName)
        {
            Assembly asm = Assembly.GetAssembly(this.GetType());
            string resourceName = asm.GetName().Name + ".Properties.Resources";
            var rm = new System.Resources.ResourceManager(resourceName, asm);
            Bitmap image = null;
            try
            {
                image = (Bitmap)rm.GetObject(imageName);
            }
            catch (Exception ex)
            {

            }
            return image;

        }

        #region Track recorder
        public virtual bool SupportsTrackRecorder()
        {
            return false;
        }

        public virtual bool StartTrackRecorder()
        {
            return false;
        }

        public virtual TrackRecording StopTrackRecorder()
        {
            return null;
        }

        public virtual TrackRecording GetTrackRecording()
        {
            return null;
        }

        public virtual Track GetTrack()
        {
            return null;
        }

        public virtual bool SaveTrack(int driver, int lap)
        {
            return false;
        }

        public virtual Track LoadTrack()
        {
            return null;
        }
        #endregion

        #region Default Options Dialog
        public virtual void ShowOptions()
        {

        }
        #endregion
    }
}
