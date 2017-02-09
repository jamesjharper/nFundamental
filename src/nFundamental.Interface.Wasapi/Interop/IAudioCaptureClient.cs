using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{
    [ComImport]
    [Guid(IIds.IAudioCaptureClient)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioCaptureClient
    {
        [PreserveSig]
        HResult GetBuffer( out IntPtr data,
                           out uint numFramesToRead,
                           out AudioClientBufferFlags flags,
                           out UInt64 devicePosition,
                           out UInt64 qpcPosition);

        [PreserveSig]
        HResult ReleaseBuffer(uint numFramesRead);

        [PreserveSig]
        HResult GetNextPacketSize(out uint numFramesInNextPacket);
    }
}
