using Fundamental.Core.AudioFormats;

namespace Fundamental.Core
{
    /// <summary>
    /// Format Negotiator 
    /// </summary>
    public interface IFormatNegotiator
    {

        bool IsAudioFormatFormated(IAudioFormat audioFormat, out IAudioFormat closestMatchFormat);


        IAudioFormat SuggestFormat(params IAudioFormat[] dontSuggestTheseFormats);

    }
}
