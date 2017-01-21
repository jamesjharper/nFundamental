using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    #region Extract from mmdeviceapi.h

    //  typedef /* [public][public][public][public][public] */
    //  enum __MIDL___MIDL_itf_mmdeviceapi_0000_0000_0001
    //  {
    //      eRender = 0,
    //      eCapture = (eRender + 1),
    //      eAll = (eCapture + 1),
    //      EDataFlow_enum_count = (eAll + 1)
    //  }
    //  EDataFlow;

    #endregion

    [Flags]
    public  enum DataFlow : uint
    {
        Render = 0,
        Capture,
        All
    };
}
