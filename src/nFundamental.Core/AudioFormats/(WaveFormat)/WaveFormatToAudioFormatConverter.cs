
namespace Fundamental.Core.AudioFormats
{
    public class WaveFormatToAudioFormatConverter : IAudioFormatConverter<WaveFormat>
    {
        /// <summary>
        /// Try to convert from the standard "Fundamental" audio format in to a propitiatory one.
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryConvert(IAudioFormat audioFormat, out WaveFormat result)
        {
            result = null;
            return false;
        }

        /// <summary>
        /// Try to convert from the a  propitiatory audio format in to the standard "Fundamental" audio format
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryConvert(WaveFormat audioFormat, out IAudioFormat result)
        {
            result = null;
            return false;
        }
    }
}
