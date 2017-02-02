namespace Fundamental.Interface.Wasapi.Win32
{
    public enum HResult : uint
    {   
        S_OK                         = 0x0,
        S_FALSE                      = 0x1,
        E_ABORT                      = 0x80004004,
        E_NOINTERFACE                = 0x80004002,
        E_FAIL                       = 0x80004005,

        E_ACCESSDENIED               = 0x80070005,
        E_INVALIDARG                 = 0x80070057,
        E_POINTER                    = 0x80004003,
        E_NOTIMPL                    = 0x80004001,
        E_NOTFOUND                   = 0x80070490,
        MF_E_ATTRIBUTENOTFOUND       = 0xC00D36E6,
        MF_E_SHUTDOWN                = 0xc00d3e85,

        AUDCLNT_E_UNSUPPORTED_FORMAT = 0x88890008,
        AUDCLNT_E_DEVICE_INVALIDATED = 0x88890004,
        AUDCLNT_S_BUFFER_EMPTY       = 0x08890001
    }


}
