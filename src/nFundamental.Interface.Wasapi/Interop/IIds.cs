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


        public const string IAudioCaptureClient = "C8ADBD64-E71E-48a0-A4DE-185C395CD317";

        public static Guid IAudioCaptureClientGuid = Guid.Parse(IAudioCaptureClient);


        public const string IAudioRenderClient = "F294ACFC-3146-4483-A7BF-ADDCA7C260E2";

        public static Guid IIAudioRenderClientGuid = Guid.Parse(IAudioRenderClient);
    }
}
