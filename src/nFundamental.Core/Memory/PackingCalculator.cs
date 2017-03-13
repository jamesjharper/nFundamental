using Fundamental.Core.Math;

namespace Fundamental.Core.Memory
{
    public class PackingCalculator
    {
        #region static members

        public static PackingCalculator Int8 { get; } = new PackingCalculator(sizeof(byte));

        public static PackingCalculator Int16 { get; } = new PackingCalculator(sizeof(short));

        public static PackingCalculator Int32 { get; } = new PackingCalculator(sizeof(int));

        public static PackingCalculator Int64 { get; } = new PackingCalculator(sizeof(long));

        #endregion


        /// <summary>
        /// The alignment size
        /// </summary>
        public uint UnitSize { get; }

        /// <summary>
        /// The unit count
        /// </summary>
        public uint UnitCount  { get; }

        /// <summary>
        /// Gets the size of the package.
        /// </summary>
        /// <value>
        /// The size of the package.
        /// </value>
        public uint PackageSize  { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the alignment.</param>
        public PackingCalculator(int unitSize) 
            : this(checked((uint)unitSize))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the unit.</param>
        /// <param name="unitCount">The unit count.</param>
        public PackingCalculator(int unitSize, int unitCount)
            : this(checked((uint)unitSize), checked((uint)unitCount))
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the alignment.</param>
        public PackingCalculator(uint unitSize) 
            : this(unitSize, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the unit.</param>
        /// <param name="unitCount">The unit count.</param>
        public PackingCalculator(uint unitSize, uint unitCount)
        {
            UnitSize = unitSize;
            UnitCount = unitCount;
            PackageSize = unitCount * unitSize;
        }

        /// <summary>
        /// Create a new packing calculator which is aligned to a single unit.
        /// </summary>
        /// <returns></returns>
        public PackingCalculator AlignToUnit()
        {
            return new PackingCalculator(UnitSize, 1);
        }

        #region Align To Envelope

        /// <summary>
        /// Create a new packing calculator which is aligned to a given envelope size
        /// </summary>
        /// <param name="other">The other packing calculator.</param>
        /// <returns></returns>
        public PackingCalculator AlignToEnvelope(PackingCalculator other)
        {
            return AlignToEnvelope(other.PackageSize);
        }

        /// <summary>
        /// Create a new packing calculator which is aligned to a given envelope size
        /// </summary>
        /// <param name="envelopeCount">The envelope count.</param>
        public PackingCalculator AlignToEnvelope(long envelopeCount) 
            => AlignToEnvelope(checked((uint)envelopeCount));

        /// <summary>
        /// Create a new packing calculator which is aligned to a given envelope size
        /// </summary>
        /// <param name="envelopeCount">The envelope count.</param>
        public PackingCalculator AlignToEnvelope(int envelopeCount)
            => AlignToEnvelope(checked((uint)envelopeCount));

        /// <summary>
        /// Create a new packing calculator which is aligned to a given envelope size
        /// </summary>
        /// <param name="envelopeCount">The envelope count.</param>
        public PackingCalculator AlignToEnvelope(uint envelopeCount)
        {
            var alignmentFactor = Rational.SmallestFactor(PackageSize, envelopeCount);
            var alignedCount = UnitCount * alignmentFactor;
            return new PackingCalculator(UnitSize, alignedCount);
        }

        #endregion

        #region Align To Least Common Multiple

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing sizes
        /// </summary>
        /// <param name="unitSizeA">The unit size a.</param>
        /// <param name="unitSizeB">The unit size b.</param>
        public static PackingCalculator AlignedLeastCommonMultiple(int unitSizeA, int unitSizeB)
            => AlignedLeastCommonMultiple(checked((uint)unitSizeA), checked((uint)unitSizeB));

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing sizes
        /// </summary>
        /// <param name="unitSizeA">The unit size a.</param>
        /// <param name="unitSizeB">The unit size b.</param>
        public static PackingCalculator AlignedLeastCommonMultiple(long unitSizeA, long unitSizeB)
            => AlignedLeastCommonMultiple(checked((uint)unitSizeA), checked((uint)unitSizeB));

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing sizes
        /// </summary>
        /// <param name="unitSizeA">The unit size a.</param>
        /// <param name="unitSizeB">The unit size b.</param>
        public static PackingCalculator AlignedLeastCommonMultiple(uint unitSizeA, uint unitSizeB)
        {
            var calc = new PackingCalculator(unitSizeA);
            return calc.AlignToLeastCommonMultiple(unitSizeB);
        }

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="other">The other packing calculator.</param>
        public PackingCalculator AlignToLeastCommonMultiple(PackingCalculator other)
        {
            return AlignToLeastCommonMultiple(other.PackageSize);
        }

        /// <summary>
        /// Aligns to least common multiple.
        /// </summary>
        /// <param name="packageSize">Size of the package.</param>
        public PackingCalculator AlignToLeastCommonMultiple(int packageSize)
            => AlignToLeastCommonMultiple(checked((uint)packageSize));

        /// <summary>
        /// Aligns to least common multiple.
        /// </summary>
        /// <param name="packageSize">Size of the package.</param>
        public PackingCalculator AlignToLeastCommonMultiple(long packageSize)
            => AlignToLeastCommonMultiple(checked((uint)packageSize));

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="packageSize">The package size to align to.</param>
        public PackingCalculator AlignToLeastCommonMultiple(uint packageSize)
        {
            var lcm = Rational.LeastCommonMultiple(PackageSize, packageSize);
            var unitSize = System.Math.Max(UnitSize, packageSize);
            var unitCount = lcm / unitSize;
            return new PackingCalculator(unitSize, unitCount);
        }

        #endregion

        #region Align To Greatest Common Divisor

        /// <summary>
        /// Create a new packing calculator which is aligned to largest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="unitSizeA">The unit size a.</param>
        /// <param name="unitSizeB">The unit size b.</param>
        public static PackingCalculator AlignToGreatestCommonDivisor(int unitSizeA, int unitSizeB)
            => AlignToGreatestCommonDivisor(checked((uint)unitSizeA), checked((uint)unitSizeB));

        /// <summary>
        /// Create a new packing calculator which is aligned to largest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="unitSizeA">The unit size a.</param>
        /// <param name="unitSizeB">The unit size b.</param>
        public static PackingCalculator AlignToGreatestCommonDivisor(long unitSizeA, long unitSizeB)
            => AlignToGreatestCommonDivisor(checked((uint)unitSizeA), checked((uint)unitSizeB));

        /// <summary>
        /// Aligns to greatest common divisor.
        /// </summary>
        /// <param name="unitSizeA">The unit size a.</param>
        /// <param name="unitSizeB">The unit size b.</param>
        public static PackingCalculator AlignToGreatestCommonDivisor(uint unitSizeA, uint unitSizeB)
        {
            var calc = new PackingCalculator(unitSizeA);
            return calc.AlignToGreatestCommonDivisor(unitSizeB);
        }

        /// <summary>
        /// Create a new packing calculator which is aligned to largest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="packageSize">The package size to align to.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(int packageSize) 
            => AlignToGreatestCommonDivisor(checked((uint)packageSize));

        /// <summary>
        /// Create a new packing calculator which is aligned to largest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="packageSize">The package size to align to.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(long packageSize)
            => AlignToGreatestCommonDivisor(checked((uint)packageSize));

        /// <summary>
        /// Create a new packing calculator which is aligned to largest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="packageSize">The package size to align to.</param>
        public PackingCalculator AlignToGreatestCommonDivisor(uint packageSize)
        {
            var gcd = Rational.GreatestCommonDivisor(PackageSize, packageSize);
            return new PackingCalculator(gcd, 1);
        }

        #endregion

        #region Align To Buffer Size

        /// <summary>
        /// Create a new packing calculator which is aligned to a the given buffer.
        /// </summary>
        /// <param name="packageSize">Size of the package.</param>
        public PackingCalculator AlignToBufferSize(int packageSize) 
            => AlignToBufferSize(checked((uint)packageSize));

        /// <summary>
        /// Create a new packing calculator which is aligned to a the given buffer.
        /// </summary>
        /// <param name="packageSize">Size of the package.</param>
        public PackingCalculator AlignToBufferSize(long packageSize)
            => AlignToBufferSize(checked((uint)packageSize));

        /// <summary>
        /// Create a new packing calculator which is aligned to a the given buffer.
        /// </summary>
        /// <param name="packageSize">Size of the package.</param>
        public PackingCalculator AlignToBufferSize(uint packageSize)
        {
            var bufferPackageCount = packageSize / PackageSize;
            var bufferUnitCount = bufferPackageCount * UnitCount;
            return new PackingCalculator(UnitSize, bufferUnitCount);
        }

        #endregion

        #region Get Package Count

        /// <summary>
        /// Packages the count.
        /// </summary>
        /// <param name="count">The count.</param>
        public int GetPackageCount(int count) => checked((int)GetPackageCount(checked((uint)count)));

        /// <summary>
        /// Packages the count.
        /// </summary>
        /// <param name="count">The count.</param>
        public long GetPackageCount(long count) => GetPackageCount(checked((uint)count));

        /// <summary>
        /// Gets the package count.
        /// </summary>
        /// <param name="count">The count.</param>
        public uint GetPackageCount(uint count) => count / PackageSize;

        #endregion

        #region RoundUp

        /// <summary>
        /// Gets the alignments size, round up to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        public int RoundUp(int count) => checked ((int)RoundUp(checked((uint)count)));

        /// <summary>
        /// Gets the alignments size, round up to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        public long RoundUp(long count) => RoundUp(checked((uint)count));

        /// <summary>
        /// Gets the alignments size, round up to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        public uint RoundUp(uint count) => count + GetUnderflowAmount(count);

        #endregion

        #region RoundUp

        /// <summary>
        /// Gets the alignments size, round down to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        public int RoundDown(int count) => checked((int)RoundDown(checked((uint)count)));

        /// <summary>
        /// Gets the alignments size, round down to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        public long RoundDown(long count) => RoundDown(checked((uint)count));

        /// <summary>
        /// Gets the alignments size, round down to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        public uint RoundDown(uint count) => count - GetOverflowAmount(count);

        #endregion

        #region Get Overflow Amount

        /// <summary>
        /// Gets the number of "items" left over once packed
        /// </summary>
        /// <param name="count">The item count.</param>
        /// <returns></returns>
        public int GetOverflowAmount(int count) => checked((int)GetOverflowAmount(checked((uint)count)));

        /// <summary>
        /// Gets the overflow amount.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public long GetOverflowAmount(long count) => GetOverflowAmount(checked((uint)count));

        /// <summary>
        /// Gets the overflow amount.
        /// </summary>
        /// <param name="count">The count.</param>
        public uint GetOverflowAmount(uint count) => count % PackageSize;

        #endregion

        #region Get Overflow Amount

        /// <summary>
        /// Gets the number of "items" pending to create a whole number of packages
        /// </summary>
        /// <param name="count">The item count.</param>
        /// <returns></returns>
        public int GetUnderflowAmount(int count) => checked((int)GetUnderflowAmount(checked((uint)count)));

        /// <summary>
        /// Gets the number of "items" pending to create a whole number of packages
        /// </summary>
        /// <param name="count">The item count.</param>
        /// <returns></returns>
        public long GetUnderflowAmount(long count) => GetUnderflowAmount(checked((uint)count));

        /// <summary>
        /// Gets the number of "items" pending to create a whole number of packages
        /// </summary>
        /// <param name="itemCount">The item count.</param>
        /// <returns></returns>
        public uint GetUnderflowAmount(uint itemCount)
        {
            var overflow = GetOverflowAmount(itemCount);
            if (overflow == 0)
                return 0;
            return UnitSize - overflow;
        }

        #endregion

    }
}
