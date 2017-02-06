using System;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiInterfaceProvider : InterfaceProvider,
            IInterfaceOptions<WasapiOptions>,
            ISupportsInterface<WasapiDefaultDeviceProvider>,
            ISupportsInterface<WasapiInterfaceNotifyClient>,
            ISupportsInterface<WasapiDeviceEnumerator>,
            ISupportsInterface<WasapiDeviceInfoFactory>

    {

        #region WASAPI Options

        /// <summary>
        /// The configuration options for WASAPI audio interface
        /// </summary>
        private readonly Options<WasapiOptions> _options = Options<WasapiOptions>.Default;

        /// <summary>
        /// Configures the specified configure.
        /// </summary>
        /// <param name="configure">The configure.</param>
        public void Configure(Action<WasapiOptions> configure)
        {
            _options?.Configure(configure);
        }

        #endregion

        #region WaveFormatToAudioFormatConverter Dependency


        /// <summary>
        /// Gets the audio format converter wave format.
        /// </summary>
        /// <value>
        /// The audio format converter wave format.
        /// </value>
        private static IAudioFormatConverter<WaveFormat> AudioFormatConverterWaveFormat => WaveFormatInterfaceProvider.GetInterface<IAudioFormatConverter<WaveFormat>>();

        #endregion

        #region IMMDeviceEnumerator Dependency

        private IMMDeviceEnumerator _deviceEnumerator;

        private IMMDeviceEnumerator WasapiDeviceEnumerator => _deviceEnumerator ?? (_deviceEnumerator = FactoryWasapiDeviceEnumerator());

        /// <summary>
        /// Factories the WASAPI device enumerator.
        /// </summary>
        /// <returns></returns>
        protected virtual IMMDeviceEnumerator FactoryWasapiDeviceEnumerator()
        {
            // Test Code seam for injecting Mock instance
            return ComObject.CreateInstance<IMMDeviceEnumerator>(ClsIds.MmDeviceEnumeratorGuid);
        }

        #endregion

        #region IWasapiDeviceTokenFactory Dependency

        private IWasapiDeviceTokenFactory _wasapiDeviceTokenFactory;

        private IWasapiDeviceTokenFactory DeviceTokenFactory => _wasapiDeviceTokenFactory ?? (_wasapiDeviceTokenFactory = FactoryDeviceTokenFactory());

        /// <summary>
        /// Factories the device token factory.
        /// </summary>
        /// <returns></returns>
        protected virtual IWasapiDeviceTokenFactory FactoryDeviceTokenFactory()
        {
            // Test Code seam for injecting Mock instance
            return new WasapiDeviceTokenFactory(WasapiDeviceEnumerator);
        }

        #endregion

        #region WasapiInterfaceNotifyClient Dependency

        private WasapiInterfaceNotifyClient _wasapiInterfaceNotifyClient;

        private WasapiInterfaceNotifyClient WasapiInterfaceNotifyClient => _wasapiInterfaceNotifyClient ?? (_wasapiInterfaceNotifyClient = FactoryWasapiInterfaceNotifyClient());

        /// <summary>
        /// Factories the device token factory.
        /// </summary>
        /// <returns></returns>
        protected virtual WasapiInterfaceNotifyClient FactoryWasapiInterfaceNotifyClient()
        {
            // Test Code seam for injecting Mock instance
            _wasapiInterfaceNotifyClient = new WasapiInterfaceNotifyClient(DeviceTokenFactory);
            WasapiDeviceEnumerator.RegisterEndpointNotificationCallback(_wasapiInterfaceNotifyClient);
            return _wasapiInterfaceNotifyClient;
        }

        #endregion

        #region IWasapiPropertyNameTranslator Dependency

        /// <summary>
        /// Gets the WASAPI property name translator.
        /// </summary>
        /// <value>
        /// The WASAPI property name translator.
        /// </value>
        private IWasapiPropertyNameTranslator WasapiPropertyNameTranslator => new WasapiPropertyNameTranslator(_options);

        #endregion

        /// <summary>
        /// Gets the audio interface used for finding system default devices.
        /// </summary>
        /// <returns></returns>
        WasapiDefaultDeviceProvider ISupportsInterface<WasapiDefaultDeviceProvider>.GetAudioInterface() 
            => new WasapiDefaultDeviceProvider(WasapiDeviceEnumerator, DeviceTokenFactory);

        /// <summary>
        /// Gets the audio interface for finding all system devices.
        /// </summary>
        /// <returns></returns>
        WasapiDeviceEnumerator ISupportsInterface<WasapiDeviceEnumerator>.GetAudioInterface() => 
            new WasapiDeviceEnumerator(WasapiDeviceEnumerator, DeviceTokenFactory);

        /// <summary>
        /// Gets the WASAPI Interface notification client. Typically this endpoint is used for getting the
        /// Interfaces IDeviceStatusNotifier, IDefaultDeviceStatusNotifier and IDeviceAvailabilityNotifier
        /// </summary>
        /// <returns></returns>
        WasapiInterfaceNotifyClient ISupportsInterface<WasapiInterfaceNotifyClient>.GetAudioInterface()
              => WasapiInterfaceNotifyClient;

        /// <summary>
        /// Gets the audio interface used for finding details about devices 
        /// </summary>
        /// <returns></returns>
        WasapiDeviceInfoFactory ISupportsInterface<WasapiDeviceInfoFactory>.GetAudioInterface() 
            => new WasapiDeviceInfoFactory(WasapiInterfaceNotifyClient, WasapiPropertyNameTranslator, AudioFormatConverterWaveFormat);

    }
}
