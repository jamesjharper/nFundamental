using System;
using Fundamental.Core.AudioFormats;
using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Internal
{
    public interface IWasapiAudioClient
    {
        /// <summary>
        /// Gets the number of audio frames that the buffer can hold.
        /// </summary>
        /// <returns></returns>
        int GetBufferSize();

        /// <summary>
        /// Gets the number of frames of padding in the endpoint buffer.
        /// </summary>
        /// <returns></returns>
        int GetCurrentPadding();

        /// <summary>
        /// The GetMixFormat method retrieves the stream format that the audio engine uses for its internal processing of shared-mode streams.
        /// </summary>
        /// <returns></returns>
        DevicePeriod GetDevicePeriod();

        /// <summary>
        /// The GetMixFormat method retrieves the stream format that the audio engine uses for its internal processing of shared-mode streams.
        /// </summary>
        IAudioFormat GetMixFormat();

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="interfaceId">The interface identifier.</param>
        /// <returns></returns>
        object GetService(Guid interfaceId);

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>() where T : class;

        /// <summary>
        /// Gets the stream latency.
        /// </summary>
        /// <returns></returns>
        TimeSpan GetStreamLatency();

        /// <summary>
        /// Initializes the audio stream.
        /// </summary>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="streamFlags">The stream flags.</param>
        /// <param name="bufferDuration">Duration of the buffer.</param>
        /// <param name="devicePeriod">The device period.</param>
        /// <param name="format">The format.</param>
        void Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, TimeSpan bufferDuration, TimeSpan devicePeriod, AudioFormat format);

        /// <summary>
        /// Initializes the audio stream.
        /// </summary>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="streamFlags">The stream flags.</param>
        /// <param name="bufferDuration">Duration of the buffer.</param>
        /// <param name="devicePeriod">The device period.</param>
        /// <param name="format">The format.</param>
        /// <param name="sessionId">The session identifier.</param>
        void Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, TimeSpan bufferDuration, TimeSpan devicePeriod, AudioFormat format, Guid sessionId);

        /// <summary>
        /// The IsFormatSupported method indicates whether the audio endpoint device supports a particular stream format
        /// </summary>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="format">The format.</param>
        /// <param name="closestMatch">The closest match.</param>
        /// <returns>
        ///   <c>true</c> if [is format supported] [the specified share mode]; otherwise, <c>false</c>.
        /// </returns>
        bool IsFormatSupported(AudioClientShareMode shareMode, IAudioFormat format, out IAudioFormat closestMatch);

        /// <summary>
        /// Resets the audio stream.
        /// </summary>
        void Reset();

        /// <summary>
        /// Sets the event handle.
        /// </summary>
        /// <param name="handle">The handle.</param>
        void SetEventHandle(IntPtr handle);

        /// <summary>
        /// Starts the audio stream.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the audio stream.
        /// </summary>
        void Stop();
    }
}