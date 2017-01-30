using System;

namespace Fundamental
{
 
    public class FundamentalException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FundamentalException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FundamentalException(string message) : base(message)
        {
        }
    }
}
