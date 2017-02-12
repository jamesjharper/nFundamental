using System;

namespace Fundamental.Interface
{
    public class DefaultDeviceChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the device token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>
        public IDeviceToken DeviceToken { get; }

        /// <summary>
        /// Gets the device role.
        /// </summary>
        /// <value>
        /// The device role.
        /// </value>
        public DeviceRole DeviceRole { get; }

        /// <summary>
        /// Gets the type of the device.
        /// </summary>
        /// <value>
        /// The type of the device.
        /// </value>
        public DeviceType DeviceType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDeviceChangedEventArgs"/> class.
        /// </summary>
        /// <param name="deviceRole">The device role.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="deviceToken">The device token.</param>
        public DefaultDeviceChangedEventArgs(DeviceRole deviceRole, DeviceType deviceType, IDeviceToken deviceToken)
        {
            DeviceToken = deviceToken;
            DeviceRole = deviceRole;
            DeviceType = deviceType;
        }
    }
}
