using System;
using Fundamental.Core.AudioFormats;

namespace Fundamental.Core
{
    public interface IAudioFormatConverter<T>
    {
        /// <summary>
        /// Try to convert from the standard "Fundamental" audio format in to a propitiatory one.
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        bool TryConvert(IAudioFormat audioFormat, out T result);

        /// <summary>
        /// Try to convert from the a  propitiatory audio format in to the standard "Fundamental" audio format
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        bool TryConvert(T audioFormat, out IAudioFormat result);
    }

    public static class AudioFormatConverterExtentions
    {
        /// <summary>
        /// Converts the specified audio format to the target type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this">The this.</param>
        /// <param name="audioFormat">The audio format.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public static T Convert<T>(this IAudioFormatConverter<T> @this, IAudioFormat audioFormat)
        {
            T result;
            if (!@this.TryConvert(audioFormat, out result))
                throw new NotSupportedException( $"The given audio format could not be converted to a {typeof(T).Name} instance.");
            return result;
        }

        /// <summary>
        /// Converts the specified audio format to the target type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this">The this.</param>
        /// <param name="audioFormat">The audio format.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public static IAudioFormat Convert<T>(this IAudioFormatConverter<T> @this, T audioFormat)
        {
            IAudioFormat result;
            if (!@this.TryConvert(audioFormat, out result))
                throw new NotSupportedException($"The given audio format of type {typeof(T).Name} could not be converted to a audio format instance.");
            return result;
        }
    }
}