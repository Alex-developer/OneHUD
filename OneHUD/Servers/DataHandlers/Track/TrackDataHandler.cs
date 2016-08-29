using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using OneHUDData;
using OneHUDData.TrackInfo;
using OneHUDInterface;

namespace AGServer.Servers.DataHandlers.Actions
{
    class TrackDataHandler
    {
        public static TrackDataHandlerResult ProcessRequest(TelemetryData telemetry, Dictionary<string, IGame> plugins, NameValueCollection postData)
        {
            TrackDataHandlerResult result = new TrackDataHandlerResult();

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

                    case "SaveTrack":
                        if (telemetry.Game != null)
                        {
                            IGame _game = plugins[telemetry.Game];
                            if (_game.SupportsTrackRecorder())
                            {
                                int lap;
                                int driver;
                                if (Int32.TryParse(postData["data[lap]"], out lap))
                                {
                                    if (Int32.TryParse(postData["data[driver]"], out driver))
                                    {
                                        _game.SaveTrack(driver, lap);
                                        result.Result = true;
                                    }
                                    else
                                    {
                                        result.Result = false;
                                    }
                                } else {
                                    result.Result = false;
                                }
                                
                                var tt = 56;
                            }
                        }
                        break;

                    case "LoadTrack":
                        if (telemetry.Game != null)
                        {
                            IGame _game = plugins[telemetry.Game];
                            Track track =_game.LoadTrack();
                            if (track != null)
                            {
                                result.Result = true;
                                result.Track = track;
                            }
                            else
                            {
                                result.Result = false;
                            }
                        }
                        break;
                }
            }

            return result;
        }
    }
}
