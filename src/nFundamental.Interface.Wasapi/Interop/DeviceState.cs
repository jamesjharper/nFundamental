using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    [Flags]
    public enum DeviceState : uint
    {
        Active = 0x1,
        Disabled = 0x2,
        NotPresent = 0x4,
        Unplugged = 0x8,
        All = 0xf
    }
}
