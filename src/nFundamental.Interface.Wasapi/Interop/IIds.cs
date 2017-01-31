using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    public class IIds
    {
        /// <summary>
        /// The Audio Client COM Interface Identifier
        /// </summary>
        public const string IAudioClient = "1CB9AD4C-DBFA-4c32-B178-C2F568A703B2";

        public static Guid IAudioClientGuid = Guid.Parse(IAudioClient);

    }
}
