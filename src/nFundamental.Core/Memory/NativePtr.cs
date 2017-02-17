using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Fundamental.Core.Memory
{
    public abstract class NativePtr : IDisposable
    {

        /// <summary>
        /// Gets the null.
        /// </summary>
        /// <value>
        /// The null.
        /// </value>
        public static NullPtr Null => NullPtr.Value;

        /// <summary>
        /// The underlying pointer
        /// </summary>
        private IntPtr _ptr;

        /// <summary>
        /// Gets the Pointer.
        /// </summary>
        public IntPtr Ptr => _ptr;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativePtr"/> class.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        protected NativePtr(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dealloc();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NativePtr"/> to <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator IntPtr(NativePtr ptr)
        {
            return ptr._ptr;
        }

        /// <summary>
        /// Gets the pointer Located at the pointer
        /// </summary>
        /// <returns></returns>
        public abstract NativePtr AttachPointerAtPointer();

        /// <summary>
        /// PTRs to structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PtrToStructure<T>()
        {
#if NETSTANDARD1_0
        return Marshal.PtrToStructure<T>(_ptr);
#else
        return (T)Marshal.PtrToStructure(_ptr, typeof(T));
#endif
        }

        /// <summary>
        /// Deallocs this instance.
        /// </summary>
        private void Dealloc()
        {
            var ptr = Interlocked.Exchange(ref _ptr, IntPtr.Zero);
            if (ptr == IntPtr.Zero)
                return;
            Dealloc(ptr);
        }

        /// <summary>
        /// Deallocs the specified pointer.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        protected abstract void Dealloc(IntPtr ptr);


        /// <summary>
        /// Copies the given struct or delete on fail.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="structure">The structure.</param>
        /// <param name="ptr">The PTR.</param>
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

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(NativePtr a, NativePtr b)
        {
            // If both are null, or both are same instance, return true.
            // If one is null, but not both, return false.
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            
            return a._ptr == b._ptr;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(NativePtr a, NativePtr b)
        {
            return !(a == b);
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is NativePtr))
                return false;

            var other = (NativePtr)obj;
            return other._ptr == _ptr;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _ptr.GetHashCode();
        }
    }

    public class NullPtr : NativePtr
    {

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public static NullPtr Value { get; } = new NullPtr();

        /// <summary>
        /// Initializes a new instance of the <see cref="NullPtr"/> class.
        /// </summary>
        private NullPtr() : base(IntPtr.Zero)
        {
        }

        /// <summary>
        /// Gets the pointer Located at the pointer
        /// </summary>
        /// <returns></returns>
        public override NativePtr AttachPointerAtPointer()
        {
            return Value;
        }

        /// <summary>
        /// Deallocates the specified pointer.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        protected override void Dealloc(IntPtr ptr)
        {
        }
    }
}
