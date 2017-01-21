using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundamental.Interface
{
    public interface IDeviceAvailabilityNotifier
    {
        /// <summary>
        /// Occurs when a device added is to the system.
        /// </summary>
        event EventHandler<DeviceAddedEventArgs> DeviceAdded;

        /// <summary>
        /// Occurs when a device removed is to the system.
        /// </summary>
        event EventHandler<DeviceRemovedEventArgs> DeviceRemoved;
    }
}
