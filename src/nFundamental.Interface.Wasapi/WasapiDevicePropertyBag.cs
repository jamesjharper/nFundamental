using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDevicePropertyBag : IDevicePropertyBag
    {
        /// <summary>
        /// The WASAPI property name translator
        /// </summary>
        private readonly IWasapiPropertyNameTranslator _wasapiPropertyNameTranslator;

        /// <summary>
        /// The underlying property store
        /// </summary>
        public IPropertyStore PropertyStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDevicePropertyBag" /> class.
        /// </summary>
        /// <param name="propertyStore">The property store.</param>
        /// <param name="wasapiPropertyNameTranslator">The WASAPI property name translator.</param>
        public WasapiDevicePropertyBag(IPropertyStore propertyStore,
                                       IWasapiPropertyNameTranslator wasapiPropertyNameTranslator)
        {
            _wasapiPropertyNameTranslator = wasapiPropertyNameTranslator;
            PropertyStore = propertyStore;
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                object result;
                return TryGetValue(key, out result) ? result : null;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(string key)
        {
            object dummy;
            return TryGetValue(key, out dummy);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(string key, out object value)
        {
            value = null;
           
            // If the key was not recognized, an empty GUID should be returned.
            var propertyKey = _wasapiPropertyNameTranslator.ResolvePropertyKey(key);
            if (Equals(propertyKey.FormatId, Guid.Empty))
                return false;

            // If the key isn't found in the property bag then EMPTY is returned
            PropVariant variant;
            PropertyStore.GetValue(propertyKey, out variant).ThrowIfFailed();
            if (variant.ValueType == VariantType.VT_EMPTY)
                return false;

            // If we don't know how to read the returned 
            // variant type, we ignore it
            if (!variant.IsVariantTypeSupported())
                return false;
            
            value = variant.ToObject();
            return true;
        }


        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get { return GetPropertyKeyEnumerable().Select(x => _wasapiPropertyNameTranslator.ResolvePropertyName(x)); }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public IEnumerable<object> Values
        {
            get { return GetPropertyKeyValueEnumerable().Select(x => x.Value); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var keyValuePair in GetPropertyKeyValueEnumerable())
            {
                var propName = _wasapiPropertyNameTranslator.ResolvePropertyName(keyValuePair.Key);

                // Filter out properties who's names we couldn't resolve
                if(Equals(propName, string.Empty))
                    continue;
                yield return new KeyValuePair<string, object>(propName, keyValuePair.Value);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Private Methods

        private IEnumerable<PropertyKey> GetPropertyKeyEnumerable()
        {
            int count;
            PropertyStore.GetCount(out count);

            for (var i = 0; i < count; i++)
            {
                PropertyKey propertyKey;
                PropertyStore.GetAt(i, out propertyKey).ThrowIfFailed();

                yield return propertyKey;
            }
        }

        private IEnumerable<KeyValuePair<PropertyKey, object>> GetPropertyKeyValueEnumerable()
        {
            foreach (var propertyKey in GetPropertyKeyEnumerable())
            {
                PropVariant variant;
                PropertyStore.GetValue(propertyKey, out variant).ThrowIfFailed();

                if(!variant.IsVariantTypeSupported())
                    continue;
                
                var value = variant.ToObject();
                yield return new KeyValuePair<PropertyKey, object>(propertyKey, value);
            }
        }
    }
}
