using System;
using System.Collections.Generic;
using System.Linq;
using Fundamental.Core.Math;
using NUnit.Framework;

namespace Fundamental.Core.Tests.Math
{
    [TestFixture]
    public class BitwiseTests
    {
        [TestCase(/* Input */ 0,  /* Expected */  0)]
        [TestCase(/* Input */ 1,  /* Expected */  1)]
        [TestCase(/* Input */ 2,  /* Expected */  1)]
        [TestCase(/* Input */ 3,  /* Expected */  2)]
        [TestCase(/* Input */ 16, /* Expected */  1)]
        [TestCase(/* Input */ -1, /* Expected */ 32)]
        public void CanCountBits(int input, int expectedFlaggedBitsCount)
        {
            Assert.AreEqual(expectedFlaggedBitsCount, Bitwise.NumberOfSetBits(input));
        }


        [TestCase(/* Input */ 0,  /* Expected */  true)]
        [TestCase(/* Input */ 1,  /* Expected */  true)]
        [TestCase(/* Input */ 2,  /* Expected */  true)]
        [TestCase(/* Input */ 3,  /* Expected */  false)]
        [TestCase(/* Input */ 10, /* Expected */  false)]
        [TestCase(/* Input */ 16, /* Expected */  true)]
        public void KnowsIfIsSquareOf2(int input, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, Bitwise.IsSquareOf2(input));
        }



        [TestCase(/* Input */ 0,  /* Expected */  0)]
        [TestCase(/* Input */ 1,  /* Expected */  0)]
        [TestCase(/* Input */ 2,  /* Expected */  1)]
        [TestCase(/* Input */ 3,  /* Expected */  1)]
        [TestCase(/* Input */ 4,  /* Expected */  2)]
        [TestCase(/* Input */ 5,  /* Expected */  2)]
        [TestCase(/* Input */ 16, /* Expected */  4)]
        [TestCase(/* Input */ 96, /* Expected */  6)]
        [TestCase(/* Input */ 128, /* Expected */  7)]
        public void CanFindLog2(int input, int expectedResult)
        {
            Assert.AreEqual(expectedResult, Bitwise.LogBase2(input));
        }

        [TestCase(/* Input */ 0,  /* Expected */  1)]
        [TestCase(/* Input */ 1,  /* Expected */  2)]
        [TestCase(/* Input */ 2,  /* Expected */  4)]
        [TestCase(/* Input */ 3,  /* Expected */  8)]
        [TestCase(/* Input */ 4,  /* Expected */  16)]
        [TestCase(/* Input */ 5,  /* Expected */  32)]
        [TestCase(/* Input */ 6,  /* Expected */  64)]
        [TestCase(/* Input */ 7,  /* Expected */  128)]
        [TestCase(/* Input */ 30, /* Expected */  1073741824)]
        public void CanFindPowerBase2(int input, int expectedResult)
        {
            Assert.AreEqual(expectedResult, Bitwise.PowerBase2(input));
        }


        [TestCase(/* Input */ 0,  /* Expected */  0)]
        [TestCase(/* Input */ 1,  /* Expected */  1)]
        [TestCase(/* Input */ 2,  /* Expected */  2)]
        [TestCase(/* Input */ 3,  /* Expected */  2)]
        [TestCase(/* Input */ 4,  /* Expected */  4)]
        [TestCase(/* Input */ 5,  /* Expected */  4)]
        [TestCase(/* Input */ 16, /* Expected */  16)]
        [TestCase(/* Input */ 17, /* Expected */  16)]
        [TestCase(/* Input */ 24, /* Expected */  16)]
        [TestCase(/* Input */ 96, /* Expected */  64)]
        [TestCase(/* Input */ 128, /* Expected */  128)]
        public void CanRoundToNearestBase2Power(int input, int expectedResult)
        {
            Assert.AreEqual(expectedResult, Bitwise.RoundDownToNearestBase2Power(input));
        }

    }
}
