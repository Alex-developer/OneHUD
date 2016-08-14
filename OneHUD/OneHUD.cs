using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Drawing;
using OneHUD.Plugin;
using OneHUD.Processes;
using OneHUD.Servers.HTTP;
using OneHUD.Servers.Utils;
using OneHUDInterface;
using OneHUDData;

namespace OneHUD
{
    public partial class OneHUD : Form
    {
        Dictionary<string, IGame> _plugins;
        private ProcessMonitor _processMonitor;
        private IGame _game = null;
        private TelemetryData _telemetryData;
        private TimingData _timingData;
        private Thread _gameReaderThread;
        private Thread _httpServerThread;

        private IPAddress _ipAddress;
        private int _httpServerPort = 80;
        private string _httpServerPath = Directory.GetCurrentDirectory() + "\\Web";
        private HTTPServer _httpServer;

        #region Constructor
        public OneHUD()
        {
            InitializeComponent();
        }
        #endregion

        #region Startup Code
        public void Startup()
        {
            _telemetryData = new TelemetryData();
            _timingData = new TimingData();

            lsvPlugins.Items.Clear();

            _plugins = new Dictionary<string, IGame>();
            ICollection<IGame> plugins = PluginLoader<IGame>.LoadPlugins("Plugins");
            if (plugins.Count > 0)
            {
                foreach (var item in plugins)
                {
                    _plugins.Add(item.Name, item);

                    string[] lvText = new string[4];
                    lvText[0] = "";
                    lvText[1] = item.DisplayName;
                    lvText[2] = item.Version;
                    lvText[3] = item.Author;

                    ListViewItem lvItem = new ListViewItem(lvText);

                    Bitmap pluginIcon = item.Icon;

                    if (pluginIcon != null)
                    {
                        imageListPlugins.Images.Add(item.Name, pluginIcon);
                        lvItem.ImageKey = item.Name;
                    }
                    else
                    {
                        lvItem.ImageKey = "missing";
                    }

                    lsvPlugins.Items.Add(lvItem);
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

                Invoke(new Action(() => { Status.Text = _game.DisplayName; }));
                Invoke(new Action(() => { labelStatus.Text = _game.DisplayName; }));

                if (imageListPlugins.Images.ContainsKey(_game.Name))
                {
                    Invoke(new Action(() => { pictureConnected.Image = imageListPlugins.Images[_game.Name]; }));
                }
                _telemetryData.Reset();
                _telemetryData.Game = gameName;
                _telemetryData.Description = _plugins[gameName].DisplayName;
                _gameReaderThread = new Thread(() => GameReaderThread());
                _gameReaderThread.Start();

            }
        }

        private void GameReaderThread()
        {
            _game.Start(_telemetryData, _timingData);
        }

        private void StopGameReaderThread()
        {
            _game.Stop(); // !!!!
            _gameReaderThread.Abort();
            _telemetryData.Reset();
        }

        private void GameClosed(string gameName, EventArgs e)
        {
            StopGameReaderThread();
            _game = null;

            Invoke(new Action(() => { Status.Text = "Waiting ..."; }));
            Invoke(new Action(() => { labelStatus.Text = "Not Connected"; }));
            Invoke(new Action(() => { pictureConnected.Image = null; }));
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
                _httpServerThread = new Thread(() => HTTPServerThread(_telemetryData, _timingData, _ipAddress, _httpServerPort, _httpServerPath, _plugins));
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

        private void HTTPServerThread(TelemetryData telemetryData, TimingData timingData, IPAddress ipAddress, int port, string httpServerPath, Dictionary<string, IGame> plugins)
        {
            _httpServer = new HTTPServer(httpServerPath, port, telemetryData, timingData, ipAddress, plugins);
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

        private void txtPort_TextChanged_1(object sender, EventArgs e)
        {

        }
        #endregion

        private void labelStatus_Click(object sender, EventArgs e)
        {

        }

    }
}
