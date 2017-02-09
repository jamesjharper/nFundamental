using System;

namespace Fundamental.Interface.Wasapi.Options
{
    public class WasapiAudioSourceSettings
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
        public TimeSpan BufferLength { get; set; } = TimeSpan.FromMilliseconds(100);
    }
}
