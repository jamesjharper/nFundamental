using System;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Core.Memory;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using System.Reflection;

namespace Fundamental.Interface.Wasapi.Internal
{

    

    /// <summary>
    /// An Anti corruption layer for the IAudioClient COM object. 
    /// Interaction with IAudioClient can require pointer Manipulation, which is a little ugly in managed languages 
    /// </summary>
    public class WasapiAudioClient
    {
        /// <summary>
        /// The audio client
        /// </summary>
        private readonly IAudioClient _audioClient;

        /// <summary>
        /// The wave format converter
        /// </summary>
        private readonly IAudioFormatConverter<WaveFormat> _waveFormatConverter;


        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioClient"/> class.
        /// </summary>
        /// <param name="audioClient">The audio client.</param>
        /// <param name="waveFormatConverter">The wave format converter.</param>
        public WasapiAudioClient(IAudioClient audioClient,
                                 IAudioFormatConverter<WaveFormat> waveFormatConverter)
        {
            _audioClient = audioClient;
            _waveFormatConverter = waveFormatConverter;
        }



        /// <summary>
        /// Gets the number of audio frames that the buffer can hold.
        /// </summary>
        /// <returns></returns>
        public int GetBufferSize()
        {
            uint outInt;
            _audioClient.GetBufferSize(out outInt).ThrowIfFailed();
            return checked ((int)outInt);
        }


        /// <summary>
        /// Gets the maximum latency for the current stream and can be called any time after the stream has been initialized.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetStreamLatency()
        {
            ulong outuLong;
            _audioClient.GetStreamLatency(out outuLong).ThrowIfFailed();
            return TimeSpan.FromTicks(checked((long)outuLong));
        }

        /// <summary>
        /// Gets the number of frames of padding in the endpoint buffer.
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPadding()
        {
            uint outInt;
            _audioClient.GetCurrentPadding(out outInt).ThrowIfFailed();
            return checked((int)outInt);
        }

        /// <summary>
        /// The IsFormatSupported method indicates whether the audio endpoint device supports a particular stream format
        /// </summary>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="format">The format.</param>
        /// <param name="closestMatch">The closest match.</param>
        /// <returns>
        ///   <c>true</c> if [is format supported] [the specified share mode]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFormatSupported(AudioClientShareMode shareMode, AudioFormat format, out IAudioFormat closestMatch)
        {
            var waveFormatEx = _waveFormatConverter.Convert(format);

            using (var pWaveFormatEx = CoTaskMemPtr.CopyToPtr(waveFormatEx.ToBytes()))
            using (var ppClosestMatch = CoTaskMemPtr.Alloc(IntPtr.Size))
            {
                // ReSharper disable once RedundantAssignment
                var ppClosestMatchOut = ppClosestMatch.Ptr;

                var hresult = _audioClient.IsFormatSupported(shareMode, pWaveFormatEx, out ppClosestMatchOut);

                // Try reading the closest match from the pointer to the pointer
                closestMatch = ReadAudioFormat(ppClosestMatch);

                // Succeeded with a closest match to the specified format.
                if (hresult == HResult.E_FAIL)
                    return false;

                //  Succeeded but the specified format is not supported in exclusive mode.
                if (hresult == HResult.AUDCLNT_E_UNSUPPORTED_FORMAT)
                    return shareMode != AudioClientShareMode.Exclusive;


                hresult.ThrowIfFailed();
                return hresult == HResult.S_OK;
            }
        }


        /// <summary>
        /// The GetMixFormat method retrieves the stream format that the audio engine uses for its internal processing of shared-mode streams.
        /// </summary>
        public IAudioFormat GetMixFormat()
        {
            using (var ppFormat = CoTaskMemPtr.Alloc(IntPtr.Size))
            {
                // ReSharper disable once RedundantAssignment
                var ppFormatOut = ppFormat.Ptr;

                _audioClient.GetMixFormat(out ppFormatOut).ThrowIfFailed();

                // Try reading the format from the pointer to the pointer
                return ReadAudioFormat(ppFormat);

            }
        }


