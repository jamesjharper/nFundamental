using System;

namespace Fundamental.Core
{
    /// <summary>
    /// Source Data Received Event arguments 
    /// </summary>
    public class DataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the size of the byte.
        /// </summary>
        /// <value>
        /// The size of the byte.
        /// </value>
        public int ByteSize { get;  }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAvailableEventArgs" /> class.
        /// </summary>
        /// <param name="byteSize">Size of the byte.</param>
        public DataAvailableEventArgs(int byteSize)
        {
            ByteSize = byteSize;
        }
    }
}
