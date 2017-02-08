namespace Fundamental.Core
{
    public class FormatNotSupportedException : FundamentalException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatNotSupportedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FormatNotSupportedException(string message) : base(message)
        {
        }
    }
}
