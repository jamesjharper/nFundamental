using System;

namespace Fundamental.Interface
{
    public interface IDeviceStatusNotifier
    {
        /// <summary>
        /// Occurs when a device status changes
        /// </summary>
        event EventHandler<DeviceStatusChangedEvent> DeviceStatusChanged;
    }
}
