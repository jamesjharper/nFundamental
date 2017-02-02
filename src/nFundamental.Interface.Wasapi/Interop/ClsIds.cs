using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    public static class ClsIds
    {
        /// <summary>
        /// The device enumerator COM Class Identifier
        /// </summary>
        public const string MmDeviceEnumerator = "BCDE0395-E52F-467C-8E3D-C4579291692E";

        public static Guid MmDeviceEnumeratorGuid = new Guid(MmDeviceEnumerator);

    }
}
