using System;
using System.Collections.Generic;
using System.Linq;
using Fundamental.Interface.Wasapi.Mono;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.XPlatform
{
    public class NativeWindow
    {

        /// <summary>
        /// The window handle
        /// </summary>
        private IntPtr _windowHandle = IntPtr.Zero;

		/// <summary>
		/// The window collection
		/// </summary>
		private static readonly Dictionary<IntPtr, List<NativeWindow>> WindowCollection = new Dictionary<IntPtr, List<NativeWindow>>();

		[ThreadStatic]
		static NativeWindow _windowCreating;


		/// <summary>
		/// Gets the handle.
		/// </summary>
		/// <value>
		/// The handle.
		/// </value>
		public IntPtr Handle => _windowHandle;

		/// <summary>
		/// Froms the handle.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		public static NativeWindow FromHandle(IntPtr handle)
		{
			return FindFirstInTable (handle);
		}


		#region Private and Internal Methods

		/// <summary>
		/// Invalidates the handle.
		/// </summary>
		internal void InvalidateHandle()
		{
			RemoveFromTable (this);
			_windowHandle = IntPtr.Zero;
		}

		#endregion

		#region Public Instance Methods

		/// <summary>
		/// Assigns the handle.
		/// </summary>
		/// <param name="handle">The handle.</param>
		public void AssignHandle(IntPtr handle)
		{
			RemoveFromTable (this);
			_windowHandle = handle;
			AddToTable (this);
			OnHandleChange();
		}

		/// <summary>
		/// Adds to table.
		/// </summary>
		/// <param name="window">The window.</param>
		private static void AddToTable (NativeWindow window)
		{
			var handle = window.Handle;
			if (handle == IntPtr.Zero)
				return;

			lock (WindowCollection)
			{

			    List<NativeWindow> match;
			    if (!WindowCollection.TryGetValue(handle, out match))
			    {
			        WindowCollection.Add(handle, new List<NativeWindow> {window});
                    return;
			    }

			    if (match.Contains(window))
                    return;

			    match.Add(window);
			}
		}

		/// <summary>
		/// Removes from table.
		/// </summary>
		/// <param name="window">The window.</param>
		private static void RemoveFromTable (NativeWindow window)
		{
			var handle = window.Handle;
			if (handle == IntPtr.Zero)
				return;

			lock (WindowCollection)
			{
                List<NativeWindow> match;
			    if (!WindowCollection.TryGetValue(handle, out match))
                    return;

			    match.Remove(window);
			}
		}

		/// <summary>
		/// Finds the first in table.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		private static NativeWindow FindFirstInTable (IntPtr handle)
		{
            lock (WindowCollection)
			{
                List<NativeWindow> match;
			    return !WindowCollection.TryGetValue(handle, out match) ? null : match.FirstOrDefault();
			}
		}

		/// <summary>
		/// Creates the handle.
		/// </summary>
		/// <param name="cp">The cp.</param>
		public virtual void CreateHandle(CreateParams cp)
		{
		    if (cp == null) 
                return;

		    _windowCreating = this;
		    _windowHandle = XPlatfrom.CreateMessageOnlyWindow(cp);
		    _windowCreating = null;

		    if (_windowHandle != IntPtr.Zero)
		        AddToTable (this);
		}

		/// <summary>
		/// Definitions the WND proc.
		/// </summary>
		/// <param name="m">The m.</param>
		public void DefWndProc(ref Message m)
		{
			m.Result = XPlatfrom.InvokeDefaultWindowProc(ref m);
		}

		/// <summary>
		/// Destroys the handle.
		/// </summary>
		public virtual void DestroyHandle()
		{
			if (_windowHandle != IntPtr.Zero)
             {
				XPlatfrom.DestroyWindow(_windowHandle);
			}
		}

		/// <summary>
		/// Releases the handle.
		/// </summary>
		public virtual void ReleaseHandle()
		{
			RemoveFromTable (this);
			_windowHandle=IntPtr.Zero;
			OnHandleChange();
		}

		#endregion	// Public Instance Methods

		#region Protected Instance Methods

	

	    /// <summary>
	    /// Called when [handle change].
	    /// </summary>
	    protected virtual void OnHandleChange()
		{
		}

		//protected virtual void OnThreadException(Exception e)
		//{
			//Application.OnThreadException(e);
		//}

		/// <summary>
		/// WNDs the proc.
		/// </summary>
		/// <param name="m">The m.</param>
		protected virtual void WndProc(ref Message m)
		{
			DefWndProc(ref m);
		}

		/// <summary>
		/// WNDs the proc.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="msg">The MSG.</param>
		/// <param name="wParam">The w parameter.</param>
		/// <param name="lParam">The l parameter.</param>
		/// <returns></returns>
		internal static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			var result = IntPtr.Zero;
		    var m = new Message
		    {
		        HWnd = hWnd,
		        Msg = msg,
		        WParam = wParam,
		        LParam = lParam,
		        Result = IntPtr.Zero
		    };

		    lock (WindowCollection)
		    {
		        List<NativeWindow> windows;
		        if (!WindowCollection.TryGetValue(hWnd, out windows))
                    return XPlatfrom.InvokeDefaultWindowProc (ref m);


                lock (windows) 
                {
                    if (windows.Count <= 0) 
                        return result;

                    var window = EnsureCreated (windows[0], hWnd);
                    window.WndProc (ref m);

                    // the first one is the control's one. all others are synthetic,
                    // so we want only the result from the control
                    result = m.Result;

                    for (var i = 1; i < windows.Count; i++)
                        windows[i].WndProc (ref m);


                    return result;
                }
		    }
		}

	

		/// <summary>
		/// Ensures the created.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="hWnd">The h WND.</param>
		/// <returns></returns>
		private static NativeWindow EnsureCreated (NativeWindow window, IntPtr hWnd)
		{
			// we need to do this AssignHandle here instead of relying on
			// Control.WndProc to do it, because subclasses can override
			// WndProc, install their own WM_CREATE block, and look at
			// this.Handle, and it needs to be set.  Otherwise, we end up
			// recursively creating windows and emitting WM_CREATE.
		    if (window != null || _windowCreating == null) 
                return window;

		    window = _windowCreating;
		    _windowCreating = null;

		    if (window.Handle == IntPtr.Zero)
		        window.AssignHandle (hWnd);
		    return window;
		}
		#endregion	// Protected Instance Methods
    }
}
