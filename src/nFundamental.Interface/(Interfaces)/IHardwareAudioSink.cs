using System;
using Fundamental.Core;

namespace Fundamental.Interface
{
    public interface IHardwareAudioSink : ISynchronizedAudioSink
    {
        /// <summary>
        /// Starts rendering audio.
        /// </summary>
        /// <returns></returns>
        void Start();

        /// <summary>
        /// Stops rendering audio.
        /// </summary>
        /// <returns></returns>
        void Stop();

        /// <summary>
        /// Raised when actual rendering is started.
        /// </summary>
        event EventHandler<EventArgs> Started;

        /// <summary>
        /// Raised when actual rendering is stopped.
        /// </summary>
        event EventHandler<EventArgs> Stopped;
    }
}
