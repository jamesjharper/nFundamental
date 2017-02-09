using System;
using Fundamental.Core;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSource : WasapiAudioClient
    {
        // Dependents

        /// <summary>
        /// The WASAPI options
        /// </summary>
        private readonly IOptions<WasapiOptions> _wasapiOptions;

        // Internal fields

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioSource" /> class.
        /// </summary>
        /// <param name="wasapiOptions">The WASAPI options.</param>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client inter-operations factory.</param>
        public WasapiAudioSource(IOptions<WasapiOptions> wasapiOptions,
                                 IDeviceToken wasapiDeviceToken, 
                                 IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory) : base(wasapiDeviceToken, wasapiAudioClientInteropFactory)
        {
            _wasapiOptions = wasapiOptions;
        }


        /// <summary>
        /// Gets the device access mode.
        /// </summary>
        /// <value>
        /// The device access.
        /// </value>
        protected override AudioClientShareMode DeviceAccessMode => _wasapiOptions.Value.AudioSource.DeviceAccess.ConvertToWasapiAudioClientShareMode();

        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        /// <value>
        /// The length of the buffer.
        /// </value>
        protected override TimeSpan BufferLength => _wasapiOptions.Value.AudioSource.BufferLength;

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
        public event EventHandler<EventArgs> FormatChanged;
        public event EventHandler<SourceDataReceivedEventArgs> DataRecived;

        // Private methods

        protected virtual void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDataRecived(SourceDataReceivedEventArgs e)
        {
            DataRecived?.Invoke(this, e);
        }

        protected virtual void OnFormatChanged()
        {
            FormatChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
