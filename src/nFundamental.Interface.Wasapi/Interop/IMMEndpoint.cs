using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{
    [Guid("1BE09788-6894-4089-8586-9A2A6C265AC5"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMEndpoint
    {
        [PreserveSig]
        HResult GetDataFlow(out DataFlow dataFlow);
    }
}
