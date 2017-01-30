using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDefaultDeviceProvider : IDefaultDeviceProvider
    {
        /// <summary>
        /// The underlying device enumerator
        /// </summary>
        private readonly IMMDeviceEnumerator _deviceEnumerator;

        /// <summary>
        /// The WASAPI device token factory
        /// </summary>
        private readonly IWasapiDeviceTokenFactory _wasapiDeviceTokenFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDefaultDeviceProvider"/> class.
        /// </summary>
        /// <param name="deviceEnumerator">The device enumerator.</param>
        /// <param name="wasapiDeviceTokenFactory"></param>
        public WasapiDefaultDeviceProvider(IMMDeviceEnumerator deviceEnumerator, IWasapiDeviceTokenFactory wasapiDeviceTokenFactory)
        {
            _deviceEnumerator = deviceEnumerator;
            _wasapiDeviceTokenFactory = wasapiDeviceTokenFactory;
        }

        /// <summary>
        /// Gets the default device for the operating system
        /// </summary>
        /// <param name="type">The type of device.</param>
        /// <param name="deviceRole">The device role.</param>
        /// <returns>
        /// A Token which can be used for accessing the device
        /// </returns>
        public WasapiDeviceToken GetDefaultDevice(DeviceType type, DeviceRole deviceRole)
        {
            var dataFlow = type.ConvertToWasapiDataFlow();
            var role = deviceRole.ConvertToWasapiDeviceRole();

            IMMDevice immDevice;
            _deviceEnumerator.GetDefaultAudioEndpoint(dataFlow, role, out immDevice).ThrowIfFailed();
            return _wasapiDeviceTokenFactory.GetToken(immDevice);
        }


        #region IDefaultDeviceProvider


        /// <summary>
        /// Gets the default device for the operating system
        /// </summary>
        /// <param name="type">The type of device.</param>
        /// <param name="deviceRole">The device role.</param>
        /// <returns>
        /// A Token which can be used for accessing the device
        /// </returns>
        IDeviceToken IDefaultDeviceProvider.GetDefaultDevice(DeviceType type, DeviceRole deviceRole)
        {
            return GetDefaultDevice(type, deviceRole);
        }

        #endregion
    }
}
