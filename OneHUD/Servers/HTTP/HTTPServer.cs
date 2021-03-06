﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using AGServer.Servers.DataHandlers.Actions;
using AGServer.Servers.DataHandlers.Connected;
using AGServer.Servers.DataHandlers.Startup;
using AGServer.Servers.DataHandlers.HeartBeat;
using AGServer.Servers.DataHandlers.Telemetry;
using AGServer.Servers.HTTP.Services;
using AGServer.Servers.DataHandlers;
using OneHUD.Servers.DataHandlers.Timing;
using WebSocketSharp.Server;
using OneHUDInterface;
using OneHUDData;
using OneHUDData.Timing;

namespace OneHUD.Servers.HTTP
{
    class AGError
    {
        public string result = "Error";
    }

    class HTTPServer
    {
        private readonly string[] indexFiles =
        {
            "index.html",
            "index.htm",
            "default.html",
            "default.htm"
        };

        private static readonly Object lockObject = new Object();

        private static readonly IDictionary<string, string> mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            #region extension to MIME type list
            
            { ".asf", "video/x-ms-asf" },
            { ".asx", "video/x-ms-asf" },
            { ".avi", "video/x-msvideo" },
            { ".bin", "application/octet-stream" },
            { ".cco", "application/x-cocoa" },
            { ".crt", "application/x-x509-ca-cert" },
            { ".css", "text/css" },
            { ".deb", "application/octet-stream" },
            { ".der", "application/x-x509-ca-cert" },
            { ".dll", "application/octet-stream" },
            { ".dmg", "application/octet-stream" },
            { ".ear", "application/java-archive" },
            { ".eot", "application/octet-stream" },
            { ".exe", "application/octet-stream" },
            { ".flv", "video/x-flv" },
            { ".gif", "image/gif" },
            { ".hqx", "application/mac-binhex40" },
            { ".htc", "text/x-component" },
            { ".htm", "text/html" },
            { ".html", "text/html" },
            { ".ico", "image/x-icon" },
            { ".img", "application/octet-stream" },
            { ".iso", "application/octet-stream" },
            { ".jar", "application/java-archive" },
            { ".jardiff", "application/x-java-archive-diff" },
            { ".jng", "image/x-jng" },
            { ".jnlp", "application/x-java-jnlp-file" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".js", "application/x-javascript" },
            { ".mml", "text/mathml" },
            { ".mng", "video/x-mng" },
            { ".mov", "video/quicktime" },
            { ".mp3", "audio/mpeg" },
            { ".mpeg", "video/mpeg" },
            { ".mpg", "video/mpeg" },
            { ".msi", "application/octet-stream" },
            { ".msm", "application/octet-stream" },
            { ".msp", "application/octet-stream" },
            { ".pdb", "application/x-pilot" },
            { ".pdf", "application/pdf" },
            { ".pem", "application/x-x509-ca-cert" },
            { ".pl", "application/x-perl" },
            { ".pm", "application/x-perl" },
            { ".png", "image/png" },
            { ".prc", "application/x-pilot" },
            { ".ra", "audio/x-realaudio" },
            { ".rar", "application/x-rar-compressed" },
            { ".rpm", "application/x-redhat-package-manager" },
            { ".rss", "text/xml" },
            { ".run", "application/x-makeself" },
            { ".sea", "application/x-sea" },
            { ".shtml", "text/html" },
            { ".sit", "application/x-stuffit" },
            { ".swf", "application/x-shockwave-flash" },
            { ".svg", "image/svg+xml" },
            { ".tcl", "application/x-tcl" },
            { ".tk", "application/x-tcl" },
            { ".txt", "text/plain" },
            { ".war", "application/java-archive" },
            { ".wbmp", "image/vnd.wap.wbmp" },
            { ".wmv", "video/x-ms-wmv" },
            { ".xml", "text/xml" },
            { ".xpi", "application/x-xpinstall" },
            { ".zip", "application/zip" },
        
            #endregion
        };

        private readonly string _rootDirectory;
        private readonly HttpListener _listener;
        private readonly int _port;
        private readonly IPAddress _ipAddress;
        private readonly TelemetryData _telemetryData;
        private readonly TimingData _timingData;
        private string _httpServerPath;
        private Dictionary<string, IGame> _plugins;
        private WebSocketSharp.Server.HttpServer _webSocketServer;

        public HTTPServer(string path, int port, TelemetryData gameState, TimingData timingData, IPAddress ipAddress, Dictionary<string, IGame> plugins)
        {
            _telemetryData = gameState;
            _timingData = timingData;
            _rootDirectory = path;
            _port = port;
            _ipAddress = ipAddress;
            _plugins = plugins;
        }


        public void Start()
        {
            Initialise();
        }

        public void Stop()
        {
            _webSocketServer.Stop();
        }

