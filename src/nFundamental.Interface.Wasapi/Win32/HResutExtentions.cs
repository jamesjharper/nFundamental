using System.Runtime.InteropServices;

namespace Fundamental.Interface.Wasapi.Win32
{
    public static class HResutExtentions
    {
        public static void ThrowIfFailed(this HResult hr)
        {
            // Wasapi Exceptions;
            //AUDCLNT_E_NOT_INITIALIZED
            //The audio stream has not been successfully initialized.

            //AUDCLNT_E_NOT_STOPPED
            //The audio stream was not stopped at the time the call was made.

            //AUDCLNT_E_BUFFER_OPERATION_PENDING
            //The client is currently writing to or reading from the buffer.

            //AUDCLNT_E_SERVICE_NOT_RUNNING
            //The Windows audio service is not running.
            //if(hr == )

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
