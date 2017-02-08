using System;
using System.Runtime.InteropServices;

namespace Fundamental.Core.Memory
{
    public class CoTaskMemPtr : NativePtr
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HGlobalPtr"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        private CoTaskMemPtr(int size) : base(Marshal.AllocCoTaskMem(size)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HGlobalPtr"/> class.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        private CoTaskMemPtr(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// Attaches the specified Pointer to the scoped instance.
        /// </summary>
        /// <param name="ptr">The pointer.</param>
        /// <returns></returns>
        public static NativePtr Attach(IntPtr ptr)
        {
            return new CoTaskMemPtr(ptr);
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
            return new CoTaskMemPtr(size);
        }

        /// <summary>
        /// Deallocs the specified PTR.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        protected override void Dealloc(IntPtr ptr)
        {
            Marshal.FreeCoTaskMem(ptr);
        }


        /// <summary>
        /// Gets the pointer Located at the pointer
        /// </summary>
        /// <returns></returns>
        public override NativePtr AttachPointerAtPointer()
        {
            var p = PtrToStructure<IntPtr>();
            return new CoTaskMemPtr(p);
        }
    }
}
