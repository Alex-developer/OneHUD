using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using AGServer.Plugin;
using AGServer.Processes;
using AGServerInterface;
using AGServer.Servers;
using AGServer.Servers.Net;
using AGData;

namespace AGServer
{
    public partial class AGServer : Form
    {
        Dictionary<string, IGame> _plugins;
        private ProcessMonitor _processMonitor;
        private IGame _game = null;
        private TelemetryData _telemetryData;
        private Thread _gameReaderThread;
        private Thread _httpServerThread;

        private IPAddress _ipAddress;
        private int _httpServerPort = 80;
        private string _httpServerPath = Directory.GetCurrentDirectory() + "\\Web";
        private HTTPServer _httpServer;

        #region Constructor
        public AGServer()
        {
            InitializeComponent();
        }
        #endregion

        #region Startup Code
        public void Startup()
        {
            
            _plugins = new Dictionary<string, IGame>();
            ICollection<IGame> plugins = PluginLoader<IGame>.LoadPlugins("Plugins");
            if (plugins.Count > 0)
            {
                foreach (var item in plugins)
                {
                    _plugins.Add(item.Name, item);
                    item.GameEvent += HandleGameEvent;
                }
                _processMonitor = new ProcessMonitor(_plugins);
                _processMonitor.GameLoadedEvent += new ProcessMonitor.GameLoaded(GameLoaded);
                _processMonitor.GameClosedEvent += new ProcessMonitor.GameClosed(GameClosed);
                _processMonitor.StartProcessMonitor();

                _ipAddress = NetHelpers.GetLocalIpAddress();
                
                StartWebServer();
            }
            else
            {
                MessageBox.Show("No Plugins Were Found, Application will now exit", "Plugin Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
        }
        #endregion

        #region Event Handlers
        private void GameLoaded(string gameName, EventArgs e)
        {
            if (_game == null)
            {
                _game = _plugins[gameName];
                Status.Text = _game.DisplayName;
                _telemetryData = new TelemetryData();
                _gameReaderThread = new Thread(() => GameReaderThread());
                _gameReaderThread.Start();

            }
        }

        private void GameReaderThread()
        {
            _game.Start(_telemetryData);
        }

        private void StopGameReaderThread()
        {
            _game.Stop(); // !!!!
            _gameReaderThread.Abort();
        }

        private void GameClosed(string gameName, EventArgs e)
        {
            StopGameReaderThread();
            _game = null;
            Status.Text = "Waiting ...";
        }

        private void HandleGameEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Cleanup
        private void Cleanup()
        {
            StopWebServer();

            if (_processMonitor != null) {
                _processMonitor.StopProcessMonitor();
            }

            if (_game != null)
            {
                _game.Stop();
            }
        }
        #endregion

        #region Form Events
        private void AGServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cleanup();
        }

        private void AGServer_Load(object sender, EventArgs e)
        {
            Startup();
        }
        #endregion

        #region HTTP Server
        private void StartWebServer()
        {
            UpdateHTTPServerInfo();
            if (NetHelpers.CheckPortIsFree(_ipAddress, _httpServerPort))
            {
                _httpServerThread = new Thread(() => HTTPServerThread(_telemetryData, _ipAddress, _httpServerPort, _httpServerPath));
                _httpServerThread.Start();
                UpdateHTTPServerInfo();
            }
            else
            {
                string processName = "Unknown";
                Port process = NetHelpers.FindProcessUsingPort(_httpServerPort);
                if (process != null)
                {
                    processName = process.process_name;
                }

                MessageBox.Show("Unable to start the HTTP Server. The specified port (" + _httpServerPort.ToString() + ") is already in use by " + processName  + ".", "HTTP Server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void StopWebServer()
        {
            if (_httpServerThread != null && _httpServerThread.IsAlive)
            {
                _httpServer.Stop();
                _httpServerThread.Abort();
            }
        }

        private void HTTPServerThread(TelemetryData telemetryData, IPAddress ipAddress, int port, string httpServerPath)
        {
            _httpServer = new HTTPServer(httpServerPath, port, telemetryData, ipAddress);
            _httpServer.Start();
            while (true)
            {
                Thread.Sleep(1);
            }
        }

        #region HTTP Server UI Code
        private void UpdateHTTPServerInfo()
        {
            lblIPAddress.Text = _ipAddress.ToString();
            txtPort.Text = _httpServerPort.ToString();

            if (_httpServerThread != null && _httpServerThread.IsAlive)
            {
                lblWebServerStatus.Text = "Running";
                lblWebServerStatus.ForeColor = System.Drawing.Color.White;
                lblWebServerStatus.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                lblWebServerStatus.Text = "Stopped";
                lblWebServerStatus.ForeColor = System.Drawing.Color.Black;
                lblWebServerStatus.BackColor = System.Drawing.Color.Red;
            }

        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _httpServerPort = Convert.ToInt32(txtPort.Text);
            }
            catch (FormatException ex)
            {
                txtPort.Text = "";
            }
        }

        private void btnHTTPServerRestart_Click(object sender, EventArgs e)
        {
            StopWebServer();
            StartWebServer();
        }
        #endregion
        #endregion

    }
}
