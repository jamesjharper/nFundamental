using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWasapiPropertyNameTranslator
    {
        /// <summary>
        /// Resolves the property key from the WASAPI Property key object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        WasapiPropertyBagKey ResolvePropertyKey(PropertyKey key);

        /// <summary>
        /// Resolves the property key.
        /// </summary>
        /// <param name="keyId"> The Property key Id.</param>
        /// <returns></returns>
        PropertyKey ResolvePropertyKey(string keyId);
    }
}
