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
            : base(wasapiDeviceToken, deviceInfo, wasapiAudioClientInteropFactory)
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

        /// <summary>
        /// Gets or sets a value indicating whether to prefer device native format over WASAPI up sampled format.
        /// </summary>
        /// <value>
        /// <c>true</c> if [prefer device native format]; otherwise, <c>false</c>.
        /// </value>
        protected override bool PreferDeviceNativeFormat => _wasapiOptions.Value.AudioCapture.PreferDeviceNativeFormat;

        protected override void HardwareSyncAudioPump()
        {
            throw new NotImplementedException();
        }

        protected override void ManualSyncAudioPump()
        {
            throw new NotImplementedException();
        }


        public int Write(byte[] buffer, int offset, int length)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DataRequestedEventArgs> DataRequested;

       
    }
}
