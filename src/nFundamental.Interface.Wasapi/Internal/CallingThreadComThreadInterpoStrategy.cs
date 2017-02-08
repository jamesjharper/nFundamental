using System;

namespace Fundamental.Interface.Wasapi.Internal
{
    /// <summary>
    /// IComThreadInterpoStrategy which simply Invokes on the calling thread
    /// </summary>
    /// <seealso cref="Fundamental.Interface.Wasapi.IComThreadInterpoStrategy" />
    public class CallingThreadComThreadInterpoStrategy : IComThreadInterpoStrategy
    {
        /// <summary>
        /// check to see if the current thread requires  invoke.
        /// </summary>
        /// <returns></returns>
        public bool RequiresInvoke() => false;
        
        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The arguments.</param>
        public object InvokeOnTargetThread(Delegate method, params object[] args)
        {
            // Invoke on the a calling thread
            return method.DynamicInvoke(args);
        }
    }
}
