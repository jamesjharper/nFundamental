using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    [Flags]
    public  enum DataFlow : uint
    {
        Render = 0,
        Capture,
        All
    };
}
