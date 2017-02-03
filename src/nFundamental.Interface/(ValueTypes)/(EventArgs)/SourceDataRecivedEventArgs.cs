using Fundamental.Core;

namespace Fundamental.Interface
{
    /// <summary>
    /// Source Data Received Event arguments 
    /// </summary>
    public class SourceDataReceivedEventArgs
    { 
        /// <summary>
        /// Gets the audio data.
        /// </summary>
        /// <value>
        /// The audio data.
        /// </value>
        public IAudioData AudioData { get;  }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceDataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="audioData">The audio data.</param>
        public  SourceDataReceivedEventArgs(IAudioData audioData)
        {
            AudioData = audioData;
        }
    }
}
