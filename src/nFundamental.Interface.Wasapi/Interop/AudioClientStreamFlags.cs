using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    [Flags]
    public enum AudioClientStreamFlags
    {
        None,
        CrossProcess    = 0x00010000,
        Loopback        = 0x00020000,
        EventCallback   = 0x00040000,
        NoPersist       = 0x00080000,
    }
}
