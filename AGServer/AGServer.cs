using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using AGServer.Plugin;
using AGServer.Processes;
using AGServerInterface;
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

        #region Constructor
        public AGServer()
        {
            InitializeComponent();
        }
        #endregion

        #region Startuo Code
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

    }
}
