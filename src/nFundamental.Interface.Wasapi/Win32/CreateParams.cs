using System;

namespace Fundamental.Interface.Wasapi.Win32
{
    public class CreateParams
    {

        public string ClassName { get; set; }
     
        public object Param { get; set; }

        public IntPtr Parent { get; set; }

    }
}
