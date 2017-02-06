using System;
using Fundamental.Core.Math;

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
            return Bitwise.NumberOfSetBits((uint)@this);
        }


    }
}
