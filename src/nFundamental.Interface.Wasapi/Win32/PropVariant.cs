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
    }
}
