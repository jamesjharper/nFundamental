using System;

namespace Fundamental.Interface
{
    public class DeviceAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the device identifier token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>
        public IDeviceToken DeviceToken { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceAddedEventArgs"/> class.
        /// </summary>
        /// <param name="deviceToken">The device identifier token.</param>
        public DeviceAddedEventArgs(IDeviceToken deviceToken)
        {
            DeviceToken = deviceToken;
        }
    }
}
