using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Fundamental.Basic
{
    public class CoTaskMemPtr : NativePtr
    {
        private CoTaskMemPtr(IntPtr ptr) : base(ptr) { }

        public static NativePtr Alloc(int size)
        {
            var ptr = Marshal.AllocCoTaskMem(size);
            return new CoTaskMemPtr(ptr);
        }

        public static NativePtr CopyToPtr<T>(T structure)
        {
            var ptr = Alloc(Marshal.SizeOf(structure));
            CopyOrDeleteOnFail(structure, ptr);
            return ptr;
        }

        protected override void Dealloc(IntPtr ptr)
        {
            Marshal.FreeCoTaskMem(ptr);
        }
    }
}
