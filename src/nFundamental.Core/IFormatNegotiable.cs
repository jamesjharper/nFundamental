using System.Collections.Generic;
using Fundamental.Core.AudioFormats;

namespace Fundamental.Core
{
    /// <summary>
    /// Format Negotiator 
    /// </summary>
    public interface IFormatNegotiable : IIsFormatSupported
    {

        /// <summary>
        /// Determines whether a given format is supported and returns a list of alternatives
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="closestMatchingFormats">The closest matching formats.</param>
        /// <returns>
        ///   <c>true</c> if [is audio format supported] [the specified audio format]; otherwise, <c>false</c>.
        /// </returns>
        bool IsAudioFormatSupported(IAudioFormat audioFormat, out IEnumerable<IAudioFormat> closestMatchingFormats);


        /// <summary>
        /// Suggests the possible formats which are supported by the audio endpoint.
        /// This may return none, one or many.
        /// </summary>
        /// <param name="dontSuggestTheseFormats">The don't suggest these formats.</param>
        /// <returns></returns>
        IEnumerable<IAudioFormat> SuggestFormats(params IAudioFormat[] dontSuggestTheseFormats);

    }
}
