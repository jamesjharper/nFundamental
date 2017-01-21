using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceTokenFactory : IWasapiDeviceTokenFactory
    {
        /// <summary>
        /// Gets the token from the give IMMDevice.
        /// </summary>
        /// <param name="immDevice">The Multimedia device com object.</param>
        /// <returns></returns>
        public WasapiDeviceToken GetToken(IMMDevice immDevice)
        {
            string deviceId;
            immDevice.GetId(out deviceId);
            return new WasapiDeviceToken(deviceId);
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="id">A device identifier.</param>
        /// <returns></returns>
        public WasapiDeviceToken GetToken(string id)
        {
            return new WasapiDeviceToken(id);
        }
    }
}
