using System;
using System.Linq;
using OneHUDInterface;
using OneHUDData.TrackInfo;
using OneHUDData.TrackRecorder;

namespace AGServer.Servers.DataHandlers.Actions
{
    class TrackDataHandlerResult : DataHandlerResult
    {

        private bool _result = false;

        private TrackRecording _trackRecording = null;
        private Track _track;

        public Track Track
        {
            get
            {
                return _track;
            }
            set
            {
                _track = value;
            }
        }

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

