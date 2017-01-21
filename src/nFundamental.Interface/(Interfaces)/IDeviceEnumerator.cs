using System.Collections.Generic;

namespace Fundamental.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDeviceEnumerator
    {
        /// <summary>
        /// Gets the all known system devices.
        /// </summary>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        IEnumerable<IDeviceToken> GetDevices();

        /// <summary>
        /// Gets the all known system devices by the device type.
        /// </summary>
        /// <param name="type">The device type.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        IEnumerable<IDeviceToken> GetDevices(DeviceType type);

        /// <summary>
        /// Gets the all known system devices by the device type and its current state.
        /// </summary>
        /// <param name="type">The device type.</param>
        /// <param name="stateMask">State mask.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        IEnumerable<IDeviceToken> GetDevices(DeviceType type, params DeviceState[] stateMask);

        /// <summary>
        /// Gets the all known system devices by the device type and its current state.
        /// </summary>
        /// <param name="typeMask">The type mask.</param>
        /// <param name="stateMask">The state mask.</param>
        /// <returns>
        /// IEnumerable of device tokens
        /// </returns>
        IEnumerable<IDeviceToken> GetDevices(DeviceType[] typeMask, DeviceState[] stateMask);
    }
}
