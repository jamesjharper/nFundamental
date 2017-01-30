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
    }
}