        private void Initialise()
        {
            _webSocketServer = new WebSocketSharp.Server.HttpServer(_port);
            _webSocketServer.RootPath = _rootDirectory + "\\";

            _webSocketServer.AddWebSocketService<TelemetryService>("/Telemetry", () => new TelemetryService(_telemetryData));
            _webSocketServer.AddWebSocketService<HeartBeatService>("/HeartBeat", () => new HeartBeatService(_telemetryData));
            _webSocketServer.AddWebSocketService<TimingService>("/Timing", () => new TimingService(_timingData));
 
            _webSocketServer.Start();

            _webSocketServer.OnPost += (sender, e) =>
            {
                lock (lockObject)
                {
                    var request = e.Request;
                    var response = e.Response;

                    DataHandlerResult result = null;

                    var uri = request.Url;

                    if (uri.Segments.Length == 2)
                    {
                        var action = uri.Segments[1];

                        string postedText = "";
                        using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                        {
                            postedText = reader.ReadToEnd();
                        }
                        NameValueCollection postData = HttpUtility.ParseQueryString(postedText);

                        switch (action) {
                            case "File":
                                result = ActionsDataHandler.ProcessFileRequest(_telemetryData, _plugins, postData);
                                break;

                            case "Track":
                                result = TrackDataHandler.ProcessRequest(_telemetryData, _plugins, postData);
                                break;

                            case "TrackRecorder":
                                result = TrackDataHandler.ProcessRequest(_telemetryData, _plugins, postData);
                                break;

                            case "Connected":
                                result = ConnectedDataHandler.ProcessConnectedRequest(_telemetryData, postData);
                                break;

                            case "HeartBeat":
                                result = HeartBeatDataHandler.ProcessConnectedRequest(_telemetryData, postData);
                                break;

                            case "Telemetry":
                                result = TelemetryDataHandler.ProcessConnectedRequest(_telemetryData, postData);
                                break;

                            case "Timing":
                                result = TimingDataHandler.ProcessConnectedRequest(_timingData, postData);
                                break;
                            case "Startup":
                                result = StartupDataHandler.ProcessStartupRequest(_telemetryData, _plugins, postData);
                                break;
                        }
                        response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        result = new DataHandlerError();
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                    }

                    string json = new JavaScriptSerializer().Serialize(result);
                    response.ContentType = "application/json";

                    byte[] content = Encoding.UTF8.GetBytes(json);
                    response.ContentLength64 = content.Length;

                    System.IO.Stream output = response.OutputStream;
                    output.Write(content, 0, content.Length);

                    result.Dispose();
                }
            };

            _webSocketServer.OnGet += (sender, e) =>
            {
                lock (lockObject)
                {
                    var request = e.Request;
                    var response = e.Response;
                    DataHandlerResult result = null;

                    string[] rawUrlArray = request.RawUrl.Split('?');
                    string rawUrl = rawUrlArray[0];

                    _httpServerPath = rawUrl;

                    if (string.IsNullOrEmpty(_httpServerPath) || _httpServerPath == "/")
                    {
                        foreach (string indexFile in indexFiles)
                        {
                            if (File.Exists(Path.Combine(_rootDirectory, indexFile)))
                            {
                                _httpServerPath = indexFile;
                                break;
                            }
                        }
                    }

                    byte[] content = _webSocketServer.GetFile(_httpServerPath);

                    if (content == null)
                    {

                        List<string> urlBits = rawUrl.Split('/').ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                        switch (urlBits[0])
                        {
                            case "Startup":
                                result = StartupDataHandler.ProcessStartupRequest(_telemetryData, _plugins, request.QueryString);
                                break;
                        }

                        string json = new JavaScriptSerializer().Serialize(result);
                        content = Encoding.UTF8.GetBytes(json);
                    }

                    string mime;
                    response.ContentType = mimeTypeMappings.TryGetValue(Path.GetExtension(_httpServerPath), out mime) ? mime : "application/octet-stream";
                    response.ContentEncoding = Encoding.UTF8;

                    if (content != null)
                    {
                        int start = 0;
                        int end = -1;
                        int byteRange = -1;

                        if (request.Headers.GetValues("Range") != null)
                        {
                            string rangeHeader = request.Headers.GetValues("Range")[0].Replace("bytes=", "");
                            string[] range = rangeHeader.Split('-');
                            start = int.Parse(range[0]);
                            if (range[1].Trim().Length > 0)
                            {
                                int.TryParse(range[1], out end);
                            }
                            if (end == -1) { 
                                end = content.Length; 
                            }

                            byteRange = end - start;
                            response.AddHeader("Accept-Ranges", "bytes");
                            response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", start, byteRange-1, byteRange));
                            response.AddHeader("X-Content-Duration", "0.0");
                            response.AddHeader("Content-Duration", "0.0");

                            response.ContentLength64 = byteRange;
                            response.StatusCode = (int)HttpStatusCode.PartialContent;

                            Console.WriteLine(rangeHeader);
                            Console.WriteLine(string.Format("bytes {0}-{1}/{2}", start, byteRange-1, byteRange));
                            Console.WriteLine("Content length" + byteRange.ToString());
                            var tt = 56;
                            //http://stackoverflow.com/questions/25898922/handle-range-requests-with-httplistener
                            //http://www.stevesouders.com/blog/2013/04/21/html5-video-bytes-on-ios/
                        }
                        else
                        {

                           // response.AddHeader("cache-control", "no-store, must-revalidate, private");
                          //  response.AddHeader("Pragma", "no-cache");

                            response.ContentLength64 = content.Length;
                            response.StatusCode = (int)HttpStatusCode.OK;

                            byteRange = content.Length;
                        }

                        System.IO.Stream output = response.OutputStream;
                        output.Write(content, start, byteRange);

                        response = null;
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                }
            };
        }

        private void process(HttpServer sender, HttpRequestEventArgs e)
        {
        }

        private void processRequest()
        {
        }

    }
}