using System;
using System.Runtime.InteropServices;
using Fundamental.Core.Memory;
using Fundamental.Interface.Wasapi.Mono;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.XPlatform
{
    public class CarbonPlatformDriver : XPlatfromDriver
    {
        private static CarbonPlatformDriver _instance;
        private static int _refCount;

        public static CarbonPlatformDriver GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CarbonPlatformDriver();
            }
            _refCount++;
            return _instance;
        }

        internal override IntPtr InitializeDriver()
        {
            throw new NotImplementedException();
        }

        internal override void ShutdownDriver(IntPtr token)
        {
            throw new NotImplementedException();
        }

        internal override IntPtr CreateMessageOnlyWindow(CreateParams cp)
        {
            throw new NotImplementedException();
        }

        internal override void DestroyWindow(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal override IntPtr InvokeDefaultWindowProc(ref Message msg)
        {
            throw new NotImplementedException();
        }


      
    }
}
