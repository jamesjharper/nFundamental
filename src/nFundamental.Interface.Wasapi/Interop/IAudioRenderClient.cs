using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{
    [ComImport]
    [Guid(IIds.IAudioRenderClient)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioRenderClient
    {
        [PreserveSig]
        HResult GetBuffer(int numFramesRequested, out IntPtr dataBufferPointer);

        [PreserveSig]
        HResult ReleaseBuffer(int numFramesWritten, AudioClientBufferFlags bufferFlags);
    }
}
