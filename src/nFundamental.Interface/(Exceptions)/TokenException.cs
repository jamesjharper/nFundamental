namespace Fundamental.Interface
{
    public class TokenException : InterfaceException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TokenException(string message) : base(message)
        {
        }
    }
}
