using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using OneHUDData;
using OneHUDInterface;

namespace AGServer.Servers.DataHandlers.Actions
{
    class TrackRecorderDataHandler
    {
        public static TrackRecorderDataHandlerResult ProcessRequest(TelemetryData telemetry, Dictionary<string, IGame> plugins, NameValueCollection postData)
        {
            TrackRecorderDataHandlerResult result = new TrackRecorderDataHandlerResult();

            if (postData.AllKeys.Contains("action"))
            {
                switch (postData["action"])
                {
                    case "StartRecording":
                        if (telemetry.Game != null)
                        {
                            IGame _game = plugins[telemetry.Game];
                            if (_game.SupportsTrackRecorder())
                            {
                                _game.StartTrackRecorder();
                                result.Result = true;
                            }
                        }
                        break;

                    case "StopRecording":
                        if (telemetry.Game != null)
                        {
                            IGame _game = plugins[telemetry.Game];
                            if (_game.SupportsTrackRecorder())
                            {
                                result.TrackRecording = _game.StopTrackRecorder();
                                result.Result = true;
                            }
                        }
                        break;

                    case "GetTrackRecording":
                        if (telemetry.Game != null)
                        {
                            IGame _game = plugins[telemetry.Game];
                            if (_game.SupportsTrackRecorder())
                            {
                                result.TrackRecording = _game.GetTrackRecording();
                                result.Result = true;
                            }
                        }
                        break;
                }
            }

            return result;
        }
    }
}
