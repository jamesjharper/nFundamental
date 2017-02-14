using Fundamental.Core;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceAudioSourceFactory : IDeviceAudioSourceFactory
    {
        /// <summary>
        /// The WASAPI options
        /// </summary>
        private readonly IOptions<WasapiOptions> _wasapiOptions;

        /// <summary>
        /// The device information factory
        /// </summary>
        private readonly IDeviceInfoFactory _deviceInfoFactory;

        /// <summary>
        /// The WASAPI audio client interop factory
        /// </summary>
        private readonly IWasapiAudioClientInteropFactory _wasapiAudioClientInteropFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceAudioSourceFactory"/> class.
        /// </summary>
        /// <param name="wasapiOptions">The WASAPI options.</param>
        /// <param name="deviceInfoFactory"></param>
        /// <param name="wasapiAudioClientInteropFactory">The WASAPI audio client interop factory.</param>
        public WasapiDeviceAudioSourceFactory(IOptions<WasapiOptions> wasapiOptions,
                                              IDeviceInfoFactory deviceInfoFactory,
                                              IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory)
        {
            _wasapiOptions = wasapiOptions;
            _deviceInfoFactory = deviceInfoFactory;
            _wasapiAudioClientInteropFactory = wasapiAudioClientInteropFactory;
        }

        /// <summary>
        /// Gets a source client for the given device token instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        IHardwareAudioSource IDeviceAudioSourceFactory.GetAudioSource(IDeviceToken deviceToken)
        {
            return GetAudioSource(deviceToken);
        }

        /// <summary>
        /// Gets a source client for the given device token instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public WasapiAudioSource GetAudioSource(IDeviceToken deviceToken)
        {
            var deviceInfo = _deviceInfoFactory.GetInfoDevice(deviceToken);
            return new WasapiAudioSource(deviceToken, deviceInfo, _wasapiOptions, _wasapiAudioClientInteropFactory);
        }
    }
}
