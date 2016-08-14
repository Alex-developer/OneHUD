using System;
using System.Linq;
using System.Drawing;
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
        protected string _author;

        public GameBase()
        {
            _processNames = new List<string>();
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

        public virtual bool Start(TelemetryData telemetryData, TimingData timingData)
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

    }
}
