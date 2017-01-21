using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi
{
    public interface IWasapiDeviceTokenFactory
    {
        /// <summary>
        /// Gets the token from the give IMMDevice.
        /// </summary>
        /// <param name="immDevice">The Multimedia device com object.</param>
        /// <returns></returns>
        WasapiDeviceToken GetToken(IMMDevice immDevice);

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="id">A device identifier.</param>
        /// <returns></returns>
        WasapiDeviceToken GetToken(string id);
    }
}
