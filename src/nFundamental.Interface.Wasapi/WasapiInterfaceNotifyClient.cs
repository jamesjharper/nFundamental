using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    [Guid("DDE8D8D4-055B-4FB1-BE59-6889F70EF9D1")]
    public class WasapiInterfaceNotifyClient :
       IMMNotificationClient,
       IDeviceStatusNotifier,
       IDefaultDeviceStatusNotifier,
       IDeviceAvailabilityNotifier
    {

        /// <summary>
        /// The WASAPI device token factory
        /// </summary>
        private readonly IWasapiDeviceTokenFactory _wasapiDeviceTokenFactory;

        #region IDeviceStatusNotifier

        /// <summary>
        /// Occurs when a device status changes
        /// </summary>
        public event EventHandler<DeviceStatusChangedEvent> DeviceStatusChanged;

        #endregion

        #region IDefaultDeviceStatusNotifier

        /// <summary>
        /// Occurs when default device changed.
        /// </summary>
        public event EventHandler<DefaultDeviceChangedEventArgs> DefaultDeviceChanged;

        #endregion

        #region IDefaultDeviceStatusNotifier

        /// <summary>
        /// Occurs when a device added is to the system.
        /// </summary>
        public event EventHandler<DeviceAddedEventArgs> DeviceAdded;

        /// <summary>
        /// Occurs when a device removed is to the system.
        /// </summary>
        public event EventHandler<DeviceRemovedEventArgs> DeviceRemoved;

        #endregion


        /// <summary>
        /// Occurs when [device property changed].
        /// </summary>
        public event EventHandler<DevicePropertyChangedEventArgs> DevicePropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiInterfaceNotifyClient" /> class.
        /// </summary>
        /// <param name="wasapiDeviceTokenFactory">The WASAPI Device Token factory.</param>
        public WasapiInterfaceNotifyClient(IWasapiDeviceTokenFactory wasapiDeviceTokenFactory)
        {
            _wasapiDeviceTokenFactory = wasapiDeviceTokenFactory;
        }

        #region IMMNotificationClient


        /// <summary>
        /// The OnDeviceStateChanged method indicates that the state of an audio endpoint device has changed.
        /// </summary>
        /// <param name="deviceId">Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated,
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.</param>
        /// <param name="deviceState">Specifies the new state of the endpoint device. The value of this parameter is one of the following DEVICE_STATE_XXX constants:
        /// DEVICE_STATE_ACTIVE
        /// DEVICE_STATE_DISABLED
        /// DEVICE_STATE_NOTPRESENT
        /// DEVICE_STATE_UNPLUGGED
        /// </param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        HResult IMMNotificationClient.OnDeviceStateChanged(string deviceId, Interop.DeviceState deviceState)
        {
            var token = _wasapiDeviceTokenFactory.GetToken(deviceId);
            var state = deviceState.ConvertToFundamentalDeviceState();
            DeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEvent(token, state));
            return HResult.S_OK;
        }

        /// <summary>
        /// The OnDeviceAdded method indicates that a new audio endpoint device has been added.
        /// </summary>
        /// <param name="deviceId">Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated,
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.</param>
        /// <returns></returns>
        HResult IMMNotificationClient.OnDeviceAdded(string deviceId)
        {
            var token = _wasapiDeviceTokenFactory.GetToken(deviceId);
            DeviceAdded?.Invoke(this, new DeviceAddedEventArgs(token));
            return HResult.S_OK;
        }

        /// <summary>
        /// The OnDeviceRemoved method indicates that an audio endpoint device has been removed.
        /// </summary>
        /// <param name="deviceId">Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated,
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        HResult IMMNotificationClient.OnDeviceRemoved(string deviceId)
        {
            var token = _wasapiDeviceTokenFactory.GetToken(deviceId);
            DeviceRemoved?.Invoke(this, new DeviceRemovedEventArgs(token));
            return HResult.S_OK;
        }

        /// <summary>
        /// The OnDefaultDeviceChanged method notifies the client that the default audio endpoint device for a particular device role has changed.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction of the endpoint device. This parameter is set to one of the following EDataFlow enumeration values:
        /// - eRender
        /// - eCapture
        /// The data-flow direction for a rendering device is eRender.The data-flow direction for a capture device is eCapture.</param>
        /// <param name="deviceRole">The device role of the audio endpoint device. This parameter is set to one of the following ERole enumeration values:
        /// - eConsole
        /// - eMultimedia
        /// - eCommunications</param>
        /// <param name="deviceId">Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated,
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        HResult IMMNotificationClient.OnDefaultDeviceChanged(DataFlow dataFlow, Role deviceRole, string deviceId)
        {
            var token = _wasapiDeviceTokenFactory.GetToken(deviceId);
            var type = dataFlow.ConvertToFundamentalDeviceState();
            var role = deviceRole.ConvertToFundamentalDeviceRole();
            DefaultDeviceChanged?.Invoke(this, new DefaultDeviceChangedEventArgs(role, type, token));
            return HResult.S_OK;
        }

        /// <summary>
        /// The OnPropertyValueChanged method indicates that the value of a property belonging to an audio endpoint device has changed.
        /// </summary>
        /// <param name="deviceId">Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated,
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.</param>
        /// <param name="key">A PROPERTYKEY structure that specifies the property. The structure contains the property-set GUID and an index identifying a
        /// property within the set. The structure is passed by value. It remains valid for the duration of the call. For more information
        /// about PROPERTYKEY, see the Windows SDK documentation.</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        HResult IMMNotificationClient.OnPropertyValueChanged(string deviceId, PropertyKey key)
        {
            var token = _wasapiDeviceTokenFactory.GetToken(deviceId);
            DevicePropertyChanged?.Invoke(this, new DevicePropertyChangedEventArgs(token, key));
            return HResult.S_OK;
        }

        #endregion

    }
}
