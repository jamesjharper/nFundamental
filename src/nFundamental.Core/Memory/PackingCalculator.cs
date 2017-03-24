using Fundamental.Core.Math;

namespace Fundamental.Core.Memory
{
    public class PackingCalculator
    {
        #region static members

        public static PackingCalculator Int8 { get; } = new PackingCalculator(sizeof(byte));

        public static PackingCalculator Int16 { get; } = new PackingCalculator(sizeof(short));

        public static PackingCalculator Int24 { get; } = new PackingCalculator(sizeof(byte) * 3);

        public static PackingCalculator Int32 { get; } = new PackingCalculator(sizeof(int));

        public static PackingCalculator Int64 { get; } = new PackingCalculator(sizeof(long));

        #endregion
        
        // Unit size

        /// <summary> The "atomic" size of a unit </summary>
        public ulong ULongUnitSize { get; }

        /// <summary> The "atomic" size of a unit </summary>
        public int UnitSize => checked((int)ULongUnitSize);

        /// <summary> The "atomic" size of a unit </summary>
        public uint UIntUnitSize => checked ((uint) ULongUnitSize);

        // Unit Count

        /// <summary> The number of units in a single package </summary>
        public ulong ULongUnitCount { get; }

        /// <summary> The number of units in a single package </summary>
        public int UnitCount => checked((int)ULongUnitCount);

        /// <summary> The number of units in a single package </summary>
        public uint UIntUnitCount => checked((uint)ULongUnitCount);

        // Package size

        /// <summary> The total number of sub "atomic" units which make up a single package </summary>
        public ulong ULongPackageSize { get; }

        /// <summary> The total number of sub "atomic" units which make up a single package </summary>
        public int PackageSize => checked((int)ULongPackageSize);

