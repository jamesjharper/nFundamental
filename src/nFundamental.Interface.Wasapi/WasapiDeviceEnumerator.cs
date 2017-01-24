using System.Collections.Generic;
using System.Linq;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceEnumerator : IDeviceEnumerator
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
        /// Initializes a new instance of the <see cref="WasapiDeviceEnumerator"/> class.
        /// </summary>
        /// <param name="deviceEnumerator">The device enumerator.</param>
        /// <param name="wasapiDeviceTokenFactory">The WASAPI device token factory.</param>
        public WasapiDeviceEnumerator(IMMDeviceEnumerator deviceEnumerator, IWasapiDeviceTokenFactory wasapiDeviceTokenFactory)
        {
            _deviceEnumerator = deviceEnumerator;
            _wasapiDeviceTokenFactory = wasapiDeviceTokenFactory;
        }

        /// <summary>
        /// Gets the all known system devices.
        /// </summary>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        public IEnumerable<WasapiDeviceToken> GetDevices()
        {
            return GetDevices(DataFlow.All, Interop.DeviceState.All);
        }

        /// <summary>
        /// Gets the all known system devices by the device type.
        /// </summary>
        /// <param name="type">The device type.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        public IEnumerable<WasapiDeviceToken> GetDevices(DeviceType type)
        {
            var dataFlow = type.ConvertToWasapiDataFlow();
            return GetDevices(dataFlow, Interop.DeviceState.All);
        }

        /// <summary>
        /// Gets the all known system devices by the device type and its current state.
        /// </summary>
        /// <param name="type">The device type.</param>
        /// <param name="stateMask">State mask.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        public IEnumerable<WasapiDeviceToken> GetDevices(DeviceType type, params DeviceState[] stateMask)
        {
            var deviceState = stateMask.ConvertToWasapiDeviceState();

            var dataFlow = type.ConvertToWasapiDataFlow();
            return GetDevices(dataFlow, deviceState);
        }

        /// <summary>
        /// Gets the all known system devices by the device type and its current state.
        /// </summary>
        /// <param name="typeMask">The type mask.</param>
        /// <param name="stateMask">The state mask.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        public IEnumerable<WasapiDeviceToken> GetDevices(DeviceType[] typeMask, DeviceState[] stateMask)
        {
            var deviceState = stateMask.ConvertToWasapiDeviceState();
            var dataFlow = typeMask.ConvertToWasapiDeviceState();
            return GetDevices(dataFlow, deviceState);
        }

        #region IDeviceEnumerator

        /// <summary>
        /// Gets the all known system devices.
        /// </summary>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        IEnumerable<IDeviceToken> IDeviceEnumerator.GetDevices()
        {
            return GetDevices();
        }


        /// <summary>
        /// Gets the all known system devices by the device type.
        /// </summary>
        /// <param name="type">The device type.</param>
        /// <returns>IEnumerable of device tokens</returns>
        IEnumerable<IDeviceToken> IDeviceEnumerator.GetDevices(DeviceType type)
        {
            return GetDevices(type);
        }

        /// <summary>
        /// Gets the all known system devices by the device type and its current state.
        /// </summary>
        /// <param name="type">The device type.</param>
        /// <param name="stateMask">State mask.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        IEnumerable<IDeviceToken> IDeviceEnumerator.GetDevices(DeviceType type, params DeviceState[] stateMask)
        {
            return GetDevices(type, stateMask);
        }

        /// <summary>
        /// Gets the all known system devices by the device type and its current state.
        /// </summary>
        /// <param name="typeMask">The type mask.</param>
        /// <param name="stateMask">The state mask.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        IEnumerable<IDeviceToken> IDeviceEnumerator.GetDevices(DeviceType[] typeMask, DeviceState[] stateMask)
        {
            return GetDevices(typeMask, stateMask);
        }


        #endregion

        // Private Methods

        private IEnumerable<WasapiDeviceToken> GetDevices(DataFlow dataFlow, Interop.DeviceState stateMask)
        {
            IMMDeviceCollection result;
            _deviceEnumerator.EnumAudioEndpoints(dataFlow, stateMask, out result).ThrowIfFailed();
            return GetTokenEnumerable(result);
        }

        private IEnumerable<WasapiDeviceToken> GetTokenEnumerable(IMMDeviceCollection deviceCollection)
        {
            return GetDeviceEnumerable(deviceCollection)
                .Select(device => _wasapiDeviceTokenFactory.GetToken(device));
        }

        private IEnumerable<IMMDevice> GetDeviceEnumerable(IMMDeviceCollection deviceCollection)
        {
            int count;
            deviceCollection.GetCount(out count).ThrowIfFailed();

            for (var i = 0; i < count; i++)
            {
                IMMDevice mmDevice;
                deviceCollection.Item(i, out mmDevice).ThrowIfFailed();
                yield return mmDevice;
            }
        }
    }
}
