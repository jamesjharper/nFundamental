using Fundamental.Core.AudioFormats;

namespace Fundamental.Core
{
    public interface IIsFormatSupported
    {

        /// <summary>
        /// Determines whether a given format is supported
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <returns>
        ///   <c>true</c> if [is audio format supported] [the specified audio format]; otherwise, <c>false</c>.
        /// </returns>
        bool IsAudioFormatSupported(IAudioFormat audioFormat);
    }
}
