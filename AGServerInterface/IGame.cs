using System;
using System.Collections.Generic;
using System.Drawing;
using OneHUDData;
using OneHUDData.TrackInfo;
using OneHUDData.TrackRecorder;
using OneHUDData.AnalysisData;

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

        bool Start(TelemetryData telemetryData, TimingData timingData, AnalysisManager analysisData);

        bool Stop();

        PageTypes Supports { get; }
        ConnectionType ConnectionType { get; }

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

        /// <summary>
        /// Saves the track.
        /// </summary>
        /// <param name="lap">The lap.</param>
        bool SaveTrack(int driver, int lap);

        /// <summary>
        /// Loads the track.
        /// </summary>
        /// <returns></returns>
        Track LoadTrack();

        void ShowOptions();

    }

    public enum ConnectionType
    {
        BYPROCESS,
        MANUAL,
        BOTH
    }

    public enum PageTypes
    {
        None = 0,
        Dash = 1,
        Timing = 2,
        TrackMap = 4,
        Telemetry = 8,

        ServerOptions = 16384
    }
}