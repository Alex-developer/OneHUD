using System;
using AGData;

namespace AGServerInterface
{
    public interface IGame
    {
        event EventHandler GameEvent;

        string Name { get; }
        string DisplayName { get; }
        string[] ProcessNames { get;  }

        bool Start(TelemetryData telemetryData);
        bool Stop();
    }
}
