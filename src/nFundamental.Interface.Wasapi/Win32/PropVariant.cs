using System;
using System.Runtime.InteropServices;

namespace Fundamental.Interface.Wasapi.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct PropArray
    {
        internal UInt32 cElems;
        internal IntPtr pElems;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PropVariant
    {
        public static PropVariant Empty = FromObject(null);


        [FieldOffset(0)] private ushort varType;
        [FieldOffset(2)] private ushort wReserved1;
        [FieldOffset(4)] private ushort wReserved2;
        [FieldOffset(6)] private ushort wReserved3;

        [FieldOffset(8)] private byte bVal;
        [FieldOffset(8)] private sbyte cVal;
        [FieldOffset(8)] private ushort uiVal;
        [FieldOffset(8)] private short iVal;
        [FieldOffset(8)] private UInt32 uintVal;
        [FieldOffset(8)] private Int32 intVal;
        [FieldOffset(8)] private UInt64 ulVal;
        [FieldOffset(8)] private Int64 lVal;
        [FieldOffset(8)] private float fltVal;
        [FieldOffset(8)] private double dblVal;
        [FieldOffset(8)] private short boolVal;
        [FieldOffset(8)] private IntPtr pclsidVal; //this is for GUID ID pointer
        [FieldOffset(8)] private IntPtr pszVal; //this is for ansi string pointer
        [FieldOffset(8)] private IntPtr pwszVal; //this is for Unicode string pointer
        [FieldOffset(8)] private IntPtr punkVal; //this is for punkVal (interface pointer)
        [FieldOffset(8)] private PropArray ca;
        [FieldOffset(8)] private System.Runtime.InteropServices.ComTypes.FILETIME filetime;




        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public VariantType ValueType => (VariantType)varType;

        /// <summary>
        /// Determines whether [is variant type supported].
        /// </summary>
        /// <returns></returns>
        public bool IsVariantTypeSupported()
        {
            var vt = (VariantType)varType;

            if ((vt & VariantType.VT_VECTOR) != 0)
            {
                switch (vt & (~VariantType.VT_VECTOR))
                {
                    case VariantType.VT_EMPTY:
                        return true;

                    case VariantType.VT_I1:
                        return true;

                    case VariantType.VT_UI1:
                        return true;

                    case VariantType.VT_I2:
                        return true;

                    case VariantType.VT_UI2:
                        return true;

                    case VariantType.VT_I4:
                        return true;

                    case VariantType.VT_UI4:
                        return true;

                    case VariantType.VT_I8:
                        return true;

                    case VariantType.VT_UI8:
                        return true;

                    case VariantType.VT_R4:
                        return true;

                    case VariantType.VT_R8:
                        return true;

                    case VariantType.VT_BOOL:
                        return true;

                    case VariantType.VT_CLSID:
                        return true;

                    case VariantType.VT_LPSTR:
                        return true;

                    case VariantType.VT_LPWSTR:
                        return true;
                }
            }
            else
            {
                switch (vt)
                {
                    case VariantType.VT_EMPTY:
                        return true;

                    case VariantType.VT_I1:
                        return true;

                    case VariantType.VT_UI1:
                        return true;

                    case VariantType.VT_I2:
                        return true;

                    case VariantType.VT_UI2:
                        return true;

                    case VariantType.VT_I4:
                        return true;

                    case VariantType.VT_UI4:
                        return true;

                    case VariantType.VT_I8:
                        return true;

                    case VariantType.VT_UI8:
                        return true;

                    case VariantType.VT_R4:
                        return true;

                    case VariantType.VT_R8:
                        return true;

                    case VariantType.VT_FILETIME:
                        return true;

                    case VariantType.VT_BOOL:
                        return true;

                    case VariantType.VT_CLSID:
                        return true;

                    case VariantType.VT_LPSTR:
                        return true;

                    case VariantType.VT_LPWSTR:
                        return true;
                    case VariantType.VT_BLOB:
                        return true;
                    case VariantType.VT_NULL:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a prop variant from a given object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static PropVariant FromObject(object source)
        {
            var propVariant = new PropVariant();
            propVariant.Init(source);
            return propVariant;
        }

        /// <summary>
        /// To the object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public object ToObject()
        {
            var vt = (VariantType) varType;

            if ((vt & VariantType.VT_VECTOR) != 0)
            {
                switch (vt & (~VariantType.VT_VECTOR))
                {
                    case VariantType.VT_EMPTY:
                        return null;

                    case VariantType.VT_I1:
                    {
                        var array = new sbyte[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                            array[i] = (sbyte) Marshal.ReadByte(ca.pElems, i);
                        return array;
                    }

                    case VariantType.VT_UI1:
                    {
                        var array = new byte[ca.cElems];
                        Marshal.Copy(ca.pElems, array, 0, (int) ca.cElems);
                        return array;
                    }

                    case VariantType.VT_I2:
                    {
                        var array = new short[ca.cElems];
                        Marshal.Copy(ca.pElems, array, 0, (int) ca.cElems);
                        return array;
                    }

                    case VariantType.VT_UI2:
                    {
                        var array = new ushort[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                            array[i] = (ushort) Marshal.ReadInt16(ca.pElems, i*sizeof(ushort));
                        return array;
                    }

                    case VariantType.VT_I4:
                    {
                        var array = new int[ca.cElems];
                        Marshal.Copy(ca.pElems, array, 0, (int) ca.cElems);
                        return array;
                    }

                    case VariantType.VT_UI4:
                    {
                        var array = new uint[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                            array[i] = (uint) Marshal.ReadInt32(ca.pElems, i*sizeof(uint));
                        return array;
                    }

                    case VariantType.VT_I8:
                    {
                        var array = new Int64[ca.cElems];
                        Marshal.Copy(ca.pElems, array, 0, (int) ca.cElems);
                        return array;
                    }

                    case VariantType.VT_UI8:
                    {
                        var array = new UInt64[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                            array[i] = (UInt64) Marshal.ReadInt64(ca.pElems, i*sizeof(UInt64));
                        return array;
                    }

                    case VariantType.VT_R4:
                    {
                        var array = new float[ca.cElems];
                        Marshal.Copy(ca.pElems, array, 0, (int) ca.cElems);
                        return array;
                    }

                    case VariantType.VT_R8:
                    {
                        var array = new double[ca.cElems];
                        Marshal.Copy(ca.pElems, array, 0, (int) ca.cElems);
                        return array;
                    }

                    case VariantType.VT_BOOL:
                    {
                        var array = new bool[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                            array[i] = (bool) (Marshal.ReadInt16(ca.pElems, i*sizeof(ushort)) != 0);
                        return array;
                    }

                    case VariantType.VT_CLSID:
                    {
                        var array = new Guid[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                        {
                            var guid = new byte[16];
                            Marshal.Copy(ca.pElems, guid, i*16, 16);
                            array[i] = new Guid(guid);
                        }
                        return array;
                    }

                    case VariantType.VT_LPSTR:
                    {
                        var array = new string[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                        {
                            var ptr = Marshal.ReadIntPtr(ca.pElems, i*IntPtr.Size);
                            array[i] = Marshal.PtrToStringAnsi(ptr);
                        }
                        return array;
                    }

                    case VariantType.VT_LPWSTR:
                    {
                        var array = new string[ca.cElems];
                        for (var i = 0; i < ca.cElems; i++)
                        {
                            var ptr = Marshal.ReadIntPtr(ca.pElems, i*IntPtr.Size);
                            array[i] = Marshal.PtrToStringUni(ptr);
                        }
                        return array;
                    }

                    case VariantType.VT_UNKNOWN:
                    default:
                        break;
                }
            }
            else
            {
                switch (vt)
                {
                    case VariantType.VT_EMPTY:
                        return null;
                    case VariantType.VT_NULL:
                        return null;

                    case VariantType.VT_I1:
                        return cVal;
                    case VariantType.VT_UI1:
                        return bVal;

                    case VariantType.VT_I2:
                        return iVal;

                    case VariantType.VT_UI2:
                        return uiVal;

                    case VariantType.VT_I4:
                        return intVal;

                    case VariantType.VT_UI4:
                        return uintVal;

                    case VariantType.VT_I8:
                        return lVal;

                    case VariantType.VT_UI8:
                        return ulVal;

                    case VariantType.VT_R4:
                        return fltVal;

                    case VariantType.VT_R8:
                        return dblVal;

                    case VariantType.VT_FILETIME:
                        return filetime;

                    case VariantType.VT_BOOL:
                        return (bool) (boolVal != 0);

                    case VariantType.VT_CLSID:
                        var guid = new byte[16];
                        Marshal.Copy(pclsidVal, guid, 0, 16);
                        return new Guid(guid);

                    case VariantType.VT_LPSTR:
                        return Marshal.PtrToStringAnsi(pszVal);

                    case VariantType.VT_LPWSTR:
                        return Marshal.PtrToStringUni(pwszVal);

                    case VariantType.VT_BLOB:
                    {
                        var blob = new byte[ca.cElems];
                        Marshal.Copy(ca.pElems, blob, 0, (int) ca.cElems);
                        return blob;
                    }
                }
            }

            throw new System.NotSupportedException();
        }


        /// <SecurityNote>
        /// Critical -Accesses unmanaged code and structure is overlapping in memory
        /// TreatAsSafe - inputs are verified or safe
        /// </SecurityNote>
        private void Init(object value)
        {
            if (value == null)
            {
                varType = (ushort) VariantType.VT_EMPTY;
            }
            else if (value is Array)
            {
                var type = value.GetType();

                if (type == typeof(sbyte[]))
                {
                    InitVector(value as Array, typeof(sbyte), VariantType.VT_I1);
                }
                else if (type == typeof(byte[]))
                {
                    InitVector(value as Array, typeof(byte), VariantType.VT_UI1);
                }
                else if (value is char[])
                {
                    varType = (ushort) VariantType.VT_LPSTR;
                    pszVal = Marshal.StringToCoTaskMemAnsi(new string(value as char[]));
                }
                else if (value is char[][])
                {
                    var charArray = value as char[][];

                    var strArray = new string[charArray.GetLength(0)];

                    for (var i = 0; i < charArray.Length; i++)
                    {
                        strArray[i] = new string(charArray[i] as char[]);
                    }

                    Init(strArray, true);
                }
                else if (type == typeof(short[]))
                {
                    InitVector(value as Array, typeof(short), VariantType.VT_I2);
                }
                else if (type == typeof(ushort[]))
                {
                    InitVector(value as Array, typeof(ushort), VariantType.VT_UI2);
                }
                else if (type == typeof(int[]))
                {
                    InitVector(value as Array, typeof(int), VariantType.VT_I4);
                }
                else if (type == typeof(uint[]))
                {
                    InitVector(value as Array, typeof(uint), VariantType.VT_UI4);
                }
                else if (type == typeof(Int64[]))
                {
                    InitVector(value as Array, typeof(Int64), VariantType.VT_I8);
                }
                else if (type == typeof(UInt64[]))
                {
                    InitVector(value as Array, typeof(UInt64), VariantType.VT_UI8);
                }
                else if (value is float[])
                {
                    InitVector(value as Array, typeof(float), VariantType.VT_R4);
                }
                else if (value is double[])
                {
                    InitVector(value as Array, typeof(double), VariantType.VT_R8);
                }
                else if (value is Guid[])
                {
                    InitVector(value as Array, typeof(Guid), VariantType.VT_CLSID);
                }
                else if (value is string[])
                {
                    Init(value as string[], false);
                }
                else if (value is bool[])
                {
                    var boolArray = value as bool[];
                    var array = new short[boolArray.Length];

                    for (var i = 0; i < boolArray.Length; i++)
                    {
                        array[i] = (short) (boolArray[i] ? -1 : 0);
                    }

                    InitVector(array, typeof(short), VariantType.VT_BOOL);
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            else
            {
                var type = value.GetType();

                if (value is string)
                {
                    varType = (ushort) VariantType.VT_LPWSTR;
                    pwszVal = Marshal.StringToCoTaskMemUni(value as string);
                }
                else if (type == typeof(sbyte))
                {
                    varType = (ushort) VariantType.VT_I1;
                    cVal = (sbyte) value;
                }
                else if (type == typeof(byte))
                {
                    varType = (ushort) VariantType.VT_UI1;
                    bVal = (byte) value;
                }
                else if (type == typeof(System.Runtime.InteropServices.ComTypes.FILETIME))
                {
                    varType = (ushort) VariantType.VT_FILETIME;
                    filetime = (System.Runtime.InteropServices.ComTypes.FILETIME) value;
                }
                else if (value is char)
                {
                    varType = (ushort) VariantType.VT_LPSTR;
                    pszVal = Marshal.StringToCoTaskMemAnsi(new string(new char[] {(char) value}));
                }
                else if (type == typeof(short))
                {
                    varType = (ushort) VariantType.VT_I2;
                    iVal = (short) value;
                }
                else if (type == typeof(ushort))
                {
                    varType = (ushort) VariantType.VT_UI2;
                    uiVal = (ushort) value;
                }
                else if (type == typeof(int))
                {
                    varType = (ushort) VariantType.VT_I4;
                    intVal = (int) value;
                }
                else if (type == typeof(uint))
                {
                    varType = (ushort) VariantType.VT_UI4;
                    uintVal = (uint) value;
                }
                else if (type == typeof(Int64))
                {
                    varType = (ushort) VariantType.VT_I8;
                    lVal = (Int64) value;
                }
                else if (type == typeof(UInt64))
                {
                    varType = (ushort) VariantType.VT_UI8;
                    ulVal = (UInt64) value;
                }
                else if (value is float)
                {
                    varType = (ushort) VariantType.VT_R4;
                    fltVal = (float) value;
                }
                else if (value is double)
                {
                    varType = (ushort) VariantType.VT_R8;
                    dblVal = (double) value;
                }
                else if (value is Guid)
                {
                    var guid = ((Guid) value).ToByteArray();
                    varType = (ushort) VariantType.VT_CLSID;
                    pclsidVal = Marshal.AllocCoTaskMem(guid.Length);
                    Marshal.Copy(guid, 0, pclsidVal, guid.Length);
                }
                else if (value is bool)
                {
                    varType = (ushort) VariantType.VT_BOOL;
                    boolVal = (short) (((bool) value) ? -1 : 0);
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
        }

        internal void InitVector(Array array, Type type, VariantType variantType)
        {
            Init(array, type, variantType | VariantType.VT_VECTOR);
        }

        /// <SecurityNote>
        /// Critical -Accesses unmanaged code and structure is overlapping in memory
        /// TreatAsSafe - inputs are verified or safe
        /// </SecurityNote>
        internal void Init(Array array, Type type, VariantType vt)
        {
            varType = (ushort) vt;
            ca.cElems = 0;
            ca.pElems = IntPtr.Zero;

            var length = array.Length;

            if (length > 0)
            {
                throw new NotImplementedException();
                //long size = Marshal.SizeOf(type) * length;

                //var destPtr = IntPtr.Zero;
                //var handle = new GCHandle();

                //try
                //{
                //    destPtr = Marshal.AllocCoTaskMem((int) size);
                //    handle = GCHandle.Alloc(array, GCHandleType.Pinned);

                //    // TODO: Fix copy to not use unsafe
                    
                //    //Marshal.Copy();
                //    //unsafe
                //    //{
                //    //    CopyBytes((byte*)destPtr, (int)size, (byte*)handle.AddrOfPinnedObject(), (int)size);
                //    //}

                //    ca.cElems = (uint) length;
                //    ca.pElems = destPtr;

                //    destPtr = IntPtr.Zero;
                //}
                //finally
                //{
                //    if (handle.IsAllocated)
                //    {
                //        handle.Free();
                //    }

                //    if (destPtr != IntPtr.Zero)
                //    {
                //        Marshal.FreeCoTaskMem(destPtr);
                //    }
                //}
            }
        }

        /// <SecurityNote>
        /// Critical -Accesses unmanaged code and structure is overlapping in memory
        /// TreatAsSafe - inputs are verified or safe
        /// </SecurityNote>
        private void Init(string[] value, bool fAscii)
        {
            varType = (ushort) (fAscii ? VariantType.VT_LPSTR : VariantType.VT_LPWSTR);
            varType |= (ushort) VariantType.VT_VECTOR;
            ca.cElems = 0;
            ca.pElems = IntPtr.Zero;

            var length = value.Length;

            if (length > 0)
            {
                var destPtr = IntPtr.Zero;
                var sizeIntPtr = IntPtr.Size;
                long size = sizeIntPtr*length;
                var index = 0;

                try
                {
                    destPtr = Marshal.AllocCoTaskMem((int) size);

                    for (index = 0; index < length; index++)
                    {
                        IntPtr pString;
                        if (fAscii)
                        {
                            pString = Marshal.StringToCoTaskMemAnsi(value[index]);
                        }
                        else
                        {
                            pString = Marshal.StringToCoTaskMemUni(value[index]);
                        }
                        Marshal.WriteIntPtr(destPtr, (int) index*sizeIntPtr, pString);
                    }

                    ca.cElems = (uint) length;
                    ca.pElems = destPtr;
                    destPtr = IntPtr.Zero;
                }
                finally
                {
                    if (destPtr != IntPtr.Zero)
                    {
                        for (var i = 0; i < index; i++)
                        {
                            var pString = Marshal.ReadIntPtr(destPtr, i*sizeIntPtr);
                            Marshal.FreeCoTaskMem(pString);
                        }

                        Marshal.FreeCoTaskMem(destPtr);
                    }
                }
            }
        }
    }
}
