using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{
    [ComImport]
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDevice 
    {

        HResult Activate([In] Guid iid, [In] ClsCtx clsctx, [In] IntPtr activationParams /*zero*/, [Out, MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);

        HResult OpenPropertyStore([In] StorageAccess access, [Out] out IPropertyStore propertystore);

        HResult GetId([Out, MarshalAs(UnmanagedType.LPWStr)] out string deviceId);

        HResult GetState([Out] out DeviceState state);
    }
}
