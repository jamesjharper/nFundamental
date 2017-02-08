using System;
using Fundamental.Core;

namespace Fundamental.Interface
{
    public interface IAudioSource : IFormatSetable, IFormatGetable, IFormatNegotiable, IFormatChangable
    {

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
        /// Raised when audio data is received from the source.
        /// </summary>
        event EventHandler<SourceDataReceivedEventArgs> DataRecived;
    }
}
