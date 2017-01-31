using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Internal
{
    public interface IWasapiAudioClientFactory
    {

        /// <summary>
        /// Create a new audio client COM instance.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        IAudioClient FactoryAudioClient(WasapiDeviceToken token);

    }
}
