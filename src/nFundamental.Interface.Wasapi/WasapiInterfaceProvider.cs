using System;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiInterfaceProvider : InterfaceProvider,
        ISupportsInterface<IDefaultDeviceProvider>,
        ISupportsInterface<IDefaultDeviceStatusNotifier>,
        ISupportsInterface<IDeviceAvailabilityNotifier>,
        ISupportsInterface<IDeviceEnumerator>,
        ISupportsInterface<IDeviceInfoRepository>,
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
            return new WasapiDeviceTokenFactory();
        }

        #endregion


        /// <summary>
        /// Gets the audio interface for finding system default devices.
        /// </summary>
        /// <returns></returns>
        IDefaultDeviceProvider ISupportsInterface<IDefaultDeviceProvider>.GetAudioInterface()
        {
            return new WasapiDefaultDeviceProvider(WasapiDeviceEnumerator, DeviceTokenFactory);
        }

        /// <summary>
        /// Gets the audio interface for finding all system devices.
        /// </summary>
        /// <returns></returns>
        IDeviceEnumerator ISupportsInterface<IDeviceEnumerator>.GetAudioInterface()
        {
            return new WasapiDeviceEnumerator(WasapiDeviceEnumerator, DeviceTokenFactory);
        }

        IDeviceInfoRepository ISupportsInterface<IDeviceInfoRepository>.GetAudioInterface()
        {
            throw new NotImplementedException();
        }


        IDefaultDeviceStatusNotifier ISupportsInterface<IDefaultDeviceStatusNotifier>.GetAudioInterface()
        {
            throw new NotImplementedException();
        }

        IDeviceAvailabilityNotifier ISupportsInterface<IDeviceAvailabilityNotifier>.GetAudioInterface()
        {
            throw new NotImplementedException();
        }

        IDeviceStatusNotifier ISupportsInterface<IDeviceStatusNotifier>.GetAudioInterface()
        {
            throw new NotImplementedException();
        }
    }
}
