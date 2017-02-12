
using System;
using MiscUtil.Conversion;

namespace Fundamental.Core.AudioFormats
{
    public class WaveFormatToAudioFormatConverter : IAudioFormatConverter<WaveFormat>
    {
        /// <summary>
        /// Try to convert from the standard "Fundamental" audio format in to a propitiatory one.
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryConvert(IAudioFormat audioFormat, out WaveFormat result)
        {
            // This will be re factored into adapters once more wave
            // formats are supported 

            result = null;

            // Try get the encoding type
            string encoding;
            if (!audioFormat.TryGetValue(FormatKeys.Encoding, out encoding))
                return false;
            
            // The instance only supports converting PCM formats
            if (!Equals(encoding, FormatKeys.Pcm.Format))
                return false;

            // First Try see if we can Convert using WAVEFORMATEXEXTENSIBLE
            if (TryConvertAudioFormatToWaveFormatExtensible(audioFormat, out result))
                return true;

            // Second Try see if we can Convert using WAVEFORMATEX
            if (TryConvertAudioFormatToWaveFormatEx(audioFormat, out result))
                return true;

            return true;
        }

        /// <summary>
        /// Try to convert from the a  propitiatory audio format in to the standard "Fundamental" audio format
        /// </summary>
        /// <param name="audioFormat">The audio format.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryConvert(WaveFormat audioFormat, out IAudioFormat result)
        {
            // This will be re factored in to adapters once more wave
            // formats are supported 

            // First Try converting as WAVEFORMATEX
             if(TryConvertPcmWaveFormatEx(audioFormat, out result))
                return true;

            // Second Try converting as WAVEFORMATEXEXTENSIBLE
            if (TryConvertPcmWaveFormatExtensiable(audioFormat, out result))
                return true;

            return false;
        }

        // Private methods

        #region Convert IAudioFormat -> WaveFormat

        public bool TryConvertAudioFormatToWaveFormatEx(IAudioFormat audioFormat, out WaveFormat result)
        {
            // If the audio format contain speaker configuration details 
            // then WaveFormatEx does not support this information 
            if (audioFormat.ContainsKey(FormatKeys.Pcm.Speakers))
            {
                result = null;
                return false;
            }

            var endianness  = audioFormat.Value<Endianness>(FormatKeys.Endianness);
            var dataType    = audioFormat.Value<PcmDataType>(FormatKeys.Pcm.DataType);
            var sampleRate  = audioFormat.Value<int>(FormatKeys.Pcm.SampleRate);
            var channels    = audioFormat.Value<int>(FormatKeys.Pcm.Channels);
            var depth       = audioFormat.Value<int>(FormatKeys.Pcm.Depth);

            var bitConverter = GetBitConverter(endianness);
            var formatTag = dataType == PcmDataType.Int ? WaveFormatTag.Pcm : WaveFormatTag.IeeeFloat;

            result = WaveFormat.CreateFormatEx(formatTag, sampleRate, depth, channels, bitConverter);
            return true;
        }


        public bool TryConvertAudioFormatToWaveFormatExtensible(IAudioFormat audioFormat, out WaveFormat result)
        {
            // If the audio format does not contain speaker configuration details 
            // then WaveFormatEx we can not convert this format as we require it
            if (!audioFormat.ContainsKey(FormatKeys.Pcm.Speakers))
            {
                result = null;
                return false;
            }

            var endianness  = audioFormat.Value<Endianness>(FormatKeys.Endianness);
            var dataType    = audioFormat.Value<PcmDataType>(FormatKeys.Pcm.DataType);
            var sampleRate  = audioFormat.Value<int>(FormatKeys.Pcm.SampleRate);
            var depth       = audioFormat.Value<int>(FormatKeys.Pcm.Depth);
            var packing     = audioFormat.Value<int>(FormatKeys.Pcm.Packing);
            var speakers    = audioFormat.Value<Speakers>(FormatKeys.Pcm.Speakers);

            var channels = speakers.ChannelCount();

            // Channel count is optional as it can be calculated from the speaker configuration 
            if (!audioFormat.ContainsKey(FormatKeys.Pcm.Channels))
            {
                channels = audioFormat.Value<int>(FormatKeys.Pcm.Channels);
            }

            var bitConverter = GetBitConverter(endianness);
            var formatSubType = dataType == PcmDataType.Int ? AudioMediaSubType.Pcm : AudioMediaSubType.IeeeFloat;

            result = WaveFormat.CreateFormatExtensible(formatSubType, sampleRate, depth, packing, channels, speakers, bitConverter);
            return true;
        }

