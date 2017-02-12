namespace Fundamental.Interface
{
    public interface IDefaultDeviceProvider
    {

        /// <summary>
        /// Gets the default device for the operating system
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IDeviceToken GetDefaultDevice(DeviceType type);

        /// <summary>
        /// Gets the default device for the operating system
        /// </summary>
        /// <param name="type">The type of device.</param>
        /// <param name="deviceRole">The device role.</param>
        /// <returns>
        /// A Token which can be used for accessing the device
        /// </returns>
        IDeviceToken GetDefaultDevice(DeviceType type, DeviceRole deviceRole);
    }
}
