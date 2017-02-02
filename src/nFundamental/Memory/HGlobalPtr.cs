using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Fundamental.Basic
{
    public class HGlobalPtr : NativePtr
    {
        private HGlobalPtr(IntPtr ptr) : base(ptr) { }

        public static NativePtr Alloc(int size)
        {
            var ptr = Marshal.AllocHGlobal(size);
            return new HGlobalPtr(ptr);
        }

        public static NativePtr CopyToPtr<T>(T structure)
        {
            var ptr = Alloc(Marshal.SizeOf(structure));
            CopyOrDeleteOnFail(structure, ptr);
            return ptr;
        }

        protected override void Dealloc(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}
