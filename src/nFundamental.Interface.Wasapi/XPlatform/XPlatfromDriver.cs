using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Mono
{
    public abstract class XPlatfromDriver
    {
        internal abstract IntPtr InitializeDriver();

        internal abstract void ShutdownDriver(IntPtr token);

        /// <summary>
        /// Creates a message only window.
        /// </summary>
        /// <param name="cp">The cp.</param>
        /// <returns>The window handle</returns>
        internal abstract IntPtr CreateMessageOnlyWindow(CreateParams cp);

        /// <summary>
        /// Destroys the given window.
        /// </summary>
        /// <param name="handle">The handle.</param>
        internal abstract void DestroyWindow(IntPtr handle);

        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="msg">The proc message</param>
        /// <returns></returns>
        internal abstract IntPtr InvokeDefaultWindowProc(ref Message msg);


        internal abstract int RegisterMessageId(string messageName);
    }
}
