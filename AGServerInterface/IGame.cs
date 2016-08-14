using System;
using System.Collections.Generic;
using System.Drawing;
using OneHUDData;

namespace OneHUDInterface
{
    public interface IGame
    {

        string Name { get; }

        string DisplayName { get; }

        List<string> ProcessNames { get; }

        string Version { get; }

        Bitmap Icon { get; }

        string Author { get; }

        bool Start(TelemetryData telemetryData, TimingData timingData);

        bool Stop();
    }
}