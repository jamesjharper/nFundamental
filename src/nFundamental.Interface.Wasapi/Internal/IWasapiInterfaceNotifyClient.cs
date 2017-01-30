using System;

namespace Fundamental.Interface.Wasapi.Internal
{
    public interface IWasapiInterfaceNotifyClient :
       IDeviceStatusNotifier,
       IDefaultDeviceStatusNotifier,
       IDeviceAvailabilityNotifier
    {

        /// <summary>
        /// Occurs when [device property changed].
        /// </summary>
        event EventHandler<WasapiDevicePropertyChangedEventArgs> DevicePropertyChanged;
    }
}
