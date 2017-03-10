using Fundamental.Core;

namespace Fundamental.Wave.Format
{
    public class WaveFormatInterfaceProvider : InterfaceProvider,
            ISupportsInterface<IAudioFormatConverter<WaveFormat>>
    {

        #region Singleton Access 

        /// <summary>
        /// The underlying singleton instance
        /// </summary>
        private static InterfaceProvider _singleton;

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        /// <value>
        /// The singleton.
        /// </value>
        private static InterfaceProvider Singleton => _singleton ?? (_singleton = new WaveFormatInterfaceProvider());

        /// <summary>
        /// Gets the interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInterface<T>() => Singleton.Get<T>();

        /// <summary>
        /// Determines whether [is interface supported].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <c>true</c> if [is interface supported]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInterfaceSupported<T>() => Singleton.IsSupported<T>();

        /// <summary>
        /// Gets the default provider for the target system.
        /// </summary>
        /// <returns></returns>
        public static InterfaceProvider GetProvider()
        {
            return GetInterface<InterfaceProvider>();
        }

        #endregion 

        private static IAudioFormatConverter<WaveFormat> _audioFormatConverterWaveFormatSingleton;

        /// <summary>
        /// Gets the interface for converting wave formats to audio formats.
        /// </summary>
        /// <returns></returns>
        IAudioFormatConverter<WaveFormat> ISupportsInterface<IAudioFormatConverter<WaveFormat>>.GetAudioInterface()
        {
            return _audioFormatConverterWaveFormatSingleton ?? (_audioFormatConverterWaveFormatSingleton = new WaveFormatToAudioFormatConverter());
        }
    }
}
