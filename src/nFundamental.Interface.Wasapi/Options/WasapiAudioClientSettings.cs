using System;

namespace Fundamental.Interface.Wasapi.Options
{
    public class WasapiAudioClientSettings
    {
        /// <summary>
        /// Gets or sets the device access.
        /// </summary>
        /// <value>
        /// The device access.
        /// </value>
        public DeviceAccess DeviceAccess { get; set; } = DeviceAccess.Shared;

        /// <summary>
        /// Gets or sets the length of the buffer.
        /// </summary>
        /// <value>
        /// The length of the buffer.
        /// </value>
        public TimeSpan ManualSyncLatency { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Gets or sets a value indicating whether [use event synchronize].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use event synchronize]; otherwise, <c>false</c>.
        /// </value>
        public bool UseHardwareSync { get; set; } = true;
    }
}
