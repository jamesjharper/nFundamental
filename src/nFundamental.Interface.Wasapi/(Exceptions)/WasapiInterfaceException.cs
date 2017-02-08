namespace Fundamental.Interface.Wasapi
{
    public class WasapiInterfaceException : InterfaceException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceNotAccessableException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WasapiInterfaceException(string message) : base(message)
        {
        }
    }
}
