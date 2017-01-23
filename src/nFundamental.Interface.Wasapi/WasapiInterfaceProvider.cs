using System;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiInterfaceProvider : InterfaceProvider,
        ISupportsInterface<IDefaultDeviceProvider>,
        ISupportsInterface<IDefaultDeviceStatusNotifier>,
        ISupportsInterface<IDeviceAvailabilityNotifier>,
        ISupportsInterface<IDeviceEnumerator>,
        ISupportsInterface<IDeviceInfoFactory>,
        ISupportsInterface<IDeviceStatusNotifier>

    {

        #region IMMDeviceEnumerator Dependency

        private IMMDeviceEnumerator _deviceEnumerator;

        private IMMDeviceEnumerator WasapiDeviceEnumerator => _deviceEnumerator ?? (_deviceEnumerator = FactoryWasapiDeviceEnumerator());

        /// <summary>
        /// Factories the wasapi device enumerator.
        /// </summary>
        /// <returns></returns>
        protected virtual IMMDeviceEnumerator FactoryWasapiDeviceEnumerator()
        {
            // Test Code seam for injecting Mock instance
            return ComObject.CreateInstance<IMMDeviceEnumerator>(ClsIds.MMDeviceEnumeratorClsId);
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
        private IWasapiPropertyNameTranslator WasapiPropertyNameTranslator => new WasapiPropertyNameTranslator();

        #endregion

        /// <summary>
        /// Gets the audio interface used for finding system default devices.
        /// </summary>
        /// <returns></returns>
        IDefaultDeviceProvider ISupportsInterface<IDefaultDeviceProvider>.GetAudioInterface() 
            => new WasapiDefaultDeviceProvider(WasapiDeviceEnumerator, DeviceTokenFactory);
        
        /// <summary>
        /// Gets the audio interface for finding all system devices.
        /// </summary>
        /// <returns></returns>
        IDeviceEnumerator ISupportsInterface<IDeviceEnumerator>.GetAudioInterface() => 
            new WasapiDeviceEnumerator(WasapiDeviceEnumerator, DeviceTokenFactory);

        /// <summary>
        /// Gets the default device status notifier interface.
        /// </summary>
        /// <returns></returns>
        IDefaultDeviceStatusNotifier ISupportsInterface<IDefaultDeviceStatusNotifier>.GetAudioInterface() 
            => WasapiInterfaceNotifyClient;

        /// <summary>
        /// Gets the device availability notifier
        /// </summary>
        /// <returns></returns>
        IDeviceAvailabilityNotifier ISupportsInterface<IDeviceAvailabilityNotifier>.GetAudioInterface()
            => WasapiInterfaceNotifyClient;

        /// <summary>
        /// Gets the device status notifier
        /// </summary>
        /// <returns></returns>
        IDeviceStatusNotifier ISupportsInterface<IDeviceStatusNotifier>.GetAudioInterface()
            => WasapiInterfaceNotifyClient;


        /// <summary>
        /// Gets the audio interface used for finding details about devices 
        /// </summary>
        /// <returns></returns>
        IDeviceInfoFactory ISupportsInterface<IDeviceInfoFactory>.GetAudioInterface() 
            => new WasapiDeviceInfoFactory(WasapiInterfaceNotifyClient, WasapiPropertyNameTranslator);

    }
}
