using System;

namespace Fundamental.Core
{
    public interface ISynchronizedAudioSource : IAudioSource
    {
        /// <summary>
        /// Occurs when data available from the source.
        /// </summary>
        event EventHandler<DataAvailableEventArgs> DataAvailable;
    }
}
