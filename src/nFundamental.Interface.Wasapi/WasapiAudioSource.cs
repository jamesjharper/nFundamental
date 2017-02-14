using System;
using System.Threading;

using Fundamental.Core;

using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSource : WasapiAudioClient, IHardwareAudioSource
    {
        // Dependents

        /// <summary>
        /// The maximum buffer under-runs before capture assumes failure and terminates capture process 
        /// </summary>
        private const int MaxBufferUnderruns = 2;

        /// <summary>
        /// The WASAPI options
        /// </summary>
        private readonly IOptions<WasapiOptions> _wasapiOptions;

        // Internal fields

        /// <summary>
        /// The currnent audio capture client interop
        /// </summary>
        private IWasapiAudioCaptureClientInterop _audioCaptureClientInterop;

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
            : base(wasapiDeviceToken, deviceInfo, wasapiAudioClientInteropFactory)
        {
            _wasapiOptions = wasapiOptions;
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
        /// Gets the device access mode.
        /// </summary>
        /// <value>
        /// The device access.
        /// </value>
        protected override AudioClientShareMode DeviceAccessMode => _wasapiOptions.Value.AudioCapture.DeviceAccess.ConvertToWasapiAudioClientShareMode();

        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        /// <value>
        /// The length of the buffer.
        /// </value>
        protected override TimeSpan ManualSyncLatency => _wasapiOptions.Value.AudioCapture.ManualSyncLatency;

        /// <summary>
        /// Gets a value indicating whether [use hardware synchronize].
        /// </summary>
        /// <value>
        /// <c>true</c> if [use hardware synchronize]; otherwise, <c>false</c>.
        /// </value>
        protected override bool UseHardwareSync => _wasapiOptions.Value.AudioCapture.UseHardwareSync;

        /// <summary>
        /// Gets or sets a value indicating whether to prefer device native format over WASAPI up sampled format.
        /// </summary>
        /// <value>
        /// <c>true</c> if [prefer device native format]; otherwise, <c>false</c>.
        /// </value>
        protected override bool PreferDeviceNativeFormat => _wasapiOptions.Value.AudioCapture.PreferDeviceNativeFormat;

        /// <summary>
        /// Called when the instance Initializes.
        /// </summary>
        protected override void InitializeImpl()
        {
            _audioCaptureClientInterop = AudioClientInterop.GetCaptureClient(); 
        }


        // Private methods

        /// <summary>
        /// Pumps the current audio content audio.
        /// </summary>
        private void PumpAudio(IWasapiAudioCaptureClientInterop captureClientInterop)
        {
            captureClientInterop.UpdateBuffer();
            var bufferSize = captureClientInterop.GetBufferByteSize();

            if (bufferSize == 0)
                return;

            DataAvailable?.Invoke(this, new DataAvailableEventArgs(bufferSize));

            // Drop any remaining frames if they where not consumed from the read method
            captureClientInterop.ReleaseBuffer();
        }

       


        /// <summary>
        /// Runs the audio pump using hardware interrupt audio synchronization
        /// </summary>
        protected override void HardwareSyncAudioPump()
        {
            // Save out the current capture client, just to be sure we are 
            // always operating on the same instance. 
            var latencyCaculator = GetAudioFormatLatencyCalculator();
            var bufferSize = AudioClientInterop.GetBufferSize();
            var latency = latencyCaculator.FramesToLatency(bufferSize);
            var bufferUnderrunCount = 0;

            while (IsRunning)
            {
                if (bufferUnderrunCount > MaxBufferUnderruns)
                    break;

                if (!HardwareSyncEvent.WaitOne(latency))
                {
                    bufferUnderrunCount++;
                    continue;
                }

                PumpAudio(_audioCaptureClientInterop);
            }
        }

        /// <summary>
        /// Runs the audio pump using Manual audio synchronization
        /// </summary>
        protected override void ManualSyncAudioPump()
        {
            // Save out the current capture client, just to be sure we are 
            // always operating on the same instance. 
            var latencyCaculator = GetAudioFormatLatencyCalculator();
            var bufferSize = AudioClientInterop.GetBufferSize();
            var latency = latencyCaculator.FramesToLatency(bufferSize);
            var pollRate = TimeSpan.FromTicks(latency.Ticks / 2);

            while (IsRunning)
            {
                Thread.Sleep(pollRate);
                PumpAudio(_audioCaptureClientInterop);
            }
        }
    }
}
