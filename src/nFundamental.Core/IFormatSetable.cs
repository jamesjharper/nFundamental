using Fundamental.Core.AudioFormats;

namespace Fundamental.Core
{
    public interface IFormatSetable
    {

        /// <summary>
        /// Sets the given format.
        /// </summary>
        /// <param name="format">The format.</param>
        void SetFormat(IAudioFormat format);
    }
}
