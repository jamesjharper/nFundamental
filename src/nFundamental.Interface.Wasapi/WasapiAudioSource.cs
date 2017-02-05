using System;
using System.Threading.Tasks;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSource : IAudioSource
    {
        /// <summary>
        /// The WASAPI device token
        /// </summary>
        private readonly WasapiDeviceToken _wasapiDeviceToken;

        /// <summary>
        /// The WASAPI audio client factory
        /// </summary>
        private readonly IWasapiAudioClientFactory _wasapiAudioClientFactory;

        /// <summary>
        /// The WASAPI options
        /// </summary>
        private readonly IOptions<WasapiOptions> _wasapiOptions;

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        private WasapiOptions Options => _wasapiOptions.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioSource"/> class.
        /// </summary>
        /// <param name="wasapiOptions"></param>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="wasapiAudioClientFactory"></param>
        public WasapiAudioSource(IOptions<WasapiOptions> wasapiOptions, 
                                 WasapiDeviceToken wasapiDeviceToken, 
                                 IWasapiAudioClientFactory wasapiAudioClientFactory)
        {
            _wasapiOptions = wasapiOptions;
            _wasapiDeviceToken = wasapiDeviceToken;
            _wasapiAudioClientFactory = wasapiAudioClientFactory;
        }


        public void SetFormat(IAudioFormat audioFormat)
        {
            throw new NotImplementedException();
        }

        public IAudioFormat GetFormat()
        {
            throw new NotImplementedException();
        }

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
