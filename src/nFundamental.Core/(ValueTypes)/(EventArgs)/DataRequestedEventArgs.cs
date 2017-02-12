
namespace Fundamental.Core
{
    /// <summary>
    /// Source Data Received Event arguments 
    /// </summary>
    public class DataRequestedEventArgs
    {
        /// <summary>
        /// Gets the size of the byte.
        /// </summary>
        /// <value>
        /// The size of the byte.
        /// </value>
        public int ByteSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAvailableEventArgs" /> class.
        /// </summary>
        /// <param name="byteSize">Size of the byte.</param>
        public DataRequestedEventArgs(int byteSize)
        {
            ByteSize = byteSize;
        }
    }
}
