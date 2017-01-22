using System;
using System.Collections;
using System.Collections.Generic;
using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDevicePropertyBag : IDevicePropertyBag
    {
        /// <summary>
        /// The underlying property store
        /// </summary>
        public IPropertyStore PropertyStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDevicePropertyBag" /> class.
        /// </summary>
        /// <param name="propertyStore">The property store.</param>
        public WasapiDevicePropertyBag(IPropertyStore propertyStore)
        {
            PropertyStore = propertyStore;
        }


        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Keys { get; }
        public IEnumerable<object> Values { get; }
        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            throw new NotImplementedException();
        }
    }
}
