using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using OneHUDInterface;

namespace OneHUD.Processes
{
    public class ProcessMonitor
    {

        public event GameLoaded GameLoadedEvent;
        public EventArgs e = null;
        public delegate void GameLoaded(string game, EventArgs e);

        public event GameClosed GameClosedEvent;
        public delegate void GameClosed(string game, EventArgs e);

        private bool _monitoring = false;
        private Thread _gameMonitorThread;
        readonly Dictionary<string, IGame> _plugins;

        private string _runningGameProcess;

        public ProcessMonitor(Dictionary<string, IGame> plugins)
        {
            _plugins = plugins;
        }

        public bool StartProcessMonitor() {
            _monitoring = true;
            _gameMonitorThread = new Thread(new ThreadStart(GameMonitorThread));
            _gameMonitorThread.Start();
            return true;
        }

        public bool StopProcessMonitor()
        {
            _monitoring = false;
            _gameMonitorThread.Abort();
            return true;
        }


        public void GameMonitorThread()
        {
            while (_monitoring)
            {
                if (_runningGameProcess == null)
                {
                    foreach (var key in _plugins)
                    {
                        IGame game = key.Value;

                        for (int i = 0; i < game.ProcessNames.Count; ++i)
                        {
                            Process[] processesByName = Process.GetProcessesByName(game.ProcessNames[i]);
                            if (processesByName.Length > 0)
                            {
                                _runningGameProcess = game.ProcessNames[i];
                                if (GameLoadedEvent != null)
                                {
                                    GameLoadedEvent(game.Name, e);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Process[] processesByName = Process.GetProcessesByName(_runningGameProcess);
                    if (processesByName.Length == 0)
                    {
                        _runningGameProcess = null;
                        if (GameClosedEvent != null)
                        {
                            GameClosedEvent("", e);
                        }
                    }

                }

                Thread.Sleep(500);
            }

        }
    }
}
