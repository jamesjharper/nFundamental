
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
        /// Finds the smallest factor x where (a * x) / b == whole number.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns></returns>
        public static int SmallestFactor(int a, int b)
        {
            var m = 1;
            while ((a * m) % b != 0) { m++; }
            return m;
        }


        /// <summary>
        /// Finds the smallest factor x where (a * x) / b == whole number.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static long SmallestFactor(long a, long b)
        {
            var m = 1;
            while ((a * m) % b != 0) { m++; }
            return m;
        }

        #endregion
    }
}
