using System;
using System.Runtime.InteropServices;

namespace Fundamental.Core.Memory
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
