
namespace Fundamental.Core.Math
{
    public class Rational
    {
        #region Greatest Common Divisor

        /// <summary>
        /// Finds the greatest the common divisor.
        /// </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        /// <returns> The greatest the common divisor</returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                var temp = b; b = a % b; a = temp;
            }
            return a;
        }

        /// <summary>
        /// Finds the greatest the common divisor.
        /// </summary>
        /// <param name="a">Input a.</param>
        /// <param name="b">Input b.</param>
        /// <returns> The greatest the common divisor</returns>
        public static long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b; b = a % b; a = temp;
            }
            return a;
        }

        #endregion

        #region Least Common Multiple

        /// <summary>
        /// Finds the least the common multiple.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns> The least the common multiple. </returns>
        public static int LeastCommonMultiple(int a, int b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        /// <summary>
        /// Finds the least the common multiple.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns> The least the common multiple. </returns>
        public static long LeastCommonMultiple(long a, long b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        #endregion

        #region Smallest Factor

        /// <summary>
        /// Finds the smallest factor x where a  x * (a / b) == whole number.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns></returns>
        public static int SmallestFactor(int numerator, int denominator)
        {
            var factor = 1;
            while ((numerator * factor) % denominator != 0) { factor++; }
            return factor;
        }

        /// <summary>
        /// Finds the smallest factor x where (a * x) / b == whole number.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns></returns>
        public static long SmallestFactor(long numerator, long denominator)
        {
            var factor = 1;
            while ((numerator * factor) % denominator != 0) { factor++; }
            return factor;
        }

        #endregion
    }
}
