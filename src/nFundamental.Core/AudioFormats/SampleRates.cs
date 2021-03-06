﻿namespace Fundamental.Core.AudioFormats
{
    /// <summary>
    /// List of common sample rates
    /// </summary>
    public static class SampleRate
    {
        /// <summary>
        /// 8 kHz.
        /// Used for low quality speech.
        /// </summary>
        public const int Khz8 = 8000;

        /// <summary>
        /// 11 kHz.
        /// Used for lower-quality speech.
        /// </summary>
        public const int Khz11 = 11025;

        /// <summary>
        /// 16 kHz. A multiple of 8 kHz. 
        /// Wide-band frequency extension over standard telephone narrow-band 8,000 Hz. Used in most modern VoIP and VVoIP communication products.
        /// </summary>
        public const int Khz16 = 16000;

        /// <summary>
        /// 22 kHz. A multiple of 11 kHz. 
        /// Used for lower-quality PCM and MPEG audio and speech.
        /// </summary>
        public const int Khz22 = 22050;

        /// <summary>
        /// 32 kHz. A multiple of 16 kHz. 
        /// This sample rate generally used with 12-bit audio on DV.
        /// </summary>
        public const int Khz32 = 32000;

        /// <summary>
        /// 44.1 kHz. A multiple of 22.05 kHz. 
        /// This sample rate is used for music CDs and is a common native hardware sample rate.
        /// </summary>
        public const int Khz44 = 44100;

        /// <summary>
        /// 48 kHz. 
        /// Most digital video formats use this sample rate and is a common native hardware sample rate.
        /// </summary>
        public const int Khz48 = 48000;

        /// <summary>
        /// 88.2kHz. A multiple of 44.1 kHz. 
        // This is useful for high-resolution audio that needs to be compatible with 44.1 kHz.
        /// </summary>
        public const int Khz88 = 88200;

        /// <summary>
        /// 96 kHz. A multiple of 48 kHz.
        /// This is becoming the professional standard for audio post-production and music recording.
        /// </summary>
        public const int Khz96 = 96000;

        /// <summary>
        /// 192 kHz. A multiple of 96 kHz.
        /// A multiple of 48 and 96 kHz, this is a very high-resolution sample rate used mostly for professional music recording and mastering
        /// </summary>
        public const int Khz192 = 192000;
    }
}