
namespace Fundamental.Interface
{
    public interface IDeviceAudioSourceFactory
    {
        /// <summary>
        /// Gets a source client for the given device token instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        IHardwareAudioSource GetAudioSource(IDeviceToken deviceToken);
    }
}
