using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{
    [ComImport]
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceCollection
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="deviceCount">
        /// Pointer to a UINT variable into which the method writes the number of devices in the device collection..</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, possible return codes include, 
        /// but are not limited to, the values shown in the following table.
        /// 
        /// Return code Description:
        /// E_POINTER = Parameter pcDevices is NULL.
        /// </returns>
        [PreserveSig]
        HResult GetCount([Out] out int deviceCount);

        /// <summary>
        /// Items the specified device index.
        /// </summary>
        /// <param name="deviceIndex">
        /// The device number. If the collection contains n devices, the devices are numbered 0 to n– 1.
        /// </param>
        /// <param name="device">
        /// Pointer to a pointer variable into which the method writes the address of the IMMDevice interface 
        /// of the specified item in the device collection. Through this method, the caller obtains a counted 
        /// reference to the interface. The caller is responsible for releasing the interface, when it is no 
        /// longer needed, by calling the interface's Release method. If the Item call fails, *ppDevice is NULL.
        /// </param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. If it fails, possible return codes include, but are not limited 
        /// to, the values shown in the following table.
        /// 
        /// Return code Description:
        /// E_POINTER    = Parameter ppDevice is NULL.
        /// E_INVALIDARG = Parameter nDevice is not a valid device number.
        ///</returns>
        [PreserveSig]
        HResult Item([In]int deviceIndex, [Out] out IMMDevice device);
    }
}
