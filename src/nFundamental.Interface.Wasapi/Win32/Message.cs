using System;
using System.Runtime.InteropServices;

namespace Fundamental.Interface.Wasapi.Win32
{
    public struct Message
    {
        public IntPtr HWnd { get; set; }

        public IntPtr LParam { get; set; }

        public int Msg { get; set; }

        public IntPtr Result { get; set; }

        public IntPtr WParam { get; set; }


        public static Message Create(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            return new Message
            {
                Msg = msg,
                HWnd = hWnd,
                WParam = wparam,
                LParam = lparam
            };
        }

        public static bool operator ==(Message a, Message b)
        {
            return (a.HWnd == b.HWnd) && (a.LParam == b.LParam) && (a.Msg == b.Msg) && (a.Result == b.Result) && (a.WParam == b.WParam);
        }

        public static bool operator !=(Message a, Message b)
        {
            return !(a == b);
        }


        public override bool Equals(object o)
        {
            if (!(o is Message))
            {
                return false;
            }

            var other = (Message) o;

            return ((Msg == other.Msg) &&
                    (HWnd == other.HWnd) &&
                    (LParam == other.LParam) &&
                    (WParam == other.WParam) &&
                    (Result == other.Result));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public T GetLParam<T>()
        {
            return Marshal.PtrToStructure<T>(LParam);

        }

        public override string ToString()
        {
            return $"msg=0x{Msg:x} ({Msg}) hwnd=0x{HWnd.ToInt32():x} wparam=0x{WParam.ToInt32():x} lparam=0x{LParam.ToInt32():x} result=0x{Result.ToInt32():x}";
        }

    }
}
