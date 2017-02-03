using System;
using Fundamental.Core;

namespace Fundamental.Interface
{
    public interface IAudioSource
    {
        /// <summary>
        /// Sets the format.
        /// </summary>
        void SetFormat(IAudioFormat audioFormat);

        /// <summary>
        /// Gets the current format.
        /// </summary>
        IAudioFormat GetFormat();

        /// <summary>
        /// Starts capturing audio.
        /// </summary>
        /// <returns></returns>
        void Start();

        /// <summary>
        /// Stops capturing audio.
        /// </summary>
        /// <returns></returns>
        void Stop();

        /// <summary>
        /// Raised when actual capturing is started.
        /// </summary>
        event EventHandler<EventArgs> Started;

        /// <summary>
        /// Raised when actual capturing is stopped.
        /// </summary>
        event EventHandler<EventArgs> Stopped;

        /// <summary>
        /// Raised when source format changes.
        /// </summary>
        event EventHandler<EventArgs> FormatChanged;

        /// <summary>
        /// Raised when audio data is received from the source.
        /// </summary>
        event EventHandler<SourceDataReceivedEventArgs> DataRecived;
    }
}
