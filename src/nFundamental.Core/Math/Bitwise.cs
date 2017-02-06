using System;

namespace Fundamental.Core.Math
{
    public static class Bitwise
    {

        /// <summary>
        /// Finds the numbers the of flagged bits using magic.
        /// Thank you Stack overflow!
        /// http://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public static int NumberOfSetBits(uint i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (int)(((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        /// <summary>
        /// Finds the numbers the of flagged bits using magic.
        /// Thank you Stack overflow!
        /// http://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public static int NumberOfSetBits(int i)
        {
            return NumberOfSetBits(unchecked ((uint)i));
        }

        /// <summary>
        /// Determines whether [is square of2] [the specified i].
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>
        ///   <c>true</c> if [is square of2] [the specified i]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSquareOf2(int i)
        {
            if (i == 0)
                return true;
            return NumberOfSetBits(unchecked((uint)i)) == 1;
        }

        private static readonly uint[] MulDeBruijnBit = {
                 0,  9,  1, 10, 13, 21,  2, 29, 11, 14, 16, 18, 22, 25,  3, 30,
                 8, 12, 20, 28, 15, 17, 24,  7, 19, 27, 23,  6, 26,  5,  4, 31
              };


        /// <summary>
        /// Logs x to the base of 2.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static int LogBase2(int x)
        {
            return (int)LogBase2(unchecked((uint)x));
        }

        /// <summary>
        /// Logs x to the base of 2.
        /// </summary>
        /// <param name="x">The v.</param>
        /// <returns></returns>
        public static uint LogBase2(uint x)
        {
            x |= x >> 1; x |= x >> 2; x |= x >> 4; x |= x >> 8; x |= x >> 16;
            return MulDeBruijnBit[(x * 0x07C4ACDDu) >> 27];
        }

        /// <summary>
        /// Base 2 power of the given exponent.
        /// </summary>
        /// <param name="exponent">The exponent.</param>
        /// <returns></returns>
        public static int PowerBase2(int exponent)
        {
            if(exponent > 30)
                throw new ArgumentException("Power base 2 can not exceed exponent 30 of a signed integer.");
            return 1 << exponent;
        }

        /// <summary>
        /// Base 2 power of the given exponent.
        /// </summary>
        /// <param name="exponent">The exponent.</param>
        /// <returns></returns>
        public static uint PowerBase2(uint exponent)
        {
            if (exponent > 31)
                throw new ArgumentException("Power base 2 can not exceed exponent 31 of a unsigned integer.");
            return (uint)(1 << unchecked((int)exponent));
        }


        /// <summary>
        /// Rounds to nearest base 2 power.
        /// </summary>
        /// <param name="base2">The base2.</param>
        /// <returns></returns>
        public static int RoundDownToNearestBase2Power(int base2)
        {
            return base2 == 0 ? 0 : PowerBase2(LogBase2(base2));
        }

        /// <summary>
        /// Rounds to nearest base 2 power.
        /// </summary>
        /// <param name="base2">The base2.</param>
        /// <returns></returns>
        public static uint RoundDownToNearestBase2Power(uint base2)
        {
            return base2 == 0 ? 0 : PowerBase2(LogBase2(base2));
        }
    }
}
