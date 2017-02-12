using System;

namespace Fundamental.Interface
{
    public class DeviceRemovedEventArgs : EventArgs
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
        public DeviceRemovedEventArgs(IDeviceToken deviceToken)
        {
            DeviceToken = deviceToken;
        }
    }
}
