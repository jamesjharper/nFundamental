using System;

namespace Fundamental
{
    public class Options<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Gets the default setting type.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static Options<TOptions> Default { get; } = new Options<TOptions>();

        /// <summary>
        /// Gets the indirected options value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public TOptions Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Options{TOptions}"/> class.
        /// </summary>
        private Options()
        {
            Value = new TOptions();
        }

        /// <summary>
        /// Configures the specified configure.
        /// </summary>
        /// <param name="configure">The configure.</param>
        public void Configure(Action<TOptions> configure)
        {
            configure?.Invoke(Value);
        }
    }
}
