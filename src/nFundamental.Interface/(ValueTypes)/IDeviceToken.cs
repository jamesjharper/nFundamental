namespace Fundamental.Interface
{
    public interface IDeviceToken
    {
    }


    public class NullToken : IDeviceToken
    {
        /// <summary>
        /// Gets the flyweight value null token instance.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public static NullToken Value { get; } = new NullToken();
    }
}
