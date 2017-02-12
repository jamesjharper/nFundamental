using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi
{
    public abstract class WasapiAudioClient : 
        IFormatGetable, 
        IFormatSetable,
        IIsFormatSupported,
        IFormatChangeNotifiable
    {
        // Dependents

        /// <summary>
        /// The WASAPI device token
        /// </summary>
        private readonly IDeviceToken _wasapiDeviceToken;

        /// <summary>
        /// The WASAPI audio client factory
        /// </summary>
        private readonly IWasapiAudioClientInteropFactory _wasapiAudioClientInteropFactory;

        // Internal fields

        /// <summary>
        /// The is initialize flag
        /// </summary>
        private int _isInitialize;

        /// <summary>
        /// The audio client
        /// </summary>
        private IWasapiAudioClientInterop _audioClientInterop;

        /// <summary>
        /// The audio format
        /// Protected so we can get access to this value in test fixtures
        /// </summary>
        protected virtual IAudioFormat DesiredAudioFormat { get; set; }


        /// <summary>
        /// Gets a value indicating whether [supports event handle].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports event handle]; otherwise, <c>false</c>.
        /// </value>
        protected bool SupportsEventHandle { get; private set; }

        /// <summary>
        /// Gets the event handle.
        /// </summary>
        /// <value>
        /// The event handle.
        /// </value>
        protected ManualResetEvent HardwareSyncEvent { get; }

        #region Required Settings 

        /// <summary>
        /// Gets the device access mode.
        /// </summary>
        /// <value>
        /// The device access.
        /// </value>
        protected abstract AudioClientShareMode DeviceAccessMode { get; }

        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        /// <value>
        /// The length of the buffer.
        /// </value>
        protected abstract TimeSpan ManualSyncLatency { get; }

        /// <summary>
        /// Gets a value indicating whether to use hardware sampling synchronization. 
        /// </summary>
        /// <value>
        /// <c>true</c> if [use hardware synchronize]; otherwise, <c>false</c>.
        /// </value>
        protected abstract bool UseHardwareSync { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Fundamental.Interface.Wasapi.WasapiAudioSource" /> class.
        /// </summary>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client inter-operation factory.</param>
        protected WasapiAudioClient(IDeviceToken wasapiDeviceToken,
                                    IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory)
        {
            _wasapiDeviceToken = wasapiDeviceToken;
            _wasapiAudioClientInteropFactory = wasapiAudioClientInteropFactory;
            SupportsEventHandle = true;
            HardwareSyncEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// Raised when source format changes.
        /// </summary>
        public event EventHandler<EventArgs> FormatChanged;

        /// <summary>
        /// Gets the audio client.
        /// </summary>
        /// <value>
        /// The audio client.
        /// </value>
        protected virtual IWasapiAudioClientInterop AudioClientInterop => _audioClientInterop ?? (_audioClientInterop = FactoryAudioClient());

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
            closestMatchingFormats = outFormat != null ? new[] { outFormat } : new IAudioFormat[] { };
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
            return AudioClientInterop.IsFormatSupported(DeviceAccessMode, audioFormat, out closestMatchingFormat);
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
            var mixerFormat = AudioClientInterop.GetMixFormat();
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
            // Raise format changed event
            FormatChanged?.Invoke(this, EventArgs.Empty);
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

        // Private methods

        /// <summary>
        /// Ensures the is initialize.
        /// </summary>
        public void EnsureIsInitialize()
        {
            if (Interlocked.Exchange(ref _isInitialize, 1) == 1)
                return;

            try
            {
                Initialize();
            }
            catch (Exception)
            {
                _isInitialize = 0;
                throw;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected void Initialize()
        {
            var format = GetFormat();

            if (UseHardwareSync)
                InitializeForHardwareSync(format);
            else
                InitializeForManualSync(format);
        }


        private void InitializeForHardwareSync(IAudioFormat format)
        {
            // Try Initializing using hardware sync
            if (TryInitializeForHardwareSync(format))
                return;

            // If it fails then fall back to manual sync
            InitializeForManualSync(format);
        }


        private bool TryInitializeForHardwareSync(IAudioFormat format)
        {
            AudioClientInterop.Initialize(DeviceAccessMode, AudioClientStreamFlags.EventCallback, TimeSpan.Zero, TimeSpan.Zero, format);

            try
            {
                HardwareSyncEvent.Reset();
                var handle = HardwareSyncEvent.GetSafeWaitHandle().DangerousGetHandle();
                AudioClientInterop.SetEventHandle(handle);
                SupportsEventHandle = true;
            }
            catch (Exception)
            {
                // reset the current interop instance.
                _audioClientInterop = null;
                SupportsEventHandle = false;
                return false;
            }

            return true;
        }
        private void InitializeForManualSync(IAudioFormat format)
        {
            AudioClientInterop.Initialize(DeviceAccessMode, AudioClientStreamFlags.None, ManualSyncLatency, TimeSpan.Zero, format);
            SupportsEventHandle = false;
        }


        /// <summary>
        /// Gets the audio client.
        /// </summary>
        /// <returns></returns>
        protected virtual IWasapiAudioClientInterop FactoryAudioClient() => _wasapiAudioClientInteropFactory.FactoryAudioClient(_wasapiDeviceToken);

        /// <summary>
        /// Factories the capture client.
        /// </summary>
        /// <returns></returns>
        protected virtual IWasapiAudioCaptureClientInterop FactoryAudioCaptureClient()
        {
           EnsureIsInitialize();
           return AudioClientInterop.GetCaptureClient();
        }
    }
}
