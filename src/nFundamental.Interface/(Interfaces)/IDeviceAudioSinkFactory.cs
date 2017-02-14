namespace Fundamental.Interface
{
    public interface IDeviceAudioSinkFactory
    {
        /// <summary>
        /// Gets a sink client for the given device token instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        IHardwareAudioSink GetAudioSink(IDeviceToken deviceToken);
    }
}
