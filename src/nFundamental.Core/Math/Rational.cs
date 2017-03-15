
namespace Fundamental.Core.Math
{
    public class Rational
    {
        #region Greatest Common Divisor

        /// <summary> Finds the greatest the common divisor. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                var temp = b; b = a % b; a = temp;
            }
            return a;
        }

        /// <summary> Finds the greatest the common divisor. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static uint GreatestCommonDivisor(uint a, uint b)
        {
            while (b != 0)
            {
                var temp = b; b = a % b; a = temp;
            }
            return a;
        }


        /// <summary> Finds the greatest the common divisor. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b; b = a % b; a = temp;
            }
            return a;
        }

        /// <summary> Finds the greatest the common divisor. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static ulong GreatestCommonDivisor(ulong a, ulong b)
        {
            while (b != 0)
            {
                var temp = b; b = a % b; a = temp;
            }
            return a;
        }

        #endregion

        #region Least Common Multiple

        /// <summary> Finds the least the common multiple. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static int LeastCommonMultiple(int a, int b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        /// <summary> Finds the least the common multiple. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static uint LeastCommonMultiple(uint a, uint b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        /// <summary> Finds the least the common multiple. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static long LeastCommonMultiple(long a, long b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        /// <summary> Finds the least the common multiple. </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        public static ulong LeastCommonMultiple(ulong a, ulong b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        #endregion

        #region Smallest Factor

        /// <summary> Finds the smallest factor x where (a * x) / b == whole number. </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public static int SmallestFactor(int numerator, int denominator)
        {
            var factor = 1;
            while ((numerator * factor) % denominator != 0) { factor++; }
            return factor;
        }

        /// <summary> Finds the smallest factor x where (a * x) / b == whole number. </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public static long SmallestFactor(long numerator, long denominator)
        {
            var factor = 1L;
            while ((numerator * factor) % denominator != 0) { factor++; }
            return factor;
        }

        /// <summary> Finds the smallest factor x where (a * x) / b == whole number. </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public static uint SmallestFactor(uint numerator, uint denominator)
        {
            var factor = 1U;
            while ((numerator * factor) % denominator != 0) { factor++; }
            return factor;
        }

        /// <summary> Finds the smallest factor x where (a * x) / b == whole number. </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public static ulong SmallestFactor(ulong numerator, ulong denominator)
        {
            var factor = 1UL;
            while ((numerator * factor) % denominator != 0) { factor++; }
            return factor;
        }

        #endregion
    }
}
