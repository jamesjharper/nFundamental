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
        public WasapiAudioClientSettings AudioCapture { get; set; } = new WasapiAudioClientSettings();


        /// <summary>
        /// Gets or sets the audio sink settings.
        /// </summary>
        /// <value>
        /// The audio render.
        /// </value>
        public WasapiAudioClientSettings AudioRender { get; set; } = new WasapiAudioClientSettings();
    }
}
