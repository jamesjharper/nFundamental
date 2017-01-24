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
        public VarEnum ValueType => (VarEnum)varType;

        /// <summary>
        /// Determines whether [is variant type supported].
        /// </summary>
        /// <returns></returns>
        public bool IsVariantTypeSupported()
        {
            VarEnum vt = (VarEnum)varType;

            if ((vt & VarEnum.VT_VECTOR) != 0)
            {
                switch (vt & (~VarEnum.VT_VECTOR))
                {
                    case VarEnum.VT_EMPTY:
                        return true;

                    case VarEnum.VT_I1:
                        return true;

                    case VarEnum.VT_UI1:
                        return true;

                    case VarEnum.VT_I2:
                        return true;

                    case VarEnum.VT_UI2:
                        return true;

                    case VarEnum.VT_I4:
                        return true;

                    case VarEnum.VT_UI4:
                        return true;

                    case VarEnum.VT_I8:
                        return true;

                    case VarEnum.VT_UI8:
                        return true;

                    case VarEnum.VT_R4:
                        return true;

                    case VarEnum.VT_R8:
                        return true;

                    case VarEnum.VT_BOOL:
                        return true;

                    case VarEnum.VT_CLSID:
                        return true;

                    case VarEnum.VT_LPSTR:
                        return true;

                    case VarEnum.VT_LPWSTR:
                        return true;
                }
            }
            else
            {
                switch (vt)
                {
                    case VarEnum.VT_EMPTY:
                        return true;

                    case VarEnum.VT_I1:
                        return true;

                    case VarEnum.VT_UI1:
                        return true;

                    case VarEnum.VT_I2:
                        return true;

                    case VarEnum.VT_UI2:
                        return true;

                    case VarEnum.VT_I4:
                        return true;

                    case VarEnum.VT_UI4:
                        return true;

                    case VarEnum.VT_I8:
                        return true;

                    case VarEnum.VT_UI8:
                        return true;

                    case VarEnum.VT_R4:
                        return true;

                    case VarEnum.VT_R8:
                        return true;

                    case VarEnum.VT_FILETIME:
                        return true;

                    case VarEnum.VT_BOOL:
                        return true;

                    case VarEnum.VT_CLSID:
                        return true;

                    case VarEnum.VT_LPSTR:
                        return true;

                    case VarEnum.VT_LPWSTR:
                        return true;

                    case VarEnum.VT_BLOB:
                        return true;
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
            VarEnum vt = (VarEnum)varType;

            if ((vt & VarEnum.VT_VECTOR) != 0)
            {
                switch (vt & (~VarEnum.VT_VECTOR))
                {
                    case VarEnum.VT_EMPTY:
                        return null;

                    case VarEnum.VT_I1:
                        {
                            sbyte[] array = new sbyte[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                                array[i] = (sbyte)Marshal.ReadByte(ca.pElems, i);
                            return array;
                        }

                    case VarEnum.VT_UI1:
                        {
                            byte[] array = new byte[ca.cElems];
                            Marshal.Copy(ca.pElems, array, 0, (int)ca.cElems);
                            return array;
                        }

                    case VarEnum.VT_I2:
                        {
                            short[] array = new short[ca.cElems];
                            Marshal.Copy(ca.pElems, array, 0, (int)ca.cElems);
                            return array;
                        }

                    case VarEnum.VT_UI2:
                        {
                            ushort[] array = new ushort[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                                array[i] = (ushort)Marshal.ReadInt16(ca.pElems, i * sizeof(ushort));
                            return array;
                        }

                    case VarEnum.VT_I4:
                        {
                            int[] array = new int[ca.cElems];
                            Marshal.Copy(ca.pElems, array, 0, (int)ca.cElems);
                            return array;
                        }

                    case VarEnum.VT_UI4:
                        {
                            uint[] array = new uint[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                                array[i] = (uint)Marshal.ReadInt32(ca.pElems, i * sizeof(uint));
                            return array;
                        }

                    case VarEnum.VT_I8:
                        {
                            Int64[] array = new Int64[ca.cElems];
                            Marshal.Copy(ca.pElems, array, 0, (int)ca.cElems);
                            return array;
                        }

                    case VarEnum.VT_UI8:
                        {
                            UInt64[] array = new UInt64[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                                array[i] = (UInt64)Marshal.ReadInt64(ca.pElems, i * sizeof(UInt64));
                            return array;
                        }

                    case VarEnum.VT_R4:
                        {
                            float[] array = new float[ca.cElems];
                            Marshal.Copy(ca.pElems, array, 0, (int)ca.cElems);
                            return array;
                        }

                    case VarEnum.VT_R8:
                        {
                            double[] array = new double[ca.cElems];
                            Marshal.Copy(ca.pElems, array, 0, (int)ca.cElems);
                            return array;
                        }

                    case VarEnum.VT_BOOL:
                        {
                            bool[] array = new bool[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                                array[i] = (bool)(Marshal.ReadInt16(ca.pElems, i * sizeof(ushort)) != 0);
                            return array;
                        }

                    case VarEnum.VT_CLSID:
                        {
                            Guid[] array = new Guid[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                            {
                                byte[] guid = new byte[16];
                                Marshal.Copy(ca.pElems, guid, i * 16, 16);
                                array[i] = new Guid(guid);
                            }
                            return array;
                        }

                    case VarEnum.VT_LPSTR:
                        {
                            String[] array = new String[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                            {
                                IntPtr ptr = Marshal.ReadIntPtr(ca.pElems, i * IntPtr.Size);
                                array[i] = Marshal.PtrToStringAnsi(ptr);
                            }
                            return array;
                        }

                    case VarEnum.VT_LPWSTR:
                        {
                            String[] array = new String[ca.cElems];
                            for (int i = 0; i < ca.cElems; i++)
                            {
                                IntPtr ptr = Marshal.ReadIntPtr(ca.pElems, i * IntPtr.Size);
                                array[i] = Marshal.PtrToStringUni(ptr);
                            }
                            return array;
                        }

                    case VarEnum.VT_UNKNOWN:
                    default:
                        break;
                }
            }
            else
            {
                switch (vt)
                {
                    case VarEnum.VT_EMPTY:
                        return null;

                    case VarEnum.VT_I1:
                        return cVal;

                    case VarEnum.VT_UI1:
                        return bVal;

                    case VarEnum.VT_I2:
                        return iVal;

                    case VarEnum.VT_UI2:
                        return uiVal;

                    case VarEnum.VT_I4:
                        return intVal;

                    case VarEnum.VT_UI4:
                        return uintVal;

                    case VarEnum.VT_I8:
                        return lVal;

                    case VarEnum.VT_UI8:
                        return ulVal;

                    case VarEnum.VT_R4:
                        return fltVal;

                    case VarEnum.VT_R8:
                        return dblVal;

                    case VarEnum.VT_FILETIME:
                        return filetime;

                    case VarEnum.VT_BOOL:
                        return (bool)(boolVal != 0);

                    case VarEnum.VT_CLSID:
                        byte[] guid = new byte[16];
                        Marshal.Copy(pclsidVal, guid, 0, 16);
                        return new Guid(guid);

                    case VarEnum.VT_LPSTR:
                        return Marshal.PtrToStringAnsi(pszVal);

                    case VarEnum.VT_LPWSTR:
                        return Marshal.PtrToStringUni(pwszVal);

                    case VarEnum.VT_BLOB:
                        {
                            byte[] blob = new byte[ca.cElems];
                            Marshal.Copy(ca.pElems, blob, 0, (int)ca.cElems);
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
                varType = (ushort)VarEnum.VT_EMPTY;
            }
            else if (value is Array)
            {
                Type type = value.GetType();

                if (type == typeof(sbyte[]))
                {
                    InitVector(value as Array, typeof(sbyte), VarEnum.VT_I1);
                }
                else if (type == typeof(byte[]))
                {
                    InitVector(value as Array, typeof(byte), VarEnum.VT_UI1);
                }
                else if (value is char[])
                {
                    varType = (ushort)VarEnum.VT_LPSTR;
                    pszVal = Marshal.StringToCoTaskMemAnsi(new String(value as char[]));
                }
                else if (value is char[][])
                {
                    char[][] charArray = value as char[][];

                    String[] strArray = new String[charArray.GetLength(0)];

                    for (int i = 0; i < charArray.Length; i++)
                    {
                        strArray[i] = new String(charArray[i] as char[]);
                    }

                    Init(strArray, true);
                }
                else if (type == typeof(short[]))
                {
                    InitVector(value as Array, typeof(short), VarEnum.VT_I2);
                }
                else if (type == typeof(ushort[]))
                {
                    InitVector(value as Array, typeof(ushort), VarEnum.VT_UI2);
                }
                else if (type == typeof(int[]))
                {
                    InitVector(value as Array, typeof(int), VarEnum.VT_I4);
                }
                else if (type == typeof(uint[]))
                {
                    InitVector(value as Array, typeof(uint), VarEnum.VT_UI4);
                }
                else if (type == typeof(Int64[]))
                {
                    InitVector(value as Array, typeof(Int64), VarEnum.VT_I8);
                }
                else if (type == typeof(UInt64[]))
                {
                    InitVector(value as Array, typeof(UInt64), VarEnum.VT_UI8);
                }
                else if (value is float[])
                {
                    InitVector(value as Array, typeof(float), VarEnum.VT_R4);
                }
                else if (value is double[])
                {
                    InitVector(value as Array, typeof(double), VarEnum.VT_R8);
                }
                else if (value is Guid[])
                {
                    InitVector(value as Array, typeof(Guid), VarEnum.VT_CLSID);
                }
                else if (value is String[])
                {
                    Init(value as String[], false);
                }
                else if (value is bool[])
                {
                    bool[] boolArray = value as bool[];
                    short[] array = new short[boolArray.Length];

                    for (int i = 0; i < boolArray.Length; i++)
                    {
                        array[i] = (short)(boolArray[i] ? -1 : 0);
                    }

                    InitVector(array, typeof(short), VarEnum.VT_BOOL);
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            else
            {
                Type type = value.GetType();

                if (value is String)
                {
                    varType = (ushort)VarEnum.VT_LPWSTR;
                    pwszVal = Marshal.StringToCoTaskMemUni(value as String);
                }
                else if (type == typeof(sbyte))
                {
                    varType = (ushort)VarEnum.VT_I1;
                    cVal = (sbyte)value;
                }
                else if (type == typeof(byte))
                {
                    varType = (ushort)VarEnum.VT_UI1;
                    bVal = (byte)value;
                }
                else if (type == typeof(System.Runtime.InteropServices.ComTypes.FILETIME))
                {
                    varType = (ushort)VarEnum.VT_FILETIME;
                    filetime = (System.Runtime.InteropServices.ComTypes.FILETIME)value;
                }
                else if (value is char)
                {
                    varType = (ushort)VarEnum.VT_LPSTR;
                    pszVal = Marshal.StringToCoTaskMemAnsi(new String(new char[] { (char)value }));
                }
                else if (type == typeof(short))
                {
                    varType = (ushort)VarEnum.VT_I2;
                    iVal = (short)value;
                }
                else if (type == typeof(ushort))
                {
                    varType = (ushort)VarEnum.VT_UI2;
                    uiVal = (ushort)value;
                }
                else if (type == typeof(int))
                {
                    varType = (ushort)VarEnum.VT_I4;
                    intVal = (int)value;
                }
                else if (type == typeof(uint))
                {
                    varType = (ushort)VarEnum.VT_UI4;
                    uintVal = (uint)value;
                }
                else if (type == typeof(Int64))
                {
                    varType = (ushort)VarEnum.VT_I8;
                    lVal = (Int64)value;
                }
                else if (type == typeof(UInt64))
                {
                    varType = (ushort)VarEnum.VT_UI8;
                    ulVal = (UInt64)value;
                }
                else if (value is float)
                {
                    varType = (ushort)VarEnum.VT_R4;
                    fltVal = (float)value;
                }
                else if (value is double)
                {
                    varType = (ushort)VarEnum.VT_R8;
                    dblVal = (double)value;
                }
                else if (value is Guid)
                {
                    byte[] guid = ((Guid)value).ToByteArray();
                    varType = (ushort)VarEnum.VT_CLSID;
                    pclsidVal = Marshal.AllocCoTaskMem(guid.Length);
                    Marshal.Copy(guid, 0, pclsidVal, guid.Length);
                }
                else if (value is bool)
                {
                    varType = (ushort)VarEnum.VT_BOOL;
                    boolVal = (short)(((bool)value) ? -1 : 0);
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
        }

        internal void InitVector(Array array, Type type, VarEnum varEnum)
        {
            Init(array, type, varEnum | VarEnum.VT_VECTOR);
        }

        /// <SecurityNote>
        /// Critical -Accesses unmanaged code and structure is overlapping in memory
        /// TreatAsSafe - inputs are verified or safe
        /// </SecurityNote>
        internal void Init(Array array, Type type, VarEnum vt)
        {
            varType = (ushort)vt;
            ca.cElems = 0;
            ca.pElems = IntPtr.Zero;

            int length = array.Length;

            if (length > 0)
            {
                long size = Marshal.SizeOf(type) * length;

                IntPtr destPtr = IntPtr.Zero;
                GCHandle handle = new GCHandle();

                try
                {
                    destPtr = Marshal.AllocCoTaskMem((int)size);
                    handle = GCHandle.Alloc(array, GCHandleType.Pinned);

                    // TODO: Fix copy to not use unsafe
                    throw new NotImplementedException();
                    //Marshal.Copy();
                    //unsafe
                    //{
                    //    CopyBytes((byte*)destPtr, (int)size, (byte*)handle.AddrOfPinnedObject(), (int)size);
                    //}

                    ca.cElems = (uint)length;
                    ca.pElems = destPtr;

                    destPtr = IntPtr.Zero;
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }

                    if (destPtr != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(destPtr);
                    }
                }
            }
        }

        /// <SecurityNote>
        /// Critical -Accesses unmanaged code and structure is overlapping in memory
        /// TreatAsSafe - inputs are verified or safe
        /// </SecurityNote>
        private void Init(string[] value, bool fAscii)
        {
            varType = (ushort)(fAscii ? VarEnum.VT_LPSTR : VarEnum.VT_LPWSTR);
            varType |= (ushort)VarEnum.VT_VECTOR;
            ca.cElems = 0;
            ca.pElems = IntPtr.Zero;

            int length = value.Length;

            if (length > 0)
            {
                IntPtr destPtr = IntPtr.Zero;
                int sizeIntPtr = IntPtr.Size;
                long size = sizeIntPtr * length;
                int index = 0;

                try
                {
                    IntPtr pString = IntPtr.Zero;

                    destPtr = Marshal.AllocCoTaskMem((int)size);

                    for (index = 0; index < length; index++)
                    {
                        if (fAscii)
                        {
                            pString = Marshal.StringToCoTaskMemAnsi(value[index]);
                        }
                        else
                        {
                            pString = Marshal.StringToCoTaskMemUni(value[index]);
                        }
                        Marshal.WriteIntPtr(destPtr, (int)index * sizeIntPtr, pString);
                    }

                    ca.cElems = (uint)length;
                    ca.pElems = destPtr;
                    destPtr = IntPtr.Zero;
                }
                finally
                {
                    if (destPtr != IntPtr.Zero)
                    {
                        for (int i = 0; i < index; i++)
                        {
                            IntPtr pString = Marshal.ReadIntPtr(destPtr, i * sizeIntPtr);
                            Marshal.FreeCoTaskMem(pString);
                        }

                        Marshal.FreeCoTaskMem(destPtr);
                    }
                }
            }
        }
    }
}
