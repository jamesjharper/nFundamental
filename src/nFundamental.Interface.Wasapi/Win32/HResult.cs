namespace Fundamental.Interface.Wasapi.Win32
{
    public enum HResult
    {
        S_OK = 0x0,
        S_FALSE = 0x1,
        E_ABORT = unchecked((int)0x80004004),
        E_ACCESSDENIED = unchecked((int)0x80070005),
        E_NOINTERFACE = unchecked((int)0x80004002),
        E_FAIL = unchecked((int)0x80004005),
        E_INVALIDARG = unchecked((int)0x80070057),
        E_POINTER = unchecked((int)0x80004003),
        E_NOTIMPL = unchecked((int)0x80004001),
        E_NOTFOUND = unchecked((int)0x80070490),
        MF_E_ATTRIBUTENOTFOUND = unchecked((int)0xC00D36E6),
        MF_E_SHUTDOWN = unchecked((int)0xc00d3e85),
        AUDCLNT_E_UNSUPPORTED_FORMAT = unchecked((int)0x88890008),
        AUDCLNT_E_DEVICE_INVALIDATED = unchecked((int)0x88890004),
        AUDCLNT_S_BUFFER_EMPTY = 0x08890001
    }


}
