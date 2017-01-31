namespace Fundamental.Interface.Wasapi
{
    public class DeviceNotAccessableException : InterfaceException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceNotAccessableException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DeviceNotAccessableException(string message) : base(message)
        {
        }
    }
}
