using System;

namespace Fundamental.Interface.Wasapi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComThreadInteropStrategy
    {

        /// <summary>
        /// Check to see if the current thread requires invoke.
        /// </summary>
        /// <returns> true if invoke is require </returns>
        bool RequiresInvoke();
        /// <summary>
        /// Invokes the given method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The arguments.</param>
        object InvokeOnTargetThread(Delegate method, params object[] args);
    }
}
