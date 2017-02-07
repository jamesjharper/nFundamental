using System;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Mono
{
    public class X11PlatfromDriver : XPlatfromDriver
    {

        private static X11PlatfromDriver _instance;
        private static int _refCount;

        public static X11PlatfromDriver GetInstance()
        {
            if (_instance == null)
                _instance = new X11PlatfromDriver();
 
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
          //  Hwnd hwnd;
          //  hwnd = Hwnd.ObjectFromHandle(handle);

          //  // The window should never ever be a zombie here, since we should
          //  // wait until it's completely dead before returning from 
          //  // "destroying" calls, but just in case....
          //  if (hwnd == null || hwnd.zombie)
          //  {
          //     // DriverDebug("window {0:X} already destroyed", handle.ToInt32());
          //      return;
          //  }

          ////  DriverDebug("Destroying window {0}", XplatUI.Window(hwnd.client_window));

          //  SendParentNotify(hwnd.Handle, Msg.WM_DESTROY, int.MaxValue, int.MaxValue);

          //  CleanupCachedWindows(hwnd);

          //  ArrayList windows = new ArrayList();

          //  AccumulateDestroyedHandles(Control.ControlNativeWindow.ControlFromHandle(hwnd.Handle), windows);


          //  foreach (Hwnd h in windows)
          //  {
          //      SendMessage(h.Handle, Msg.WM_DESTROY, IntPtr.Zero, IntPtr.Zero);
          //      h.zombie = true;
          //  }

          //  lock (XlibLock)
          //  {
          //      if (hwnd.whole_window != IntPtr.Zero)
          //      {
          //          DriverDebug("XDestroyWindow (whole_window = {0:X})", hwnd.whole_window.ToInt32());
          //          Keyboard.DestroyICForWindow(hwnd.whole_window);
          //          XDestroyWindow(DisplayHandle, hwnd.whole_window);
          //      }
          //      else if (hwnd.client_window != IntPtr.Zero)
          //      {
          //          DriverDebug("XDestroyWindow (client_window = {0:X})", hwnd.client_window.ToInt32());
          //          Keyboard.DestroyICForWindow(hwnd.client_window);
          //          XDestroyWindow(DisplayHandle, hwnd.client_window);
          //      }

          //  }
        }

        internal override IntPtr InvokeDefaultWindowProc(ref Message msg)
        {
            return IntPtr.Zero;
        }
    }
}
