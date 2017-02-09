using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    /// <summary>
    /// Audio Client Buffer Flags
    /// </summary>
    [Flags]
    public enum AudioClientBufferFlags
    {
        None,
        DataDiscontinuity = 0x1,
        Silent = 0x2,
        TimestampError = 0x4

    }
}
