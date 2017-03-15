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
            Assert.AreEqual(ans, calc.GetWholePackageCount(itemCount));
        }


        [TestCase(/* Unit Size */  10, /* Count */  1, /* Envelope Size */  100, /* -> Expected: Package Size */  100, /* Item Count */  10 )]
        [TestCase(/* Unit Size */ 100, /* Count */  1, /* Envelope Size */  100, /* -> Expected: Package Size */  100, /* Item Count */  1 )]
        [TestCase(/* Unit Size */  43, /* Count */  1, /* Envelope Size */    8, /* -> Expected: Package Size */  344, /* Item Count */  8 )]
        [TestCase(/* Unit Size */  93, /* Count */  1, /* Envelope Size */    8, /* -> Expected: Package Size */  744, /* Item Count */  8 )]
        [TestCase(/* Unit Size */ 100, /* Count */  1, /* Envelope Size */   11, /* -> Expected: Package Size */ 1100, /* Item Count */  11 )]
        [TestCase(/* Unit Size */  11, /* Count */  1, /* Envelope Size */  100, /* -> Expected: Package Size */ 1100, /* Item Count */  100 )]
        public void CanAlignToEnvelope(int unitSize, int itemCount, int envelopeSize, int expectedPackageSize, int expectedItemCount)
        {
            var calc = new PackingCalculator(unitSize, itemCount);
            var aCalc = calc.AlignToEnvelope(envelopeSize);

            Assert.AreEqual(expectedPackageSize, aCalc.PackageSize);
            Assert.AreEqual(expectedItemCount, aCalc.UnitCount);
        }

        [TestCase(/* Unit Size */ 11, /* Count */   2, /* package Size */ 2, /* -> Expected: Package Size */  22, /* Item Count */  2)]
        [TestCase(/* Unit Size */ 11, /* Count */   2, /* package Size */ 2, /* -> Expected: Package Size */  22, /* Item Count */  2)]
        [TestCase(/* Unit Size */ 10, /* Count */   1, /* package Size */ 6, /* -> Expected: Package Size */  30, /* Item Count */  3)]
        [TestCase(/* Unit Size */  1, /* Count */  10, /* package Size */ 6, /* -> Expected: Package Size */  30, /* Item Count */  5)]
        [TestCase(/* Unit Size */  5, /* Count */   2, /* package Size */ 6, /* -> Expected: Package Size */  30, /* Item Count */  5)]
        public void CanAlignToLcm(int unitSize, int itemCount, int otherPackageSize, int expectedPackageSize, int expectedItemCount)
        {
            var calc = new PackingCalculator(unitSize, itemCount);
            var aCalc = calc.AlignToLeastCommonMultiple(otherPackageSize);

            Assert.AreEqual(expectedPackageSize, aCalc.PackageSize);
            Assert.AreEqual(expectedItemCount, aCalc.UnitCount);
        }

        [TestCase(/* Unit Size */  5,    /* Count */   2)]
        [TestCase(/* Unit Size */  51,   /* Count */   42)]
        [TestCase(/* Unit Size */  1124, /* Count */   2)]
        public void CanAlignToUnit(int unitSize, int itemCount)
        {
            var calc = new PackingCalculator(unitSize, itemCount);
            var aCalc = calc.AlignToUnit();

            Assert.AreEqual(unitSize, aCalc.PackageSize);
            Assert.AreEqual(1, aCalc.UnitCount);
        }

        [TestCase(/* Unit Size */ 6,  /* Count */   1, /* Buffer Size */  18, /* -> Expected: Item Count */  3)]
        [TestCase(/* Unit Size */ 6,  /* Count */   1, /* Buffer Size */  17, /* -> Expected: Item Count */  2)]
        [TestCase(/* Unit Size */ 6,  /* Count */   1, /* Buffer Size */  19, /* -> Expected: Item Count */  3)]
        [TestCase(/* Unit Size */ 11, /* Count */   1, /* Buffer Size */  22, /* -> Expected: Item Count */  2)]
        [TestCase(/* Unit Size */ 11, /* Count */  11, /* Buffer Size */ 650, /* -> Expected: Item Count */  55)]
        [TestCase(/* Unit Size */ 11, /* Count */   1, /* Buffer Size */  23, /* -> Expected: Item Count */  2)]
        [TestCase(/* Unit Size */ 11, /* Count */   2, /* Buffer Size */  11, /* -> Expected: Item Count */  0)]
        public void CanAlignToBuffer(int unitSize, int itemCount, int bufferSize, int expectedItemCount)
        {
            var calc = new PackingCalculator(unitSize, itemCount);
            var aCalc = calc.AlignToBufferSize(bufferSize);

            Assert.AreEqual(expectedItemCount, aCalc.UnitCount);
        }
    }
}
