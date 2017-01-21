

namespace Fundamental.Interface
{
    public interface IInterfaceProvider
    {
        /// <summary>
        /// Gets the audio interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetAudioInterface<T>();

        /// <summary>
        /// Determines whether [is audio interface supported].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <c>true</c> if [is audio interface supported]; otherwise, <c>false</c>.
        /// </returns>
        bool IsAudioInterfaceSupported<T>();
    }
}
