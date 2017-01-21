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
        /// Notifies the client that the default audio endpoint device for a particular role has changed.
        /// </summary>
        /// <param name="deviceId">
        ///  The data-flow direction of the endpoint device. This parameter is set to one of the following EDataFlow enumeration values:
        ///     - eRender
        ///     - eCapture
        /// The data-flow direction for a rendering device is eRender.The data-flow direction for a capture device is eCapture.
        /// </param>
        /// <param name="deviceState">
        /// The device role of the audio endpoint device. This parameter is set to one of the following ERole enumeration values:
        /// - eConsole
        /// - eMultimedia
        /// - eCommunications
        /// </param>
        /// <returns></returns>
        HResult OnDeviceStateChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId, [In] Wasapi.Interop.DeviceState deviceState);

        HResult OnDeviceAdded([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        HResult OnDeviceRemoved([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        HResult OnDefaultDeviceChanged([In] DataFlow dataFlow, [In] Role role, [In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        HResult OnPropertyValueChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId, [In] PropertyKey key);
    }
}
