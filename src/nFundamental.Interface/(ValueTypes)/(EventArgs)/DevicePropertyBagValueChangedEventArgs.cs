using System;

namespace Fundamental.Interface
{
    public class DevicePropertyChangedEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the property key.
        /// </summary>
        /// <value>
        /// The property key.
        /// </value>
        public string  PropertyKey { get; }

        /// <summary>
        /// Gets the device information.
        /// </summary>
        /// <value>
        /// The device information.
        /// </value>
        public IDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevicePropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="propertyKey">The property key.</param>
        public DevicePropertyChangedEventArgs(IDeviceInfo deviceInfo, string propertyKey)
        {
            PropertyKey = propertyKey;
            DeviceInfo = deviceInfo;
        }
    }
}
