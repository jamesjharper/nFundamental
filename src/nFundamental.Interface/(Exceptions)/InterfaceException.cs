namespace Fundamental.Interface
{
    public class InterfaceException : FundamentalException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InterfaceException(string message) : base(message)
        {
        }
    }
}
