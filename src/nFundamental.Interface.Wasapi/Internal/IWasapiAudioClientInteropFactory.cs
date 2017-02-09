using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Internal
{
    public interface IWasapiAudioClientInteropFactory
    {

        /// <summary>
        /// Create a new audio client COM instance.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        IWasapiAudioClientInterop FactoryAudioClient(IDeviceToken token);

    }
}