        /// <summary>
        /// The GetMixFormat method retrieves the stream format that the audio engine uses for its internal processing of shared-mode streams.
        /// </summary>
        /// <returns></returns>
        public DevicePeriod GetDevicePeriod()
        {
            ulong phnsDefaultDevicePeriod;
            ulong phnsMinimumDevicePeriod;

            _audioClient.GetDevicePeriod(out phnsDefaultDevicePeriod, out phnsMinimumDevicePeriod).ThrowIfFailed();

            return new DevicePeriod
            (
                TimeSpan.FromTicks(checked((long) phnsDefaultDevicePeriod)),
                TimeSpan.FromTicks(checked((long) phnsDefaultDevicePeriod))
            );
        }




        /// <summary>
        /// Initializes the audio stream.
        /// </summary>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="streamFlags">The stream flags.</param>
        /// <param name="bufferDuration">Duration of the buffer.</param>
        /// <param name="devicePeriod">The device period.</param>
        /// <param name="format">The format.</param>
        public void Initialize(AudioClientShareMode shareMode,
                               AudioClientStreamFlags streamFlags,
                               TimeSpan bufferDuration,
                               TimeSpan devicePeriod,
                               AudioFormat format)
        {
            Initialize(shareMode, streamFlags, bufferDuration, devicePeriod, format, Guid.Empty);
        }

        /// <summary>
        /// Initializes the audio stream.
        /// </summary>
        public void Initialize(AudioClientShareMode shareMode,
                               AudioClientStreamFlags streamFlags,
                               TimeSpan bufferDuration,
                               TimeSpan devicePeriod,
                               AudioFormat format,
                               Guid sessionId)
        {
            var waveFormatEx = _waveFormatConverter.Convert(format);

            var bufferDurationTicks = checked((uint)bufferDuration.Ticks);
            var devicePeriodTicks = checked((uint)devicePeriod.Ticks);

            using (var pWaveFormatEx = CoTaskMemPtr.CopyToPtr(waveFormatEx.ToBytes()))
            {
                _audioClient.Initialize(shareMode, streamFlags, bufferDurationTicks, devicePeriodTicks, pWaveFormatEx, sessionId).ThrowIfFailed();
            }
        }


        /// <summary>
        /// Starts the audio stream.
        /// </summary>
        public void Start()
        {
            _audioClient.Start().ThrowIfFailed();
        }

        /// <summary>
        /// Stops the audio stream.
        /// </summary>
        public void Stop()
        {
            _audioClient.Start().ThrowIfFailed();
        }


        /// <summary>
        /// Resets the audio stream.
        /// </summary>
        public void Reset()
        {
            _audioClient.Reset().ThrowIfFailed();
        }


        /// <summary>
        /// Sets the event handle.
        /// </summary>
        /// <param name="handle">The handle.</param>
        public void SetEventHandle(IntPtr handle)
        {
            _audioClient.SetEventHandle(handle).ThrowIfFailed();
        }



        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() where T : class
        {
            var interfaceId = typeof(T).GetTypeInfo().GUID;
            var result = GetService(interfaceId);
            return ComObject.QuearyInterface<T>(result);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="interfaceId">The interface identifier.</param>
        /// <returns></returns>
        public object GetService(Guid interfaceId)
        {
            object outObject;
            _audioClient.GetService(interfaceId, out outObject).ThrowIfFailed();
            return outObject;
        }

        // Private Methods

        private IAudioFormat ReadAudioFormat(NativePtr pp)
        {
            if (pp == NativePtr.Null)
                return null;

            using (var p = pp.AttachPointerAtPointer())
            {
                if (p != NativePtr.Null)
                    return null;

                var formatEx = WaveFormatEx.FromPointer(p);
                return _waveFormatConverter.Convert(formatEx);
            }
        }

    }
}
