using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{

    [ComImport]
    [Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMNotificationClient
    {
        /// <summary>
        /// The OnDeviceStateChanged method indicates that the state of an audio endpoint device has changed.
        /// </summary>
        /// <param name="deviceId">
        /// Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated, 
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.
        /// </param>
        /// <param name="deviceState">
        /// Specifies the new state of the endpoint device. The value of this parameter is one of the following DEVICE_STATE_XXX constants:
        /// DEVICE_STATE_ACTIVE
        /// DEVICE_STATE_DISABLED
        /// DEVICE_STATE_NOTPRESENT
        /// DEVICE_STATE_UNPLUGGED
        /// </param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        [PreserveSig]
        HResult OnDeviceStateChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId, [In] Wasapi.Interop.DeviceState deviceState);

        /// <summary>
        /// The OnDeviceAdded method indicates that a new audio endpoint device has been added.
        /// </summary>
        /// <param name="deviceId">
        /// Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated, 
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.
        /// </param>
        /// <returns></returns>
        [PreserveSig]
        HResult OnDeviceAdded([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// The OnDeviceRemoved method indicates that an audio endpoint device has been removed.
        /// </summary>
        /// <param name="deviceId">
        /// Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated, 
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.
        /// </param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        [PreserveSig]
        HResult OnDeviceRemoved([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// The OnDefaultDeviceChanged method notifies the client that the default audio endpoint device for a particular device role has changed.
        /// </summary>
        /// <param name="dataFlow">
        ///  The data-flow direction of the endpoint device. This parameter is set to one of the following EDataFlow enumeration values:
        ///   - eRender
        ///   - eCapture
        /// The data-flow direction for a rendering device is eRender.The data-flow direction for a capture device is eCapture.
        /// </param>
        /// <param name="role">
        /// The device role of the audio endpoint device. This parameter is set to one of the following ERole enumeration values:
        ///   - eConsole
        ///   - eMultimedia
        ///   - eCommunications
        /// </param>
        /// <param name="deviceId">
        /// Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated, 
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.
        /// </param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        [PreserveSig]
        HResult OnDefaultDeviceChanged([In] DataFlow dataFlow, [In] Role role, [In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// The OnPropertyValueChanged method indicates that the value of a property belonging to an audio endpoint device has changed.
        /// </summary>
        /// <param name="deviceId">
        /// Pointer to the endpoint ID string that identifies the audio endpoint device. This parameter points to a null-terminated, 
        /// wide-character string containing the endpoint ID. The string remains valid for the duration of the call.
        /// </param>
        /// <param name="key">
        /// A PROPERTYKEY structure that specifies the property. The structure contains the property-set GUID and an index identifying a 
        /// property within the set. The structure is passed by value. It remains valid for the duration of the call. For more information
        /// about PROPERTYKEY, see the Windows SDK documentation.
        /// </param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, it returns an error code.
        /// </returns>
        [PreserveSig]
        HResult OnPropertyValueChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId, [In] PropertyKey key);
    }
}
