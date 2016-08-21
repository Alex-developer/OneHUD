using System;
using System.Collections.Generic;
using System.Drawing;
using OneHUDData;
using OneHUDInterface.TrackInfo;
using OneHUDInterface.TrackRecorder;

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

        string URL { get; }

        bool Start(TelemetryData telemetryData, TimingData timingData);

        bool Stop();

        /// <summary>
        /// Supports the track recorder.
        /// </summary>
        /// <returns></returns>
        bool SupportsTrackRecorder();

        /// <summary>
        /// Starts the track recorder.
        /// </summary>
        /// <returns></returns>
        bool StartTrackRecorder();

        /// <summary>
        /// Stops the track recorder.
        /// </summary>
        /// <returns></returns>
        TrackRecording StopTrackRecorder();

        /// <summary>
        /// Gets the track recording.
        /// </summary>
        /// <returns></returns>
        TrackRecording GetTrackRecording();

        /// <summary>
        /// Gets the best lap and returns it as a Track
        /// </summary>
        /// <returns></returns>
        Track GetTrack();
    }
}