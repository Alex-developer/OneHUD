using System;
using System.Linq;
using System.Collections.Generic;
using OneHUDData.Vehicle;
using OneHUDData.Timing;
using OneHUDData.Players;

namespace OneHUDData
{
    public class TelemetryData
    {
        public string Game { get; set; }
        public string Description { get; set; }
        public Engine Engine { get; set; }
        public Chassis Chassis { get; set; }
        public Car Car { get; set; }
        public Times Timing { get; set; }

        private List<Player> _players;

        private readonly Object _lock = new Object();

        public TelemetryData()
        {
            Reset();
            Game = null;
        }

        public List<Player> Players
        {
            get
            {
                return _players;
            }
            set
            {
                _players = value;
            }
        }

        public void Reset()
        {
            lock (_lock)
            {             
                Engine = new Engine();
                Chassis = new Chassis();
                Car = new Car();
                Timing = new Times();
                Players = new List<Player>();
            }
        }

        public void ResetPlayers()
        {
            Players.Clear();
        }

        public void AddPlayer(float x, float y, float z, bool isMe)
        {
            Players.Add(new Player() { X = x, Y = y, Z = z, IsMe = isMe });
        }
    }

}