        /// <summary> The total number of sub "atomic" units which make up a single package </summary>
        public uint UIntPackageSize => checked((uint)ULongPackageSize);

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the alignment.</param>
        public PackingCalculator(long unitSize) 
            : this(checked((ulong)unitSize))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the unit.</param>
        /// <param name="unitCount">The unit count.</param>
        public PackingCalculator(long unitSize, long unitCount)
            : this(checked((ulong)unitSize), checked((ulong)unitCount))
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the alignment.</param>
        public PackingCalculator(ulong unitSize) 
            : this(unitSize, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the unit.</param>
        /// <param name="unitCount">The unit count.</param>
        public PackingCalculator(ulong unitSize, ulong unitCount)
        {
            ULongUnitSize = unitSize;
            ULongUnitCount = unitCount;
            ULongPackageSize = unitCount * unitSize;
        }

        /// <summary> Create a new packing calculator which is aligned to a single unit. </summary>
        public PackingCalculator AlignToUnit()
        {
            return new PackingCalculator(ULongUnitSize, 1);
        }

        #region Align To Envelope

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible factor which creates whole packages for both envelope sizes </summary>
        /// <param name="other">The other packing calculator.</param>
        public PackingCalculator AlignToEnvelope(PackingCalculator other)
        {
            return AlignToEnvelope(other.ULongPackageSize);
        }

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible factor which creates whole packages for both envelope sizes </summary>
        /// <param name="envelopeUnitCount">The number of units which constituent a single envelope.</param>
        public PackingCalculator AlignToEnvelope(int envelopeUnitCount)
            => AlignToEnvelope(checked((uint)envelopeUnitCount));

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible factor which creates whole packages for both envelope sizes </summary>
        /// <param name="envelopeUnitCount">The number of units which constituent a single envelope.</param>
        public PackingCalculator AlignToEnvelope(long envelopeUnitCount)
            => AlignToEnvelope(checked((ulong)envelopeUnitCount));

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible factor which creates whole packages for both envelope sizes </summary>
        /// <param name="envelopeUnitCount">The number of units which constituent a single envelope.</param>
        public PackingCalculator AlignToEnvelope(uint envelopeUnitCount)
        {
            var alignmentFactor = Rational.SmallestFactor(UIntPackageSize, envelopeUnitCount);
            var alignedCount = ULongUnitCount * alignmentFactor;
            return new PackingCalculator(ULongUnitSize, alignedCount);
        }

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible factor which creates whole packages for both envelope sizes </summary>
        /// <param name="envelopeUnitCount">The number of units which constituent a single envelope.</param>
        public PackingCalculator AlignToEnvelope(ulong envelopeUnitCount)
        {
            var alignmentFactor = Rational.SmallestFactor(ULongPackageSize, envelopeUnitCount);
            var alignedCount = ULongUnitCount * alignmentFactor;
            return new PackingCalculator(ULongUnitSize, alignedCount);
        }

        #endregion

        #region Align To Least Common Multiple

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package "A" in units.</param>
        /// <param name="packageBUnitSize">Size of the package "B" in units.</param>
        public static PackingCalculator AlignedLeastCommonMultiple(int packageAUnitSize, int packageBUnitSize)
            => AlignedLeastCommonMultiple(checked((uint)packageAUnitSize), checked((uint)packageBUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package "A" in units.</param>
        /// <param name="packageBUnitSize">Size of the package "B" in units.</param>
        public static PackingCalculator AlignedLeastCommonMultiple(long packageAUnitSize, long packageBUnitSize)
            => AlignedLeastCommonMultiple(checked((ulong)packageAUnitSize), checked((ulong)packageBUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package "A" in units.</param>
        /// <param name="packageBUnitSize">Size of the package "B" in units.</param>
        public static PackingCalculator AlignedLeastCommonMultiple(uint packageAUnitSize, uint packageBUnitSize)
        {
            var calc = new PackingCalculator(packageAUnitSize);
            return calc.AlignToLeastCommonMultiple(packageBUnitSize);
        }

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package "A" in units.</param>
        /// <param name="packageBUnitSize">Size of the package "B" in units.</param>
        public static PackingCalculator AlignedLeastCommonMultiple(ulong packageAUnitSize, ulong packageBUnitSize)
        {
            var calc = new PackingCalculator(packageAUnitSize);
            return calc.AlignToLeastCommonMultiple(packageBUnitSize);
        }

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="other">The other package calculator to align against.</param>
        public PackingCalculator AlignToLeastCommonMultiple(PackingCalculator other)
        {
            return AlignToLeastCommonMultiple(other.ULongPackageSize);
        }

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToLeastCommonMultiple(int otherPackageUnitSize)
            => AlignToLeastCommonMultiple(checked((uint)otherPackageUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToLeastCommonMultiple(long otherPackageUnitSize)
            => AlignToLeastCommonMultiple(checked((ulong)otherPackageUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToLeastCommonMultiple(uint otherPackageUnitSize)
        {
            var lcm = Rational.LeastCommonMultiple(UIntPackageSize, otherPackageUnitSize);
            var unitSize = System.Math.Max(UIntUnitSize, otherPackageUnitSize);
            var unitCount = lcm / unitSize;
            return new PackingCalculator(unitSize, unitCount);
        }

        /// <summary> Creates a new packing calculator which is aligned to the smallest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToLeastCommonMultiple(ulong otherPackageUnitSize)
        {
            var lcm = Rational.LeastCommonMultiple(ULongPackageSize, otherPackageUnitSize);
            var unitSize = System.Math.Max(ULongUnitSize, otherPackageUnitSize);
            var unitCount = lcm / unitSize;
            return new PackingCalculator(unitSize, unitCount);
        }

        #endregion

        #region Align To Greatest Common Divisor

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package A in units.</param>
        /// <param name="packageBUnitSize">Size of the package b in units.</param>
        public static PackingCalculator AlignToGreatestCommonDivisor(int packageAUnitSize, int packageBUnitSize)
            => AlignToGreatestCommonDivisor(checked((ulong)packageAUnitSize), checked((ulong)packageBUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package A in units.</param>
        /// <param name="packageBUnitSize">Size of the package b in units.</param>
        public static PackingCalculator AlignToGreatestCommonDivisor(long packageAUnitSize, long packageBUnitSize)
            => AlignToGreatestCommonDivisor(checked((ulong)packageAUnitSize), checked((ulong)packageBUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package A in units.</param>
        /// <param name="packageBUnitSize">Size of the package b in units.</param>
        public static PackingCalculator AlignToGreatestCommonDivisor(uint packageAUnitSize, uint packageBUnitSize)
        {
            var calc = new PackingCalculator(packageAUnitSize);
            return calc.AlignToGreatestCommonDivisor(packageBUnitSize);
        }

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="packageAUnitSize">Size of the package A in units.</param>
        /// <param name="packageBUnitSize">Size of the package b in units.</param>
        public static PackingCalculator AlignToGreatestCommonDivisor(ulong packageAUnitSize, ulong packageBUnitSize)
        {
            var calc = new PackingCalculator(packageAUnitSize);
            return calc.AlignToGreatestCommonDivisor(packageBUnitSize);
        }

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="other">The other package calculator to align against.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(PackingCalculator other)
        {
            return AlignToGreatestCommonDivisor(other.ULongPackageSize);
        }

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(int otherPackageUnitSize) 
            => AlignToGreatestCommonDivisor(checked((uint)otherPackageUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(long otherPackageUnitSize)
            => AlignToGreatestCommonDivisor(checked((ulong)otherPackageUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(uint otherPackageUnitSize)
        {
            var gcd = Rational.GreatestCommonDivisor(UIntPackageSize, otherPackageUnitSize);
            return new PackingCalculator(gcd, 1);
        }

        /// <summary> Creates a new packing calculator which is aligned to the largest possible common factor of both packing sizes </summary>
        /// <param name="otherPackageUnitSize">The other package size to align against.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(ulong otherPackageUnitSize)
        {
            var gcd = Rational.GreatestCommonDivisor(ULongPackageSize, otherPackageUnitSize);
            return new PackingCalculator(gcd, 1);
        }

        #endregion

        #region Align To Buffer Size

        /// <summary> Creates a new packing calculator which is aligned to a the given buffer size. </summary>
        /// <param name="packageBufferUnitSize">The size of the buffer in number of units.</param>
        public PackingCalculator AlignToBufferSize(int packageBufferUnitSize) 
            => AlignToBufferSize(checked((uint)packageBufferUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to a the given buffer size. </summary>
        /// <param name="packageBufferUnitSize">The size of the buffer in number of units.</param>
        public PackingCalculator AlignToBufferSize(long packageBufferUnitSize)
            => AlignToBufferSize(checked((ulong)packageBufferUnitSize));

        /// <summary> Creates a new packing calculator which is aligned to a the given buffer size. </summary>
        /// <param name="packageBufferUnitSize">The size of the buffer in number of units.</param>
        public PackingCalculator AlignToBufferSize(uint packageBufferUnitSize) 
            => AlignToBufferSize((ulong)packageBufferUnitSize);

        /// <summary> Creates a new packing calculator which is aligned to a the given buffer size. </summary>
        /// <param name="packageBufferUnitSize">The size of the buffer in number of units.</param>
        public PackingCalculator AlignToBufferSize(ulong packageBufferUnitSize)
        {
            var bufferPackageCount = packageBufferUnitSize / ULongPackageSize;
            var bufferUnitCount = bufferPackageCount * ULongUnitCount;
            return new PackingCalculator(ULongUnitSize, bufferUnitCount);
        }

        #endregion

        #region Get Whole Package Count

        /// <summary> Gets the number of whole packages which can be made up from the given number of units. </summary>
        /// <param name="unitCount">The unit count.</param>
        public int GetWholePackageCount(int unitCount) => checked((int)GetWholePackageCount(checked((uint)unitCount)));

        /// <summary> Gets the number of whole packages which can be made up from the given number of units. </summary>
        /// <param name="unitCount">The unit count.</param>
        public long GetWholePackageCount(long unitCount) => checked((long)GetWholePackageCount(checked((ulong)unitCount)));

        /// <summary> Gets the number of whole packages which can be made up from the given number of units. </summary>
        /// <param name="unitCount">The unit count.</param>
        public uint GetWholePackageCount(uint unitCount) => checked((uint)GetWholePackageCount((ulong)unitCount));

        /// <summary> Gets the number of whole packages which can be made up from the given number of units. </summary>
        /// <param name="unitCount">The unit count.</param>
        public ulong GetWholePackageCount(ulong unitCount) => unitCount / ULongPackageSize;

        #endregion

        #region RoundUp

        /// <summary> Gets a packing alignments number of units, round up to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public int RoundUp(int unitCount) => checked ((int)RoundUp(checked((uint)unitCount)));

        /// <summary> Gets a packing alignments number of units, round up to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public long RoundUp(long unitCount) => checked((long)RoundUp(checked((ulong)unitCount)));

        /// <summary> Gets a packing alignments number of units, round up to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public uint RoundUp(uint unitCount) => unitCount + GetUnderflowAmount(unitCount);

        /// <summary> Gets a packing alignments number of units, round up to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public ulong RoundUp(ulong unitCount) => unitCount + GetUnderflowAmount(unitCount);

        #endregion

        #region RoundUp

        /// <summary> Gets a packing alignments number of units, round down to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public int RoundDown(int unitCount) => checked((int)RoundDown(checked((uint)unitCount)));

        /// <summary> Gets a packing alignments number of units, round down to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public long RoundDown(long unitCount) => checked((long)RoundDown(checked((ulong)unitCount)));

        /// <summary> Gets a packing alignments number of units, round down to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public uint RoundDown(uint unitCount) => unitCount - GetOverflowAmount(unitCount);

        /// <summary> Gets a packing alignments number of units, round down to the nearest whole package. </summary>
        /// <param name="unitCount">The unit count.</param>
        public ulong RoundDown(ulong unitCount) => unitCount - GetOverflowAmount(unitCount);

        #endregion

        #region Get Overflow Amount

        /// <summary> Gets the number of "units" left over once packed </summary>
        /// <param name="unitCount">The unit count.</param>
        public int GetOverflowAmount(int unitCount) => checked((int)GetOverflowAmount(checked((uint)unitCount)));

        /// <summary> Gets the number of "units" left over once packed </summary>
        /// <param name="unitCount">The unit count.</param>
        public long GetOverflowAmount(long unitCount) => checked((long)GetOverflowAmount(checked((ulong)unitCount)));

        /// <summary> Gets the number of "units" left over once packed </summary>
        /// <param name="unitCount">The unit count.</param>
        public uint GetOverflowAmount(uint unitCount) => unitCount % UIntPackageSize;

        /// <summary> Gets the number of "units" left over once packed </summary>
        /// <param name="unitCount">The unit count.</param>
        public ulong GetOverflowAmount(ulong unitCount) => unitCount % ULongPackageSize;

        #endregion

        #region Get Overflow Amount

        /// <summary> Gets the number of "units" pending to create a whole number of packages </summary>
        /// <param name="unitCount">The unit count.</param>
        public int GetUnderflowAmount(int unitCount) => checked((int)GetUnderflowAmount(checked((uint)unitCount)));

        /// <summary> Gets the number of "units" pending to create a whole number of packages </summary>
        /// <param name="unitCount">The unit count.</param>
        public long GetUnderflowAmount(long unitCount) => checked((long)GetUnderflowAmount(checked((ulong)unitCount)));

        /// <summary> Gets the number of "units" pending to create a whole number of packages </summary>
        /// <param name="unitCount">The unit count.</param>
        public uint GetUnderflowAmount(uint unitCount)
        {
            var overflow = GetOverflowAmount(unitCount);
            if (overflow == 0) return 0;
            return UIntUnitSize - overflow;
        }


        /// <summary> Gets the number of "units" pending to create a whole number of packages </summary>
        /// <param name="unitCount">The unit count.</param>
        public ulong GetUnderflowAmount(ulong unitCount)
        {
            var overflow = GetOverflowAmount(unitCount);
            if (overflow == 0) return 0;
            return ULongUnitSize - overflow;
        }

        #endregion

    }
}
