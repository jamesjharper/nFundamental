using System;

namespace Fundamental.Core
{
    public interface ISynchronizedAudioSink : IAudioSink
    {

        /// <summary>
        /// Occurs when data requested from the sink.
        /// </summary>
        event EventHandler<DataRequestedEventArgs> DataRequested;
    }
}
