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
        /// The capture client interop
        /// </summary>
        private IWasapiAudioCaptureClientInterop _captureClientInterop;


        /// <summary>
        /// The audio pump thread
        /// </summary>
        private Thread _audioPumpThread;

        /// <summary>
        /// The is running
        /// </summary>
        private int _isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioSource" /> class.
        /// </summary>
        /// <param name="wasapiDeviceToken">The WASAPI device token.</param>
        /// <param name="wasapiOptions">The WASAPI options.</param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client inter-operations factory.</param>
        public WasapiAudioSource(IDeviceToken wasapiDeviceToken, 
                                 IOptions<WasapiOptions> wasapiOptions,
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
        /// Starts capturing audio.
        /// </summary>
        public void Start()
        {
            // If the pump is already running, then do nothing
            if (Interlocked.Exchange(ref _isRunning, 1) == 1)
                return;

            try
            {
                using (var waitForAudioPumpToStart = new ManualResetEventSlim(false))
                {
                    _captureClientInterop = FactoryAudioCaptureClient();
                    _audioPumpThread = new Thread(() =>
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            waitForAudioPumpToStart.Set();
                            StartAudioPump();
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
        /// Reads the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int length)
        {
            return _captureClientInterop?.Read(buffer, offset, length) ?? 0;
        }

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
        /// Occurs when data available from the source.
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        // Private methods

        /// <summary>
        /// Starts the audio pump.
        /// </summary>
        private void StartAudioPump()
        {
            try
            {
                Started?.Invoke(this, EventArgs.Empty);
                AudioClientInterop.Start();
        
                if (SupportsEventHandle)
                    HardwareSyncAudioPump();
                else
                    ManualSyncAudioPump();

                AudioClientInterop.Stop();
                AudioClientInterop.Reset();
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

        /// <summary>
        /// Pumps the current audio content audio.
        /// </summary>
        private void PumpAudio()
        {
            _captureClientInterop.UpdateBuffer();
            var bufferSize = _captureClientInterop.GetBufferByteSize();

            if (bufferSize == 0)
                return;

            DataAvailable?.Invoke(this, new DataAvailableEventArgs(bufferSize));

            // Drop any remaining frames if they where not consumed from the read method
            _captureClientInterop.ReleaseBuffer();
        }

        /// <summary>
        /// Runs the audio pump using hardware interrupt audio synchronization
        /// </summary>
        private void HardwareSyncAudioPump()
        {
            var bufferSize = AudioClientInterop.GetBufferSize();
            var latency = _captureClientInterop.FramesToLatency(bufferSize);
            var bufferUnderrunCount = 0;

            while (_isRunning == 1)
            {
                if (bufferUnderrunCount > MaxBufferUnderruns)
                    break;

                if (!HardwareSyncEvent.WaitOne(latency))
                {
                    bufferUnderrunCount++;
                    continue;
                }

                PumpAudio();
            }
        }

        /// <summary>
        /// Runs the audio pump using Manual audio synchronization
        /// </summary>
        private void ManualSyncAudioPump()
        {
            var bufferSize = AudioClientInterop.GetBufferSize();
            var latency = _captureClientInterop.FramesToLatency(bufferSize);
            var pollRate = TimeSpan.FromTicks(latency.Ticks / 2);

            while (_isRunning == 1)
            {
                Thread.Sleep(pollRate);
                PumpAudio();
            }
        }
    }
}
