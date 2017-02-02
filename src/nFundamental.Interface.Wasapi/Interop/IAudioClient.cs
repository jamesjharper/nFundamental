using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{
    [ComImport]
    [Guid(IIds.IAudioClient)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioClient
    {

        [PreserveSig]
        HResult Initialize( [In] [MarshalAs(UnmanagedType.I4)] AudioClientShareMode shareMode,
                            [In] [MarshalAs(UnmanagedType.U4)] AudioClientStreamFlags streamFlags,
                            [In] [MarshalAs(UnmanagedType.U8)] UInt64 bufferDuration,
                            [In] [MarshalAs(UnmanagedType.U8)] UInt64 devicePeriod,
                            [In] [MarshalAs(UnmanagedType.SysInt)] IntPtr format,
                            [In, Optional] [MarshalAs(UnmanagedType.LPStruct)] Guid audioSessionId);

        [PreserveSig]
        HResult GetBufferSize([Out] [MarshalAs(UnmanagedType.U4)] out UInt32 size);

        [PreserveSig]
        HResult GetStreamLatency([Out] [MarshalAs(UnmanagedType.U8)] out UInt64 latency);

        [PreserveSig]
        HResult GetCurrentPadding([Out] [MarshalAs(UnmanagedType.U4)] out UInt32 frameCount);

        [PreserveSig]
        HResult IsFormatSupported( [In] [MarshalAs(UnmanagedType.I4)] AudioClientShareMode shareMode,
                                   [In] [MarshalAs(UnmanagedType.SysInt)] IntPtr format, 
                                   [Out] out IntPtr closestMatch);

        [PreserveSig]
        HResult GetMixFormat([Out] [MarshalAs(UnmanagedType.SysInt)] out IntPtr format); 

        [PreserveSig]
        HResult GetDevicePeriod([Out] [MarshalAs(UnmanagedType.U8)] out UInt64 processInterval,
                                [Out] [MarshalAs(UnmanagedType.U8)] out UInt64 minimumInterval);

        [PreserveSig]
        HResult Start();

        [PreserveSig]
        HResult Stop();

        [PreserveSig]
        HResult Reset();

        [PreserveSig]
        HResult SetEventHandle([In] [MarshalAs(UnmanagedType.SysInt)] IntPtr handle);

        [PreserveSig]
        HResult GetService([In] [MarshalAs(UnmanagedType.LPStruct)] Guid interfaceId,
                           [Out] [MarshalAs(UnmanagedType.IUnknown)] out object instancePtr);
    }
}
