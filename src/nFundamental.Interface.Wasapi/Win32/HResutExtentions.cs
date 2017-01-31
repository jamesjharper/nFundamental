using System.Runtime.InteropServices;

namespace Fundamental.Interface.Wasapi.Win32
{
    public static class HResutExtentions
    {
        public static void ThrowIfFailed(this HResult hr)
        {
            // TODO: handle expected exceptions here
            Marshal.ThrowExceptionForHR((int)hr);
        }
        public static bool IsFailed(this HResult hr)
        {
            return hr != HResult.S_OK;
        }

        public static bool IsSuccess(this HResult hr)
        {
            return hr == HResult.S_OK;
        }
    }
}
