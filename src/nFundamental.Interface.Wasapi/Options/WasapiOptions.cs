namespace Fundamental.Interface.Wasapi.Options
{
    public class WasapiOptions
    {
        /// <summary>
        /// Gets or sets the device information.
        /// </summary>
        /// <value>
        /// The device information.
        /// </value>
        public WasapiDeviceInfoOptions DeviceInfo { get; set; } = new WasapiDeviceInfoOptions();

        /// <summary>
        /// Gets or sets the audio source settings.
        /// </summary>
        /// <value>
        /// The audio source settings.
        /// </value>
        public WasapiAudioSourceSettings AudioSource { get; set; } = new WasapiAudioSourceSettings();
    }
}
