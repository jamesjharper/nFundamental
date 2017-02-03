using System;

namespace Fundamental.Core
{
    public interface IInterfaceOptions<out TOptions>
    {
        /// <summary>
        /// Configures the options for interface.
        /// </summary>
        /// <param name="configure">The configure.</param>
        void Configure(Action<TOptions> configure);
    }
}
