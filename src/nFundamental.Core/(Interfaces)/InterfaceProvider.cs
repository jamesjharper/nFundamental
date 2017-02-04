using System;

namespace Fundamental.Core
{
    public abstract class InterfaceProvider : IInterfaceProvider
    {
        public T Get<T>()
        {
            var supported = this as ISupportsInterface<T>;
            if(supported == null)
                throw new NotSupportedException($"{typeof(T).Name} is not supported by interface provider {GetType().Name}.");
            return supported.GetAudioInterface();
        }

        /// <summary>
        /// Determines whether [is audio interface supported].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <c>true</c> if [is audio interface supported]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSupported<T>()
        {
            return this is ISupportsInterface<T>;
        }
    }
}
