using System;

namespace Fundamental.Interface
{
    public interface IDeviceInfo
    {
        /// <summary>
        /// Occurs when a property value in the bag changes.
        /// </summary>
        event EventHandler<DevicePropertyChangedEventArgs> PropertyValueChangedEvent;

        /// <summary>
        /// Gets the device handle.
        /// </summary>
        /// <value>
        /// The device handle.
        /// </value>
        IDeviceToken DeviceToken { get; }

        /// <summary>
        /// Gets the Device properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        IDevicePropertyBag Properties { get; }
    }
}
