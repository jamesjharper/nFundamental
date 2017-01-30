using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiDevicePropertyChangedEventArgs
    {
        /// <summary>
        /// Gets the device identifier token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>
        public IDeviceToken DeviceToken { get; }

        /// <summary>
        /// Gets the property key.
        /// </summary>
        /// <value>
        /// The property key.
        /// </value>
        public PropertyKey PropertyKey { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDevicePropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <param name="key">The key.</param>
        public WasapiDevicePropertyChangedEventArgs(IDeviceToken deviceToken, PropertyKey key)
        {
            DeviceToken = deviceToken;
            PropertyKey = key;
        }
    }
}
