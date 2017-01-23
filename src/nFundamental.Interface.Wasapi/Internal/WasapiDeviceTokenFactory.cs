using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiDeviceTokenFactory : IWasapiDeviceTokenFactory
    {
        /// <summary>
        /// The device enumerator which is used for lazy loading the 
        /// IMMDevice instance
        /// </summary>
        private readonly IMMDeviceEnumerator _deviceEnumerator;


        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceTokenFactory"/> class.
        /// </summary>
        /// <param name="deviceEnumerator">The device enumerator.</param>
        public WasapiDeviceTokenFactory(IMMDeviceEnumerator deviceEnumerator)
        {
            _deviceEnumerator = deviceEnumerator;
        }

        /// <summary>
        /// Gets the token from the give IMMDevice.
        /// </summary>
        /// <param name="immDevice">The Multimedia device com object.</param>
        /// <returns></returns>
        public WasapiDeviceToken GetToken(IMMDevice immDevice)
        {
            return new WasapiDeviceToken(immDevice, _deviceEnumerator);
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="id">A device identifier.</param>
        /// <returns></returns>
        public WasapiDeviceToken GetToken(string id)
        {
            return new WasapiDeviceToken(id, _deviceEnumerator);
        }
    }
}
