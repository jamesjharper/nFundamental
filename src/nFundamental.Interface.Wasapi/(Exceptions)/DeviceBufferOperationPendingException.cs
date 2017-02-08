namespace Fundamental.Interface.Wasapi
{
    public class DeviceBufferOperationPendingException : WasapiInterfaceException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceNotAccessableException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DeviceBufferOperationPendingException(string message) : base(message)
        {
        }
    }
}
