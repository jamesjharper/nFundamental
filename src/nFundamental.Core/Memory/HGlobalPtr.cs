using System;
using System.Runtime.InteropServices;

namespace Fundamental.Core.Memory
{
    public class HGlobalPtr : NativePtr
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HGlobalPtr"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        private HGlobalPtr(int size) : base(Marshal.AllocHGlobal(size)) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="HGlobalPtr"/> class.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        private HGlobalPtr(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// Attaches the specified Pointer to the scoped instance.
        /// </summary>
        /// <param name="ptr">The pointer.</param>
        /// <returns></returns>
        public static NativePtr Attach(IntPtr ptr)
        {
            return new HGlobalPtr(ptr);
        }

        /// <summary>
        /// Copies to PTR.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static NativePtr CopyToPtr(byte[] bytes)
        {
            var ptr = Alloc(Marshal.SizeOf(bytes.Length));
            CopyOrDeleteOnFail(bytes, ptr);
            return ptr;
        }

        /// <summary>
        /// Copies to PTR.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="structure">The structure.</param>
        /// <returns></returns>
        public static NativePtr CopyToPtr<T>(T structure)
        {
            var ptr = Alloc(Marshal.SizeOf(structure));
            CopyOrDeleteOnFail(structure, ptr);
            return ptr;
        }

        /// <summary>
        /// Allocs the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static NativePtr Alloc(int size)
        {
            return new HGlobalPtr(size);
        }

        /// <summary>
        /// Deallocs the specified pointer.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        protected override void Dealloc(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// Gets the pointer Located at the pointer
        /// </summary>
        /// <returns></returns>
        public override NativePtr AttachPointerAtPointer()
        {
            var p = PtrToStructure<IntPtr>();
            return new HGlobalPtr(p);
        }
    }
}
