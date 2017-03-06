using System;
using System.Collections.Generic;
using System.Linq;
using Fundamental.Core.Memory;
using NUnit.Framework;

namespace Fundamental.Core.Tests.Memory
{
    [TestFixture]
    public class PackingCalculatorTests
    {
        [TestCase(/* alignmentSize */ 1,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 1,      /* b */   1,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 1,      /* b */   2,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   1,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   2,     /* ans */  2)]
        [TestCase(/* alignmentSize */ 5,      /* b */   3,     /* ans */  3)]
        [TestCase(/* alignmentSize */ 5,      /* b */   4,     /* ans */  4)]
        [TestCase(/* alignmentSize */ 5,      /* b */   5,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   6,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   7,     /* ans */  2)]
        [TestCase(/* alignmentSize */ 5,      /* b */   8,     /* ans */  3)]
        [TestCase(/* alignmentSize */ 5,      /* b */   9,     /* ans */  4)]
        [TestCase(/* alignmentSize */ 5,      /* b */   10,    /* ans */  0)]
        public void CanFindOverflowSize(int alignmentSize, int itemCount, int ans)
        {
            var calc = new PackingCalculator(alignmentSize);
            Assert.AreEqual(ans, calc.GetOverflowAmount(itemCount));
        }

        [TestCase(/* alignmentSize */ 1,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 1,      /* b */   1,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 1,      /* b */   2,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   1,     /* ans */  4)]
        [TestCase(/* alignmentSize */ 5,      /* b */   2,     /* ans */  3)]
        [TestCase(/* alignmentSize */ 5,      /* b */   3,     /* ans */  2)]
        [TestCase(/* alignmentSize */ 5,      /* b */   4,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   5,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   6,     /* ans */  4)]
        [TestCase(/* alignmentSize */ 5,      /* b */   7,     /* ans */  3)]
        [TestCase(/* alignmentSize */ 5,      /* b */   8,     /* ans */  2)]
        [TestCase(/* alignmentSize */ 5,      /* b */   9,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   10,    /* ans */  0)]
        public void CanFindUnderflowSize(int alignmentSize, int itemCount, int ans)
        {
            var calc = new PackingCalculator(alignmentSize);
            Assert.AreEqual(ans, calc.GetUnderflowAmount(itemCount));
        }


        [TestCase(/* alignmentSize */ 1,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 1,      /* b */   1,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 1,      /* b */   2,     /* ans */  2)]
        [TestCase(/* alignmentSize */ 5,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   1,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   2,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   3,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   4,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   5,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   6,     /* ans */  10)]
        [TestCase(/* alignmentSize */ 5,      /* b */   7,     /* ans */  10)]
        [TestCase(/* alignmentSize */ 5,      /* b */   8,     /* ans */  10)]
        [TestCase(/* alignmentSize */ 5,      /* b */   9,     /* ans */  10)]
        [TestCase(/* alignmentSize */ 5,      /* b */   10,    /* ans */  10)]
        public void CanFindRoundedUpAlignment(int alignmentSize, int itemCount, int ans)
        {
            var calc = new PackingCalculator(alignmentSize);
            Assert.AreEqual(ans, calc.RoundUp(itemCount));
        }

        [TestCase(/* alignmentSize */ 1,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 1,      /* b */   1,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 1,      /* b */   2,     /* ans */  2)]
        [TestCase(/* alignmentSize */ 5,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   1,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   2,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   3,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   4,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   5,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   6,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   7,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   8,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   9,     /* ans */  5)]
        [TestCase(/* alignmentSize */ 5,      /* b */   10,    /* ans */  10)]
        public void CanFindRoundedDownAlignment(int alignmentSize, int itemCount, int ans)
        {
            var calc = new PackingCalculator(alignmentSize);
            Assert.AreEqual(ans, calc.RoundDown(itemCount));
        }


        [TestCase(/* alignmentSize */ 1,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 1,      /* b */   1,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 1,      /* b */   2,     /* ans */  2)]
        [TestCase(/* alignmentSize */ 5,      /* b */   0,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   1,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   2,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   3,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   4,     /* ans */  0)]
        [TestCase(/* alignmentSize */ 5,      /* b */   5,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   6,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   7,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   8,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   9,     /* ans */  1)]
        [TestCase(/* alignmentSize */ 5,      /* b */   10,    /* ans */  2)]
        public void CanFindPackageCount(int alignmentSize, int itemCount, int ans)
        {
            var calc = new PackingCalculator(alignmentSize);
            Assert.AreEqual(ans, calc.GetPackageCount(itemCount));
        }


        [TestCase(/* Package Size */  10, /* Count */  1, /* Envelope Size */  100, /* -> Expected: Package Size */  100, /* Item Count */  10 )]
        [TestCase(/* Package Size */ 100, /* Count */  1, /* Envelope Size */  100, /* -> Expected: Package Size */  100, /* Item Count */  1 )]
        [TestCase(/* Package Size */  43, /* Count */  1, /* Envelope Size */    8, /* -> Expected: Package Size */  344, /* Item Count */  8 )]
        [TestCase(/* Package Size */  93, /* Count */  1, /* Envelope Size */    8, /* -> Expected: Package Size */  744, /* Item Count */  8 )]
        [TestCase(/* Package Size */ 100, /* Count */  1, /* Envelope Size */   11, /* -> Expected: Package Size */ 1100, /* Item Count */  11 )]
        [TestCase(/* Package Size */  11, /* Count */  1, /* Envelope Size */  100, /* -> Expected: Package Size */ 1100, /* Item Count */  100 )]
        public void CanAlignToEnvelope(int packageSize, int itemCount, int envelopeSize, int expectedPackageSize, int expectedItemCount)
        {
            var calc = new PackingCalculator(packageSize, itemCount);
            var aCalc = calc.AlignToEnvelope(envelopeSize);

            Assert.AreEqual(expectedPackageSize, aCalc.PackageSize);
            Assert.AreEqual(expectedItemCount, aCalc.UnitCount);
        }
    }
}
