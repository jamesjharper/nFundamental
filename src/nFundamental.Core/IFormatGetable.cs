using Fundamental.Core.AudioFormats;

namespace Fundamental.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFormatGetable
    {

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <returns></returns>
        IAudioFormat GetFormat();
    }
}
