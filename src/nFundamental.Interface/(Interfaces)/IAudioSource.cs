using System;
using System.Threading.Tasks;

namespace Fundamental.Interface
{
    public interface IAudioSource
    {
        /// <summary>
        /// Attempts to create a handshakes between the audio source and 
        /// the specified format negotiator.
        /// </summary>
        /// <param name="formatNegotiator">The format negotiator.</param>
        Task Handshake(IFormatNegotiator formatNegotiator);

        /// <summary>
        /// Starts capturing audio.
        /// </summary>
        /// <returns></returns>
        Task Start();

        /// <summary>
        /// Stops capturing audio.
        /// </summary>
        /// <returns></returns>
        Task Stop();

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
