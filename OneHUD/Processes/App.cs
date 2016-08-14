using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace OneHUD.Processes
{
    [Serializable]
    public class App : INotifyPropertyChanged
    {
        private string _application;

        public string Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
