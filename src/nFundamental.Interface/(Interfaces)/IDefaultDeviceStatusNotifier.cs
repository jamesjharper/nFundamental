using System;

namespace Fundamental.Interface
{
    public interface IDefaultDeviceStatusNotifier
    {
        /// <summary>
        /// Occurs when default device changed.
        /// </summary>
        event EventHandler<DefaultDeviceChangedEventArgs> DefaultDeviceChanged;
    }
}
