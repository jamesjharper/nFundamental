using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;

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
        /// The device information
        /// </summary>
        private readonly IDeviceInfo _deviceInfo;

        /// <summary>
        /// The WASAPI audio client factory
        /// </summary>
        private readonly IWasapiAudioClientInteropFactory _wasapiAudioClientInteropFactory;

        /// <summary>
        /// The list of known driver keys which can yield supported formats.
        /// </summary>
        private static readonly IList<string> KnownDriverKeysForSupportedFormats = new List<string>
        {
            "AudioEngine.DeviceFormat",
            "AudioEngine.OemFormat"
        };

        // protected fields

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
        protected AutoResetEvent HardwareSyncEvent { get; }

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
        /// The current latency calculator
        /// </summary>
        private ILatencyCalculator _latencyCalculator;

        /// <summary>
        /// The audio pump thread
        /// </summary>
        private Thread _audioPumpThread;

        /// <summary>
        /// The is running
        /// </summary>
        private int _isRunning;

        /// <summary>
        /// The cached supported formats as calculating them involves a lot of com calls
        /// </summary>
        private IAudioFormat[] _cachedSupportedFormats;


        /// <summary>
        /// Initializes a new instance of the <see cref="Fundamental.Interface.Wasapi.WasapiAudioSource" /> class.
        /// </summary>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="options"></param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client inter-operation factory.</param>
        protected WasapiAudioClient(IDeviceToken wasapiDeviceToken,
                                    IDeviceInfo deviceInfo,
                                    WasapiAudioClientSettings options,
                                    IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory)
        {
            _wasapiDeviceToken = wasapiDeviceToken;
            _deviceInfo = deviceInfo;
            Options = options;
            _wasapiAudioClientInteropFactory = wasapiAudioClientInteropFactory;
            SupportsEventHandle = true;
            HardwareSyncEvent = new AutoResetEvent(false);
        }

        /// <summary>
        /// Gets the client options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        protected WasapiAudioClientSettings Options { get; }

        /// <summary>
        /// Raised when source format changes.
        /// </summary>
        public event EventHandler<EventArgs> FormatChanged;

        /// <summary>
        /// Raised when actual capturing is started.
        /// </summary>
        public event EventHandler<EventArgs> Started;

        /// <summary>
        /// Raised when actual capturing is stopped.
        /// </summary>
        public event EventHandler<EventArgs> Stopped;

        /// <summary>
        /// Raised when an error occurs during streaming.
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorOccurred;

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
            var deviceAccessMode = Options.DeviceAccess.ConvertToWasapiAudioClientShareMode();
            return WasapiClient.IsFormatSupported(deviceAccessMode, audioFormat, out closestMatchingFormat);
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
            if (_cachedSupportedFormats != null)
                return _cachedSupportedFormats;
            _cachedSupportedFormats = CalculateSuggestFormats().ToArray();
            return _cachedSupportedFormats;
        }

        /// <summary>
        /// Sets the format.
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <exception cref="Fundamental.Core.FormatNotSupportedException">Target device does not support the given format</exception>
        public void SetFormat(IAudioFormat audioFormat)
        {
            // Format can not be set on an initialized instance
            EnsureIsDeinitialize();

            if (!IsAudioFormatSupported(audioFormat))
                throw new FormatNotSupportedException("Target device does not support the given format");
            DesiredAudioFormat = audioFormat;

            // Initialize instance to reduce latency when starting pumping for the first time
            EnsureIsInitialize();

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

        /// <summary>
        /// Starts capturing audio.
        /// </summary>
        public void Start()
        {
            // If the pump is already running, then do nothing
            if (Interlocked.Exchange(ref _isRunning, 1) == 1)
                return;

            try
            {
                EnsureIsInitialize();

                using (var waitForAudioPumpToStart = new ManualResetEventSlim(false))
                {
                    _audioPumpThread = new Thread(() =>
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            waitForAudioPumpToStart.Set();
                            CallAudioPump();
                        });

                    _audioPumpThread.Start();

                    if (!waitForAudioPumpToStart.Wait(1000))
                        throw new FailedToStartAudioPumpException("Starting audio pump timed out.");
                }
            }
            catch (Exception)
            {
                _isRunning = 0;
                throw;
            }
        }


        /// <summary>
        /// Stops capturing audio.
        /// </summary>
        public void Stop()
        {
            _isRunning = 0;
            _audioPumpThread?.Join();
            _audioPumpThread = null;
        }

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
        /// Ensures the is deinitialize.
        /// </summary>
        public void EnsureIsDeinitialize()
        {
            if (Interlocked.Exchange(ref _isInitialize, 0) == 0)
                return;
            
            // Make sure we are not streaming
            Stop();
            Deinitialize();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is spooling the audio pump.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning => _isRunning == 1;

        // Protected Methods

        /// <summary>
        /// Primes the audio pump so that its ready for pumping.
        /// </summary>
        protected virtual void PrimeAudioPrime()
        {
            WasapiClient.Reset();
        }

        /// <summary>
        /// Pumps the current audio content audio using a manual sync
        /// </summary>
        protected abstract bool PumpAudioManunalSync(TimeSpan pollRate);

        /// <summary>
        /// Pumps the current audio content audio using hardware sync
        /// </summary>
        /// <param name="latency">The latency.</param>
        /// <returns></returns>
        protected abstract bool PumpAudioHardwareSync(TimeSpan latency);

        /// <summary>
        /// Gets or sets the audio format latency calculator.
        /// </summary>
        /// <value>
        /// The audio format latency calculator.
        /// </value>
        protected virtual ILatencyCalculator GetAudioFormatLatencyCalculator()
        {
            if (_latencyCalculator == null)
                throw new DeviceNotInitializedException("Unable to get audio format latency calculator, when device is not initialized.");
            return _latencyCalculator;
        }

        /// <summary>
        /// Initializes the implementation.
        /// </summary>
        protected virtual void InitializeImpl()
        {
        }

        // Private methods

        #region Format methods

        public IEnumerable<IAudioFormat> CalculateSuggestFormats()
        {
            var mixerFormats = CalculateMixerFormats();
            var oemFormats = CalculateOemFormats();

            var formats = Options.PreferDeviceNativeFormat
                ? oemFormats.Concat(mixerFormats)
                : mixerFormats.Concat(oemFormats);

            return formats.Distinct();
        }

        private IEnumerable<IAudioFormat> CalculateMixerFormats()
        {
            var mixerFormat = WasapiClient.GetMixFormat();
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
        }

        private IEnumerable<IAudioFormat> CalculateOemFormats()
        {
            // If OEM formats are to be ignored,
            if(Options.IgnoreDeviceNativeFormat)
               yield break;

            foreach (var knownKey in KnownDriverKeysForSupportedFormats)
            {
                IAudioFormat audioFormat;
                if (!TryGetDeivceDriveFormat(knownKey, out audioFormat)) 
                    continue;
                
                if (!IsAudioFormatSupported(audioFormat))
                    continue;

                yield return audioFormat;
            }
        }

        private bool TryGetDeivceDriveFormat(string keyName, out IAudioFormat audioFormat)
        {
            audioFormat = null;

            object outFormat;
            if (!_deviceInfo.Properties.TryGetValue(keyName, out outFormat))
                return false;

            audioFormat = outFormat as IAudioFormat;
            return audioFormat != null && IsAudioFormatSupported(audioFormat);
        }

        #endregion

        #region Audio Pump methods

        protected void CallAudioPump()
        {
            try
            {
        

                Started?.Invoke(this, EventArgs.Empty);

                // Give the change to reset the current buffer and ready the pump for 
                // starting with out any glitches
                PrimeAudioPrime();

                WasapiClient.Start();

                if (SupportsEventHandle)
                    HardwareSyncAudioPump();
                else
                    ManualSyncAudioPump();

                WasapiClient.Stop();
                WasapiClient.Reset();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ErrorEventArgs(ex));
            }
            finally
            {
                _isRunning = 0;
                Stopped?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HardwareSyncAudioPump()
        {
            var hardwareSyncTimeout = GetHardwareSyncTimeout();
            while (IsRunning && PumpAudioHardwareSync(hardwareSyncTimeout)) { }
        }

        private void ManualSyncAudioPump()
        {
            var manualSyncPollRate = GetManualSyncPollRate();
            while (IsRunning && PumpAudioManunalSync(manualSyncPollRate)) { }
        }


        #endregion

        #region Initialize methods

        private void Initialize()
        {
            var format = GetFormat();

            if (Options.UseHardwareSync)
                InitializeForHardwareSync(format);
            else
                InitializeForManualSync(format);

            _latencyCalculator = FactoryLatencyCaculator(format);
            InitializeImpl();
        }

        private void Deinitialize()
        {
            _audioClientInterop = null;
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
            var deviceAccessMode = Options.DeviceAccess.ConvertToWasapiAudioClientShareMode();
            WasapiClient.Initialize(deviceAccessMode, AudioClientStreamFlags.EventCallback, TimeSpan.Zero, TimeSpan.Zero, format);

            try
            {
                HardwareSyncEvent.Reset();
                var handle = HardwareSyncEvent.GetSafeWaitHandle().DangerousGetHandle();
                WasapiClient.SetEventHandle(handle);
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
            var deviceAccessMode = Options.DeviceAccess.ConvertToWasapiAudioClientShareMode();
            var manualSyncLatency = Options.ManualSyncLatency;
            WasapiClient.Initialize(deviceAccessMode, AudioClientStreamFlags.None, manualSyncLatency, TimeSpan.Zero, format);
            SupportsEventHandle = false;
        }

        #endregion

        #region Latency Calculating methods

        private static ILatencyCalculator FactoryLatencyCaculator(IAudioFormat format)
        {
            var sampleRate = format.Value<int>(FormatKeys.Pcm.SampleRate);
            var frameSize = format.Value<int>(FormatKeys.Pcm.Packing);
            return new LatencyCalculator(frameSize, (ulong)sampleRate);
        }

        protected TimeSpan GetManualSyncPollRate()
        {
            var bufferSpan = GetBufferTimeSpan();
            return TimeSpan.FromTicks(bufferSpan.Ticks / 2);
        }

        protected TimeSpan GetHardwareSyncTimeout()
        {
            // The hardware sync is equal to the buffer length
            return GetBufferTimeSpan();
        }

        protected TimeSpan GetBufferTimeSpan()
        {
            var latencyCaculator = GetAudioFormatLatencyCalculator();
            var bufferSize = WasapiClient.GetBufferSize();
            return latencyCaculator.FramesToLatency(bufferSize);
        }

        #endregion

        #region Factory methods

        protected virtual IWasapiAudioClientInterop FactoryWasapiAudioClient() => _wasapiAudioClientInteropFactory.FactoryAudioClient(_wasapiDeviceToken);

        protected virtual IWasapiAudioClientInterop WasapiClient => _audioClientInterop ?? (_audioClientInterop = FactoryWasapiAudioClient());

        #endregion
    }
}
