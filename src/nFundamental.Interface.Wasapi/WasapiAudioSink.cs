using System;
using System.Threading;

using Fundamental.Core;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSink : WasapiAudioClient, IHardwareAudioSink
    {

        /// <summary>
        /// The current audio capture client interop
        /// </summary>
        private IWasapiAudioRenderClientInterop _audioRenderClientInterop;

        /// <summary>
        /// The maximum buffer under-runs before capture assumes failure and terminates render process 
        /// </summary>
        private readonly TimeSpan _maxBufferUnderrunTime = TimeSpan.FromSeconds(1);

        /// <summary>
        /// The buffer under-run time
        /// </summary>
        private TimeSpan _bufferUnderrunTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioSink" /> class.
        /// </summary>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="wasapiOptions">The WASAPI options.</param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client inter-operation factory.</param>
        public WasapiAudioSink(IDeviceToken wasapiDeviceToken,
                               IDeviceInfo deviceInfo,
                               IOptions<WasapiOptions> wasapiOptions,
                               IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory) 
            : base(wasapiDeviceToken,  deviceInfo, wasapiOptions.Value.AudioRender, wasapiAudioClientInteropFactory)
        {
        }


        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Write(byte[] buffer, int offset, int length)
        {
            return _audioRenderClientInterop?.Write(buffer, offset, length) ?? 0;
        }

        /// <summary>
        /// Occurs when data requested from the sink.
        /// </summary>
        public event EventHandler<DataRequestedEventArgs> DataRequested;

        // protected methods

        /// <summary>
        /// Called when the instance Initializes.
        /// </summary>
        protected override void InitializeImpl()
        {
            _audioRenderClientInterop = WasapiClient.GetRenderClient();
        }

        /// <summary>
        /// Primes the audio pump so that its ready for pumping.
        /// </summary>
        protected override void PrimeAudioPrime()
        {
            base.PrimeAudioPrime();

            // We fill the render buffers up before we start. this way
            // the render client with have audio ready for it the instance it starts
            PumpAudio();
        }

        /// <summary>
        /// Pumps the current audio content audio using a manual sync
        /// </summary>
        /// <param name="pollRate"></param>
        /// <returns></returns>
        protected override bool PumpAudioManunalSync(TimeSpan pollRate)
        {
            // Aways pump before waiting for audio to be processed
            if (!PumpAudio())
                return false;

            Thread.Sleep(pollRate);
            return true;
        }

        /// <summary>
        /// Pumps the current audio content audio using hardware sync
        /// </summary>
        /// <param name="latency">The latency.</param>
        /// <returns></returns>
        protected override bool PumpAudioHardwareSync(TimeSpan latency)
        {
            // Aways pump before waiting for audio to be processed
            if (!PumpAudio())
                return false;

   
            if (HardwareSyncEvent.WaitOne(latency))
            {
                _bufferUnderrunTime = TimeSpan.Zero; // reset under run time    
                return true;
            }

            _bufferUnderrunTime += latency;

            // Stop the pump if we under-run for too long
            return _bufferUnderrunTime <= _maxBufferUnderrunTime;
        }

        // private methods

        /// <summary>
        /// Pumps content for sending to the device for audio rendering.
        /// </summary>
        private bool PumpAudio()
        {
            while (IsRunning)
            {
                var bufferSize = _audioRenderClientInterop.GetFreeBufferByteSize();

                if (bufferSize == 0)
                {
                    _audioRenderClientInterop.ReleaseBuffer();
                    return true;
                }

                // If this call takes too long, this will result in stuttered audio 
                DataRequested?.Invoke(this, new DataRequestedEventArgs(bufferSize));

                // Drop any remaining frames if they where not consumed from the read method
                _audioRenderClientInterop.ReleaseBuffer();
            }

            return true;
        }
    }
}
