using System;
using Fundamental.Core;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSink : WasapiAudioClient, IHardwareAudioSink
    {
        /// <summary>
        /// The WASAPI options
        /// </summary>
        private readonly IOptions<WasapiOptions> _wasapiOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioSink"/> class.
        /// </summary>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="wasapiOptions"></param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client inter-operation factory.</param>
        public WasapiAudioSink(IDeviceToken wasapiDeviceToken,
                               IOptions<WasapiOptions> wasapiOptions,
                               IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory) 
            : base(wasapiDeviceToken, wasapiAudioClientInteropFactory)
        {
            _wasapiOptions = wasapiOptions;
        }

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


        public int Write(byte[] buffer, int offset, int length)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DataRequestedEventArgs> DataRequested;
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Started;
        public event EventHandler<EventArgs> Stopped;
    }
}
