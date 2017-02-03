using System;

namespace Fundamental
{
    public abstract class InterfaceProvider : IInterfaceProvider
    {
        public T GetAudioInterface<T>()
        {
            var supported = this as ISupportsInterface<T>;
            if(supported == null)
                throw new NotSupportedException();
            return supported.GetAudioInterface();
        }

        /// <summary>
        /// Determines whether [is audio interface supported].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <c>true</c> if [is audio interface supported]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAudioInterfaceSupported<T>()
        {
            return this is ISupportsInterface<T>;
        }
    }
}
