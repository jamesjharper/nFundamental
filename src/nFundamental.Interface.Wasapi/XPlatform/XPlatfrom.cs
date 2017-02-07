using System;
using System.Runtime.InteropServices;
using Fundamental.Core.Memory;
using Fundamental.Interface.Wasapi.Win32;
using Fundamental.Interface.Wasapi.XPlatform;

namespace Fundamental.Interface.Wasapi.Mono
{
    public static class XPlatfrom
    {
        private static readonly XPlatfromDriver Driver;



        #region Constructor & Destructor

        /// <summary>
        /// Initializes the <see cref="XPlatfrom"/> class.
        /// </summary>
        static XPlatfrom()
        {
            Driver = ResolveDriver();
            Driver.InitializeDriver();

            // Initialize things that need to be done after the driver is ready
           // DataFormats.GetFormat(0);

            // Signal that the Application loop can be run.
            // This allows UIA to initialize a11y support for MWF
            // before the main loop begins.
            ///MediaTypeNames.Application.FirePreRun();
        }


        /// <summary>
        /// Resolves the driver.
        /// </summary>
        /// <returns></returns>
        private static XPlatfromDriver ResolveDriver()
        {

            // Compose name with current domain id because on Win32 we register class name
            // and name must be unique to process. If we load MWF into multiple appdomains
            // and try to register same class name we fail.
            //			default_class_name = "SWFClass" + System.Threading.Thread.GetDomainID ().ToString ();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
                return Win32PlatformDriver.GetInstance();


            //if (Environment.GetEnvironmentVariable ("not_supported_MONO_MWF_USE_NEW_X11_BACKEND") != null) {
            //        driver=XplatUIX11_new.GetInstance ();
            //} else 
            if (Environment.GetEnvironmentVariable("MONO_MWF_MAC_FORCE_X11") != null)
                return X11PlatfromDriver.GetInstance();

            if(Uname() == "Darwin")
                return CarbonPlatformDriver.GetInstance();

            return X11PlatfromDriver.GetInstance();
        }

        #endregion // Constructor & Destructor


        /// <summary>
        /// Creates a message only window.
        /// </summary>
        /// <param name="cp">The cp.</param>
        /// <returns>The window handle</returns>
        internal static IntPtr CreateMessageOnlyWindow(CreateParams cp)
        {
            return Driver.CreateMessageOnlyWindow(cp);
        }


        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="msg">The proc message</param>
        /// <returns></returns>
        internal static IntPtr InvokeDefaultWindowProc(ref Message msg)
        {
            return Driver.InvokeDefaultWindowProc(ref msg);
        }

        /// <summary>
        /// Destroys the window.
        /// </summary>
        /// <param name="handle">The handle.</param>
        internal static void DestroyWindow(IntPtr handle)
        {
            Driver.DestroyWindow(handle);
        }

        internal static string Uname()
        {
            using (var pBuffer = HGlobalPtr.Alloc(8192))
            {
                return uname(pBuffer) != 0 ? string.Empty : Marshal.PtrToStringAnsi(pBuffer);
            }
        }


        [DllImport("libc")]
        static extern int uname(IntPtr buf);
    }
}
