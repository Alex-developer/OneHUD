using System;
using System.Linq;
using OneHUDInterface;
using OneHUDInterface.TrackInfo;
using OneHUDInterface.TrackRecorder;

namespace AGServer.Servers.DataHandlers.Actions
{
    class TrackRecorderDataHandlerResult : DataHandlerResult
    {

        private bool _result = false;

        private TrackRecording _trackRecording = null;

        public bool Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
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
    }
}

