using System;
using System.Linq;

namespace OneHUDInterface
{
    public interface ITrackRecorder
    {
        /// <summary>
        /// Starts the track recorder.
        /// </summary>
        /// <returns></returns>
        bool Start();

        /// <summary>
        /// Stops the track recorder.
        /// </summary>
        /// <returns></returns>
        bool Stop();

        /// <summary>
        /// Gets the best lap and returns it as a Track
        /// </summary>
        /// <returns></returns>
        Track GetTrack();

    }

}
