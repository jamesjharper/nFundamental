namespace Fundamental.Interface
{
    public interface IDeviceInfoRepository
    {

        /// <summary>
        /// Gets the information device.
        /// </summary>
        /// <param name="deviceToken">The device handle.</param>
        /// <returns></returns>
        IDeviceInfo GetInfoDevice(IDeviceToken deviceToken);


        ///// <summary>
        ///// Gets the devices.
        ///// </summary>
        ///// <param name="query">The query.</param>
        ///// <returns></returns>
        //IEnumerable<IDeviceInfo> Queary(IDeviceQueary query);

    }
}
