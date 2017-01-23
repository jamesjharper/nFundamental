using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Internal
{
    public interface IWasapiPropertyNameTranslator
    {
        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string ResolvePropertyName(PropertyKey key);

        /// <summary>
        /// Resolves the property key.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        PropertyKey ResolvePropertyKey(string propertyName);
    }
}
