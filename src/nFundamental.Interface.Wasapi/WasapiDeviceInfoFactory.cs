using Fundamental.Core;
using Fundamental.Wave.Format;
using Fundamental.Interface.Wasapi.Internal;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceInfoFactory : IDeviceInfoFactory
    {
        /// <summary>
        /// The WASAPI interface notify client
        /// </summary>
        private readonly IWasapiInterfaceNotifyClient _wasapiInterfaceNotifyClient;

        /// <summary>
        /// The WASAPI property name translator
        /// </summary>
        private readonly IWasapiPropertyNameTranslator _wasapiPropertyNameTranslator;

        /// <summary>
        /// The wave format converter
        /// </summary>
        private readonly IAudioFormatConverter<WaveFormat> _waveFormatConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceInfoFactory" /> class.
        /// </summary>
        /// <param name="wasapiInterfaceNotifyClient">The WASAPI interface notify client.</param>
        /// <param name="wasapiPropertyNameTranslator">The WASAPI property name translator.</param>
        /// <param name="waveFormatConverter"></param>
        public WasapiDeviceInfoFactory(IWasapiInterfaceNotifyClient wasapiInterfaceNotifyClient,
                                       IWasapiPropertyNameTranslator wasapiPropertyNameTranslator,
                                       IAudioFormatConverter<WaveFormat> waveFormatConverter)
        {
            _wasapiInterfaceNotifyClient = wasapiInterfaceNotifyClient;
            _wasapiPropertyNameTranslator = wasapiPropertyNameTranslator;
            _waveFormatConverter = waveFormatConverter;
        }

        /// <summary>
        /// Gets a information device instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public WasapiDeviceInfo GetInfoDevice(WasapiDeviceToken deviceToken)
        {
            return new WasapiDeviceInfo(_wasapiInterfaceNotifyClient, _wasapiPropertyNameTranslator, _waveFormatConverter, deviceToken);
        }

        /// <summary>
        /// Gets the information device.
        /// </summary>
        /// <param name="deviceToken">The device handle.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">WasapiDeviceInfoFactory can only except tokens of type WasapiDeviceToken</exception>
        IDeviceInfo IDeviceInfoFactory.GetInfoDevice(IDeviceToken deviceToken)
        {
            var token = deviceToken as WasapiDeviceToken;
            if (token != null) 
                return GetInfoDevice(token);

            var message = $"{nameof(WasapiDeviceInfoFactory)} can only except tokens of type {nameof(WasapiDeviceToken)}";
            throw new UnsupportedTokenTypeException(message, typeof(WasapiDeviceToken));
        }
    }
}
