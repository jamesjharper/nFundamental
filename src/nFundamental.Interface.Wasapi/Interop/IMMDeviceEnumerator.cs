using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{


    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceEnumerator
    {
        HResult EnumAudioEndpoints([In] DataFlow dataFlow, [In] Wasapi.Interop.DeviceState stateMask, [Out] out IMMDeviceCollection deviceCollection);

        HResult GetDefaultAudioEndpoint([In] DataFlow dataFlow, [In]Role role, [Out] out IMMDevice device);

        HResult GetDevice([In, MarshalAs(UnmanagedType.LPWStr)] string id, [Out] out IMMDevice device);

        HResult RegisterEndpointNotificationCallback([In] IMMNotificationClient notificationClient);

        HResult UnregisterEndpointNotificationCallback([In] IMMNotificationClient notificationClient);
    }
}
