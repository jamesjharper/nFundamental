using System;
using System.Reflection;
using System.Threading.Tasks;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiAudioSource : IAudioSource
    {
        /// <summary>
        /// The WASAPI device token
        /// </summary>
        private readonly WasapiDeviceToken _wasapiDeviceToken;

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
        public WasapiAudioSource(IOptions<WasapiOptions> wasapiOptions, WasapiDeviceToken wasapiDeviceToken)
        {
            _wasapiOptions = wasapiOptions;
            _wasapiDeviceToken = wasapiDeviceToken;
        }


        public Task Handshake(IFormatNegotiator formatNegotiator)
        {
            
            return Task.CompletedTask;
        }

        public Task Start()
        {
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Started;
        public event EventHandler<EventArgs> Stopped;
        public event EventHandler<SourceDataReceivedEventArgs> DataRecived;

        // Private methods

        private IAudioClient ConntectToDevice(WasapiDeviceToken wasapiDeviceToken)
        {
            var immDevice = wasapiDeviceToken.MmDevice;

            object audioClient;
            var type = typeof(IAudioClient).GetTypeInfo().GUID;
            immDevice.Activate(type, ClsCtx.LocalServer, IntPtr.Zero,  out audioClient).ThrowIfFailed();
            // Throw exceptions
            
            return audioClient as IAudioClient;
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
    }
}
