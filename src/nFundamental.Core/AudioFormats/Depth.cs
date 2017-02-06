namespace Fundamental.Core.AudioFormats
{

    public static class Depth
    {
        public const int Bit1 = 1;

        public const int Bit4 = 4;

        public const int Bit8 = 8;

        public const int Bit16 = 16;

        public const int Bit24 = 24;

        public const int Bit32 = 32;

        public const int Bit64 = 64;

        public static int[] BitDepths = 
        {
            Bit1,
            Bit4,
            Bit8,
            Bit16,
            Bit24,
            Bit32,
            Bit64
        };
    }
}