        private EndianBitConverter GetBitConverter(Endianness endianness)
        {
            if (endianness == Endianness.Little)
                return EndianBitConverter.Little;

            if (endianness == Endianness.Big)
                return EndianBitConverter.Big;

            throw new ArgumentOutOfRangeException(nameof(endianness), endianness, null);
        }

        #endregion

        #region Convert WaveFormat -> IAudioFormat

        private bool TryConvertPcmWaveFormatExtensiable(WaveFormat audioFormat, out IAudioFormat result)
        {
            // Check is Extensible format tag
            if (audioFormat.FormatTag != WaveFormatTag.Extensible)
            {
                result = null;
                return false;
            }

            var extensiableFormat = new WaveFormatExtensible(audioFormat);
            return TryConvertPcmWaveFormatExtensiable(extensiableFormat, out result);
        }

        private bool TryConvertPcmWaveFormatExtensiable(WaveFormatExtensible audioFormat, out IAudioFormat result)
        {
            result = null;

            if (audioFormat.SubFormat != AudioMediaSubType.Pcm &&
                audioFormat.SubFormat != AudioMediaSubType.IeeeFloat)
            {
                return false;
            }


            result = new AudioFormat
            {
                { FormatKeys.Endianness,     GetEndianness(audioFormat)},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      (int)audioFormat.ValidBitsPerSample},
                { FormatKeys.Pcm.Packing,    (int)audioFormat.BitsPerSample},
                { FormatKeys.Pcm.SampleRate, (int)audioFormat.SamplesPerSec},
                { FormatKeys.Pcm.Channels,   (int)audioFormat.Channels},
                { FormatKeys.Pcm.Speakers,   audioFormat.ChannelMask},
                { FormatKeys.Pcm.DataType,   GetDataType(audioFormat)},
            };

            return true;
        }


        private bool TryConvertPcmWaveFormatEx(WaveFormat audioFormat, out IAudioFormat result)
        {
            result = null;
            if (audioFormat.FormatTag != WaveFormatTag.Pcm &&
                audioFormat.FormatTag != WaveFormatTag.IeeeFloat)
            {
                return false;
            }

            result = new AudioFormat
            {
                { FormatKeys.Endianness,     GetEndianness(audioFormat)},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      (int)audioFormat.BitsPerSample},
                { FormatKeys.Pcm.Packing,    (int)audioFormat.BitsPerSample}, // Packing and depth are the same when using WAVEFORMATEX
                { FormatKeys.Pcm.SampleRate, (int)audioFormat.SamplesPerSec},
                { FormatKeys.Pcm.Channels,   (int)audioFormat.Channels},
                { FormatKeys.Pcm.DataType,   GetDataType(audioFormat)},
            };

            return true;
        }

        private PcmDataType GetDataType(WaveFormat audioFormat)
        {
            if (audioFormat.FormatTag == WaveFormatTag.Pcm)
                return PcmDataType.Int;
            if (audioFormat.FormatTag == WaveFormatTag.IeeeFloat)
                return PcmDataType.Ieee754;

            throw new ArgumentOutOfRangeException(nameof(audioFormat.FormatTag), audioFormat.FormatTag, null);
        }

        private PcmDataType GetDataType(WaveFormatExtensible audioFormat)
        {
            if (audioFormat.SubFormat == AudioMediaSubType.Pcm)
                return PcmDataType.Int;
            if (audioFormat.SubFormat == AudioMediaSubType.IeeeFloat)
                return PcmDataType.Ieee754;

            throw new ArgumentOutOfRangeException(nameof(audioFormat.SubFormat), audioFormat.SubFormat, null);
        }

        private Endianness GetEndianness(WaveFormat audioFormat)
        {
            if (audioFormat.BitConverter.Endianness == MiscUtil.Conversion.Endianness.BigEndian)
                return Endianness.Big;

            if (audioFormat.BitConverter.Endianness == MiscUtil.Conversion.Endianness.LittleEndian)
                return Endianness.Little;

            throw new ArgumentOutOfRangeException(nameof(audioFormat.BitConverter.Endianness), audioFormat.BitConverter.Endianness, null);
        }

        #endregion
    }
}
