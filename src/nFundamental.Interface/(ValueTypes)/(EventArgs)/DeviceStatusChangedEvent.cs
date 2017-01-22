
namespace Fundamental.Interface
{
    public class DeviceStatusChangedEvent
    {

        /// <summary>
        /// Gets the device token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>
        public IDeviceToken DeviceToken { get; }

        /// <summary>
        /// Gets the state of the device.
        /// </summary>
        /// <value>
        /// The state of the device.
        /// </value>
        public DeviceState DeviceState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceStatusChangedEvent"/> class.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <param name="deviceState">State of the device.</param>
        public DeviceStatusChangedEvent(IDeviceToken deviceToken, DeviceState deviceState)
        {
            DeviceToken = deviceToken;
            DeviceState = deviceState; 
        }
    }
}
