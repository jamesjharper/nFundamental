using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Mono;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.XPlatform
{
    public class Win32PlatformDriver : XPlatfromDriver
    {

        #region Native Interop

        [DllImport("user32.dll", EntryPoint = "CreateWindowExW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr Win32CreateWindow(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CallingConvention = CallingConvention.StdCall)]
        internal static extern bool Win32DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr Win32DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "RegisterClassW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private static extern bool Win32RegisterClass(ref WNDCLASS wndClass);

        /// <summary>
        /// The HWND message
        /// </summary>
        public static IntPtr HwndMessage = new IntPtr(-3);

        internal delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WNDCLASS
        {
            internal int style;
            internal WndProc lpfnWndProc;
            internal int cbClsExtra;
            internal int cbWndExtra;
            internal IntPtr hInstance;
            internal IntPtr hIcon;
            internal IntPtr hCursor;
            internal IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpszClassName;
        }

        #endregion

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static Win32PlatformDriver _instance;

        /// <summary>
        /// The reference count of the singleton instance
        /// </summary>
        private static int _refCount;

        /// <summary>
        /// The WindProc callback
        /// </summary>
        internal static WndProc StaticWndProc;

        /// <summary>
        /// Initializes the <see cref="Win32PlatformDriver"/> class.
        /// </summary>
        static Win32PlatformDriver()
        {
            StaticWndProc = InternalWndProc;
        }


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static Win32PlatformDriver GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Win32PlatformDriver();
            }
            _refCount++;
            return _instance;
        }

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public int Reference => _refCount;

        /// <summary>
        /// Initializes the driver.
        /// </summary>
        /// <returns></returns>
        internal override IntPtr InitializeDriver()
        {
            return IntPtr.Zero;
        }

        /// <summary>
        /// Shutdowns the driver.
        /// </summary>
        /// <param name="token">The token.</param>
        internal override void ShutdownDriver(IntPtr token)
        {
        }


        internal override IntPtr CreateMessageOnlyWindow(CreateParams cp)
        {

            var className = RegisterWindowClass();
            var windowHandle = Win32CreateWindow(0, className, string.Empty, 0, 0, 0, 0, 0, HwndMessage, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            if (windowHandle != IntPtr.Zero) 
                return windowHandle;

            var error = Marshal.GetLastWin32Error();
            throw new Win32Exception(error);
        }



        /// <summary>
        /// Destroys the window.
        /// </summary>
        /// <param name="handle">The handle.</param>
        internal override void DestroyWindow(IntPtr handle)
        {
            Win32DestroyWindow(handle);
        }

        /// <summary>
        /// Definitions the WND proc.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        internal override IntPtr InvokeDefaultWindowProc(ref Message msg)
        {
            return Win32DefWindowProc(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
        }


        /// <summary>
        /// Registers the window class.
        /// </summary>
        /// <returns></returns>
        private string RegisterWindowClass()
        {

            var className = string.Format("Mono.{0}.{1}", Guid.NewGuid().ToString());

            var wndClass = new WNDCLASS
            {
                lpfnWndProc = StaticWndProc,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hIcon = IntPtr.Zero,
                hInstance = IntPtr.Zero,
                lpszClassName = className
            };


            bool result = Win32RegisterClass(ref wndClass);

            return className;
        }


        private static IntPtr InternalWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            return NativeWindow.WndProc(hWnd, msg, wParam, lParam);
        }


        
    }
}
