using System;
using System.Collections.Generic;
using System.Linq;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSource : IAudioSource
    {
        // Dependents

        /// <summary>
        /// The WASAPI device token
        /// </summary>
        private readonly IDeviceToken _wasapiDeviceToken;

        /// <summary>
        /// The WASAPI audio client factory
        /// </summary>
        private readonly IWasapiAudioClientFactory _wasapiAudioClientFactory;

        /// <summary>
        /// The WASAPI options
        /// </summary>
        private readonly IOptions<WasapiOptions> _wasapiOptions;

        /// <summary>
        /// The wave format converter used for converting WAVEFORMATEX objects in to readable structures
        /// </summary>
        private readonly IAudioFormatConverter<WaveFormat> _waveFormatConverter;

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        private WasapiOptions Options => _wasapiOptions.Value;

        // Internal fields

        /// <summary>
        /// The is initialize flag
        /// </summary>
        private bool _isInitialize;

        /// <summary>
        /// The audio client
        /// </summary>
        private IWasapiAudioClient _audioClient;

        /// <summary>
        /// The audio format
        /// Protected so we can get access to this value in test fixtures
        /// </summary>
        protected virtual IAudioFormat DesiredAudioFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioSource"/> class.
        /// </summary>
        /// <param name="wasapiOptions"></param>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="wasapiAudioClientFactory"></param>
        /// <param name="waveFormatConverter"></param>
        public WasapiAudioSource(IOptions<WasapiOptions> wasapiOptions,
                                 IDeviceToken wasapiDeviceToken, 
                                 IWasapiAudioClientFactory wasapiAudioClientFactory,
                                 IAudioFormatConverter<WaveFormat> waveFormatConverter)
        {
            _wasapiOptions = wasapiOptions;
            _wasapiDeviceToken = wasapiDeviceToken;
            _wasapiAudioClientFactory = wasapiAudioClientFactory;
            _waveFormatConverter = waveFormatConverter;
        }


        /// <summary>
        /// Gets the audio client.
        /// </summary>
        /// <value>
        /// The audio client.
        /// </value>
        protected virtual IWasapiAudioClient AudioClient => _audioClient ?? (_audioClient = FactoryAudioClient());

        /// <summary>
        /// Gets the device access mode.
        /// </summary>
        /// <value>
        /// The device access.
        /// </value>
        private AudioClientShareMode WasapiDeviceAccess => _wasapiOptions.Value.AudioSource.DeviceAccess.ConvertToWasapiAudioClientShareMode();

        /// <summary>
        /// Determines whether a given format is supported
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <returns>
        /// <c>true</c> if [is audio format supported] [the specified audio format]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAudioFormatSupported(IAudioFormat audioFormat)
        {
            IEnumerable<IAudioFormat> closestMatchingFormats;
            return IsAudioFormatSupported(audioFormat, out closestMatchingFormats);
        }

        /// <summary>
        /// Determines whether a given format is supported and returns a list of alternatives
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="closestMatchingFormats">The closest matching formats.</param>
        /// <returns>
        /// <c>true</c> if [is audio format supported] [the specified audio format]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAudioFormatSupported(IAudioFormat audioFormat, out IEnumerable<IAudioFormat> closestMatchingFormats)
        {
            IAudioFormat outFormat;
            var result = IsAudioFormatSupported(audioFormat, out outFormat);
            closestMatchingFormats = outFormat != null? new [] {outFormat} : new IAudioFormat[] {  };
            return result;
        }

        /// <summary>
        /// Determines whether a given format is supported and return an alternative
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="closestMatchingFormat">The closest matching format.</param>
        /// <returns>
        ///   <c>true</c> if [is audio format supported] [the specified audio format]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAudioFormatSupported(IAudioFormat audioFormat, out IAudioFormat closestMatchingFormat)
        {
            return AudioClient.IsFormatSupported(WasapiDeviceAccess, audioFormat, out closestMatchingFormat);
        }

        /// <summary>
        /// Suggests a format to use.
        /// This may return, none, one or many
        /// </summary>
        /// <param name="dontSuggestTheseFormats">The don't suggest these formats.</param>
        /// <returns></returns>
        public IEnumerable<IAudioFormat> SuggestFormats(params IAudioFormat[] dontSuggestTheseFormats)
        {
            return SuggestFormats().Where(x => !dontSuggestTheseFormats.Contains(x));
        }

        /// <summary>
        /// Suggests the formats.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAudioFormat> SuggestFormats()
        {     
            var mixerFormat = AudioClient.GetMixFormat();
            IEnumerable<IAudioFormat> closestMatchingFormats;

            // yield the mixer format, if it was not in the "don't suggest these formats" list
            if (IsAudioFormatSupported(mixerFormat, out closestMatchingFormats))
            {
                 yield return mixerFormat;
            }
            else
            {
                foreach (var match in closestMatchingFormats)
                    yield return match;
            }

            // NOTE:
            // Me might be able to parse the device info to try finding driver information such as the engine format 
            // Might be worth while consideration in the future, as in shared mode WASAPI always upsamples to 32bit float
            // which can be a waist.

            // Make sure to use "yield return" that way we can keep 
            // the resolution lazy instead of resolving an entire list of formats when we only need on
        }

        /// <summary>
        /// Sets the format.
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <exception cref="Fundamental.Core.FormatNotSupportedException">Target device does not support the given format</exception>
        public void SetFormat(IAudioFormat audioFormat)
        {
            if (!IsAudioFormatSupported(audioFormat))
                throw new FormatNotSupportedException("Target device does not support the given format");
            DesiredAudioFormat = audioFormat;
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <returns></returns>
        public IAudioFormat GetFormat()
        {
            // If no format has been set, we use the default format
            return GetDesiredFormat() ?? GetDefaultFormat();
        }

        /// <summary>
        /// Gets the desired format.
        /// </summary>
        /// <returns></returns>
        public IAudioFormat GetDesiredFormat()
        {
            return DesiredAudioFormat;
        }

        /// <summary>
        /// Gets the default format.
        /// </summary>
        /// <returns></returns>
        public IAudioFormat GetDefaultFormat()
        {
            return SuggestFormats().FirstOrDefault();
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

        public void EnsureIsInitialize()
        {
            if (_isInitialize)
                return;

            Initialize();
            _isInitialize = true;
        }


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            _audioClient = _wasapiAudioClientFactory.FactoryAudioClient(_wasapiDeviceToken);

          //  _audioClient.Initialize()
        }

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


        /// <summary>
        /// Gets the audio client.
        /// </summary>
        /// <returns></returns>
        protected virtual IWasapiAudioClient FactoryAudioClient() => _wasapiAudioClientFactory.FactoryAudioClient(_wasapiDeviceToken);


    }
}
