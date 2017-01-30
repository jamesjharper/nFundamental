namespace Fundamental
{
    public interface IOptions<out TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Gets the indirected options value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        TOptions Value { get; }
    }
}
