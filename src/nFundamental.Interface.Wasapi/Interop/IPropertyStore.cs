using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Interop
{
    /// <summary>
    /// Remarks:
    /// These methods can be called at any time after initialization of the property handler but before property changes are written to the file through 
    /// IPropertyStore::Commit.At any other time, these methods return E_FAIL.
    /// 
    /// When to Implement:
    /// An implementation of this interface is provided by CLSID_InMemoryPropertyStore, as IPropertyStoreCache.Users should never need to implement it themselves.
    /// CLSID_InMemoryPropertyStore implements IPropertyStoreCache instead of IPropertyStore so that it can store additional state information (PSC_STATE) about 
    /// each of the properties in the cache.This information can be useful for property handler implementers.It can also be useful in other 
    /// scenarios where a cache of property values is needed.
    /// 
    /// Requirements:
    ///  Minimum supported client
    ///  - Windows Vista [desktop apps | Windows Store apps]
    /// 
    ///  Minimum supported server
    ///  - Windows Server 2008 [desktop apps | Windows Store apps]
    /// 
    /// Header: Propsys.h
    /// IDL: Propsys.idl
    /// </summary>
    [ComImport]
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStore
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="propCount">
        /// Return value.
        /// </param>
        /// <returns>
        /// returns S_OK on success, even if the file has no properties.
        /// </returns>
        HResult GetCount([Out] out int propCount);

        /// <summary>
        /// Gets a property key from an item's array of properties.
        /// </summary>
        /// <param name="index">
        /// The index of the property key in the array of PROPERTYKEY structures.This is a zero-based index.
        /// </param>
        /// <param name="key">
        /// When this method returns, contains a PROPERTYKEY structure that receives the unique identifier for a property.
        /// </param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        HResult GetAt([In] int index, [Out] out PropertyKey key);

        /// <summary>
        /// Gets data for a specific property.
        /// </summary>
        /// <param name="key">
        /// A reference to the PROPERTYKEY structure retrieved through IPropertyStore::GetAt. This structure contains a unique identifier for the property in question.
        /// </param>
        /// <param name="value">
        /// When this method returns, contains a PROPVARIANT structure that contains the property data.
        /// </param>
        /// <returns>
        /// Returns S_OK or INPLACE_S_TRUNCATED if successful, or an error value otherwise.
        /// INPLACE_S_TRUNCATED is returned to indicate that the returned PROPVARIANT was coerced to a more canonical form, for instance to trim 
        /// leading or trailing spaces from a string value.Most code should use the SUCCEEDED macro to check the return value, which treats INPLACE_S_TRUNCATED as a success code.
        /// 
        /// Remarks:
        /// If the PROPERTYKEY referenced in key is not present in the property store, this method returns S_OK and the vt member of the structure 
        /// pointed to by pv is set to VT_EMPTY.
        /// </returns>
        HResult GetValue([In] ref PropertyKey key, [Out] out PropVariant value);

        /// <summary>
        /// Sets a new property value, or replaces or removes an existing value.
        /// </summary>
        /// <param name="key">
        /// A reference to the PROPERTYKEY structure retrieved through IPropertyStore::GetAt. This structure contains a unique identifier for the property in question.
        /// </param>
        /// <param name="value">
        /// A reference to a PROPVARIANT structure that contains the new property data.
        /// </param>
        /// <returns></returns>
        HResult SetValue([In] ref PropertyKey key, [In] ref PropVariant value);

        /// <summary>
        /// Saves a property change.
        /// </summary>
        /// <returns>
        /// Returns S_OK or INPLACE_S_TRUNCATED if successful, or an error value otherwise, including the following:
        /// Return code Description:
        /// 
        /// S_OK:
        /// All of the property changes were successfully written to the stream or path.This includes the case where no changes were pending and nothing was written.
        /// 
        /// INPLACE_S_TRUNCATED:
        ///The value was set but truncated in a string value or rounded if a numeric value.
        /// 
        ///STG_E_ACCESSDENIED:
        /// The stream or file is read-only so the method was not able to set the value.
        /// 
        /// STG_E_INVALIDARG:
        ///The property handler could not store any value for the PROPERTYKEY specified.
        /// 
        /// E_FAIL:
        /// Some or all of the changes could not be written to the file.Another appropriate error code can be used in place of E_FAIL.
        /// </returns>
        HResult Commit();
    }
}
