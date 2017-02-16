using System;
using System.Threading;

using Fundamental.Core;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSource : WasapiAudioClient, IHardwareAudioSource
    {
        /// <summary>
        /// The current audio capture client interop
        /// </summary>
        private IWasapiAudioCaptureClientInterop _audioCaptureClientInterop;

        /// <summary>
        /// The maximum buffer under-runs before capture assumes failure and terminates capture process 
        /// </summary>
        private readonly TimeSpan _maxBufferUnderrunTime = TimeSpan.FromSeconds(1);

        /// <summary>
        /// The buffer under-run time
        /// </summary>
        private TimeSpan _bufferUnderrunTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioSource" /> class.
        /// </summary>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="wasapiOptions">The WASAPI options.</param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client inter-operations factory.</param>
        public WasapiAudioSource(IDeviceToken wasapiDeviceToken,
                                 IDeviceInfo deviceInfo,
                                 IOptions<WasapiOptions> wasapiOptions,
                                 IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory) 
            : base(wasapiDeviceToken, deviceInfo, wasapiOptions.Value.AudioCapture, wasapiAudioClientInteropFactory)
        {
        }

        /// <summary>
        /// Reads the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int length)
        {
            return _audioCaptureClientInterop?.Read(buffer, offset, length) ?? 0;
        }

        /// <summary>
        /// Occurs when data available from the source.
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        // Protected methods

        /// <summary>
        /// Called when the instance Initializes.
        /// </summary>
        protected override void InitializeImpl()
        {
            _audioCaptureClientInterop = WasapiClient.GetCaptureClient(); 
        }


        // Private methods

        /// <summary>
        /// Pumps the current audio content audio using a manual sync
        /// </summary>
        /// <param name="pollRate"></param>
        /// <returns></returns>
        protected override bool PumpAudioManunalSync(TimeSpan pollRate)
        {
            Thread.Sleep(pollRate);
            return PumpAudio();
        }

        /// <summary>
        /// Pumps the current audio content audio using hardware sync
        /// </summary>
        /// <param name="latency">The latency.</param>
        /// <returns></returns>
        protected override bool PumpAudioHardwareSync(TimeSpan latency)
        {
            if (HardwareSyncEvent.WaitOne(latency))
            {
                _bufferUnderrunTime = TimeSpan.Zero; // reset under run time
                return PumpAudio();
            }

            _bufferUnderrunTime += latency;

            // Stop the pump if we under-run for too long
            return _bufferUnderrunTime <= _maxBufferUnderrunTime;
        }

        /// <summary>
        /// Pumps content from the device as captured audio.
        /// </summary>
        private bool PumpAudio()
        {
            var captureClientInterop = _audioCaptureClientInterop;

            while (IsRunning)
            {
                _audioCaptureClientInterop.UpdateBuffer();

                var bufferSize = captureClientInterop.GetBufferByteSize();

                if (bufferSize == 0)
                    return true;

                DataAvailable?.Invoke(this, new DataAvailableEventArgs(bufferSize));

                // Drop any remaining frames if they where not consumed from the read method
                captureClientInterop.ReleaseBuffer();
            }

            return false;
        }
    }
}
