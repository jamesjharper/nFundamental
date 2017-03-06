using Fundamental.Core.Math;
using NUnit.Framework;

namespace Fundamental.Core.Tests.Math
{
    [TestFixture]
    public class RationalTests
    {

        [TestCase(/* a */ 3,     /* b */   5,     /* ans */  1)]
        [TestCase(/* a */ 12,    /* b */   60,    /* ans */  12)]
        [TestCase(/* a */ 12,    /* b */   90,    /* ans */  6)]
        [TestCase(/* a */ 435,   /* b */   345,   /* ans */  15)]
        [TestCase(/* a */ 24,    /* b */  1244,   /* ans */  4)]
        [TestCase(/* a */ 4124,  /* b */ 24235,   /* ans */  1)]
        public void CanFindGreatestCommonDivisor(int a, int b, int ans)
        {
            Assert.AreEqual(ans, Rational.GreatestCommonDivisor(a,b));
        }

        [TestCase(/* a */ 12,    /* b */    15,   /* ans */  60)]
        [TestCase(/* a */ 18,    /* b */    24,   /* ans */  72)]
        [TestCase(/* a */ 435,   /* b */   345,   /* ans */  10005)]
        [TestCase(/* a */ 24,    /* b */  1244,   /* ans */  7464)]
        [TestCase(/* a */ 4124,  /* b */ 24235,   /* ans */  99945140)]
        public void CanFindLeastCommonMultiple(int a, int b, int ans)
        {
            Assert.AreEqual(ans, Rational.LeastCommonMultiple(a, b));
        }


        [TestCase(/* a */  10,   /* b */  100,   /* ans */  10)]
        [TestCase(/* a */ 100,   /* b */  100,   /* ans */  1)]
        [TestCase(/* a */  43,   /* b */    8,   /* ans */  8)]
        [TestCase(/* a */  93,   /* b */    8,   /* ans */  8)]
        [TestCase(/* a */ 100,   /* b */   11,   /* ans */  11)]
        [TestCase(/* a */  11,   /* b */  100,   /* ans */  100)]
        public void CanFindSmallestFactor(int a, int b, int ans)
        {
            Assert.AreEqual(ans, Rational.SmallestFactor(a, b));
        }
    }
}
