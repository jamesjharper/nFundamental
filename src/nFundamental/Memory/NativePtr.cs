using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Fundamental.Basic
{
    public abstract class NativePtr : IDisposable
    {
        private IntPtr _ptr;

        public IntPtr Ptr => _ptr;

        protected NativePtr(IntPtr ptr)
        {
            _ptr = ptr;
        }

        public void Dispose()
        {
            Dealloc();
        }

        private void Dealloc()
        {
            var ptr = Interlocked.Exchange(ref _ptr, IntPtr.Zero);
            if (ptr == IntPtr.Zero)
                return;
            Dealloc(ptr);
        }

        protected abstract void Dealloc(IntPtr ptr);


        protected static void CopyOrDeleteOnFail<T>(T structure, NativePtr ptr)
        {
            try
            {
                Marshal.StructureToPtr(structure, ptr.Ptr, false);
            }
            catch (Exception)
            {
                ptr.Dispose();
                throw;
            }
        }
    }
}
