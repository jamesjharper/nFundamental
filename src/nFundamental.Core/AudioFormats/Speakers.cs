using System;

namespace Fundamental.Core.AudioFormats
{
    [Flags]
    public enum Speakers : uint
    {
        FrontLeft           = 0x1,
        FrontRight          = 0x2,
        FrontCenter         = 0x4,
        LowFrequency        = 0x8,
        BackLeft            = 0x10,
        BackRight           = 0x20,
        FrontLeftOfCenter   = 0x40,
        FrontRightOfCenter  = 0x80,
        BackCenter          = 0x100,
        SideLeft            = 0x200,
        SideRight           = 0x400,
        TopCenter           = 0x800,
        TopFrontLeft        = 0x1000,
        TopFrontCenter      = 0x2000,
        TopFrontRight       = 0x4000,
        TopBackLeft         = 0x8000,
        TopBackCenter       = 0x10000,
        TopBackRight        = 0x20000,

        // Bit mask locations reserved for future use
        Reserved = 0x7FFC0000,

        // Used to specify that any possible permutation of speaker configurations
        All = 0x80000000,


        // Configurations
        // DirectSound Speaker Config
        Mono = FrontCenter,
        Stereo = FrontLeft | FrontRight,
        Quad = Stereo | BackLeft | BackRight,
        Surround = Stereo | FrontCenter | BackCenter,

        Surround5Point1 = Stereo | FrontCenter | LowFrequency | SideLeft | SideRight,
        Surround7Point1 = Surround5Point1 | BackLeft | BackRight,


        // The following are obsolete 5.1 and 7.1 settings (they lack side speakers).  Note this means
        // that the default 5.1 and 7.1 settings (KSAUDIO_SPEAKER_5POINT1 and KSAUDIO_SPEAKER_7POINT1 are
        // similarly obsolete but are unchanged for compatibility reasons).
        // 5Point1 = Quad | FrontCenter | LowFrequency,
        // 7Point1 = 5Point1 | FrontLeftOfCenter | FrontRightOfCenter,
        // 5Point1_Back = 5Point1,
        // 7Point1_Wide = 7Point1,
    }

    public static class SpeakersExtentions
    {
        /// <summary>
        /// Finds the number of channels by calculating the number of flagged bits.
        /// </summary>
        /// <param name="this">The this.</param>
        /// <returns></returns>
        public static int ChannelCount(this Speakers @this)
        {
            return (int)NumberOfFlaggedBits((uint)@this);
        }

        /// <summary>
        /// Finds the numbers the of flagged bits using magic.
        /// Thank Stack overflow
        /// http://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        private static uint NumberOfFlaggedBits(uint i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }
}
