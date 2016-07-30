using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using OneHUDData;

namespace OneHUDInterface
{
    public abstract class GameBase : IGame
    {

        protected string _name = "";
        protected string _displayName = "";
        protected List<string> _processNames;

        public GameBase()
        {
            _processNames = new List<string>();
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
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion; ;
            }
        }

        public virtual bool Start(TelemetryData telemetryData, TimingData timingData)
        {
            return true;
        }

        public virtual bool Stop()
        {
            return true;
        }

    }
}
