using System;

namespace Fundamental.Interface.Wasapi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComThreadInteropStrategy
    {

        /// <summary>
        /// check to see if the current thread requires  invoke.
        /// </summary>
        /// <returns></returns>
        bool RequiresInvoke();

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The arguments.</param>
        object InvokeOnTargetThread(Delegate method, params object[] args);


    }
}
