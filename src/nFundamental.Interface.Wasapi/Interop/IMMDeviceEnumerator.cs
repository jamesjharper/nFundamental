using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{


    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceEnumerator
    {
        [PreserveSig]
        HResult EnumAudioEndpoints([In] DataFlow dataFlow, [In] DeviceState stateMask, [Out] out IMMDeviceCollection deviceCollection);

        [PreserveSig]
        HResult GetDefaultAudioEndpoint([In] DataFlow dataFlow, [In]Role role, [Out] out IMMDevice device);

        [PreserveSig]
        HResult GetDevice([In, MarshalAs(UnmanagedType.LPWStr)] string id, [Out] out IMMDevice device);

        [PreserveSig]
        HResult RegisterEndpointNotificationCallback([In] IMMNotificationClient notificationClient);

        [PreserveSig]
        HResult UnregisterEndpointNotificationCallback([In] IMMNotificationClient notificationClient);
    }
}
