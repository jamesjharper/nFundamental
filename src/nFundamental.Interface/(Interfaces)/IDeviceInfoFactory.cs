namespace Fundamental.Interface
{
    public interface IDeviceInfoFactory
    {

        /// <summary>
        /// Gets a information device instance.
        /// </summary>
        /// <param name="deviceToken">The device handle.</param>
        /// <returns></returns>
        IDeviceInfo GetInfoDevice(IDeviceToken deviceToken);
    }
}
