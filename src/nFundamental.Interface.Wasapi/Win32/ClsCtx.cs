using System;

namespace Fundamental.Interface.Wasapi.Win32
{


    [Flags]
    public enum ClsCtx : uint
    {
        InprocServer = 0x1,
        InprocHandler = 0x2,
        LocalServer = 0x4,
        InprocServer16 = 0x8,
        RemoteServer = 0x10,
        InprocHandler16 = 0x20,
        //Reserved1 = 0x40,
        //Reserved2 = 0x80,
        //Reserved3 = 0x100,
        //Reserved4 = 0x200,
        NoCodeDownload = 0x400,
        //Reserved5 = 0x800,
        NoCustomMarshal = 0x1000,
        EnableCodeDownload = 0x2000,
        NoFailureLog = 0x4000,
        DisableAaa = 0x8000,
        EnableAaa = 0x10000,
        FromDefaultContext = 0x20000,
        Activate32BitServer = 0x40000,
        Activate64BitServer = 0x80000,
        EnableCloaking = 0x100000,
        AppContainer = 0x400000,
        ActivateAaaAsIu = 0x800000,
        PsDll = 0x80000000
    }
}
