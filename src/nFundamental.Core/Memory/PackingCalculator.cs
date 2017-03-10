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

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing sizes
        /// </summary>
        /// <param name="unitSizeA">The unit size a.</param>
        /// <param name="unitSizeB">The unit size b.</param>
        /// <returns></returns>
        public static PackingCalculator AlignedLeastCommonMultiple(int unitSizeA, int unitSizeB)
        {
            var calc = new PackingCalculator(unitSizeA);
            return calc.AlignToLeastCommonMultiple(unitSizeB);
        }

        #endregion

        /// <summary>
        /// The alignment size
        /// </summary>
        public int UnitSize { get; }

        /// <summary>
        /// The unit count
        /// </summary>
        public int UnitCount  { get; }

        /// <summary>
        /// Gets the size of the package.
        /// </summary>
        /// <value>
        /// The size of the package.
        /// </value>
        public int PackageSize  { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the alignment.</param>
        public PackingCalculator(int unitSize) : this(unitSize, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackingCalculator"/> class.
        /// </summary>
        /// <param name="unitSize">Size of the unit.</param>
        /// <param name="unitCount">The unit count.</param>
        public PackingCalculator(int unitSize, int unitCount)
        {
            UnitSize = unitSize;
            UnitCount = unitCount;
            PackageSize = unitCount * unitSize;
        }


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
        /// <returns></returns>
        public PackingCalculator AlignToEnvelope(int envelopeCount)
        {
            var alignmentFactor = Rational.SmallestFactor(PackageSize, envelopeCount);
            var alignedCount = UnitCount * alignmentFactor;
            return new PackingCalculator(UnitSize, alignedCount);
        }

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="other">The other packing calculator.</param>
        /// <returns></returns>
        public PackingCalculator AlignToLeastCommonMultiple(PackingCalculator other)
        {
            return AlignToLeastCommonMultiple(other.PackageSize);
        }

        /// <summary>
        /// Create a new packing calculator which is aligned to smallest possible common factor of both packing calculators size
        /// </summary>
        /// <param name="packageSize">The package size to align to.</param>
        /// <returns></returns>
        public PackingCalculator AlignToLeastCommonMultiple(int packageSize)
        {
            var lcm = Rational.LeastCommonMultiple(PackageSize, packageSize);
            var unitCount = lcm / UnitSize;
            return new PackingCalculator(UnitSize, unitCount);
        }


        /// <summary>
        /// Create a new packing calculator which is aligned to a single unit.
        /// </summary>
        /// <returns></returns>
        public PackingCalculator AlignToUnit()
        {
            return new PackingCalculator(UnitSize, 1);
        }

        /// <summary>
        /// Create a new packing calculator which is aligned to a the given buffer.
        /// </summary>
        /// <param name="packageSize">Size of the package.</param>
        /// <returns></returns>
        public PackingCalculator AlignToBufferSize(int packageSize)
        {
            var bufferPackageCount = packageSize / PackageSize;
            var bufferUnitCount = bufferPackageCount * UnitCount;
            return new PackingCalculator(UnitSize, bufferUnitCount);
        }

        /// <summary>
        /// Gets the alignments size, round up to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public int RoundUp(int count) =>  checked ((int)RoundUp((long)count));

        /// <summary>
        /// Gets the alignments size, round up to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public long RoundUp(long count) => count + GetUnderflowAmount(count);

        /// <summary>
        /// Gets the alignments size, round down to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public int RoundDown(int count) => unchecked ((int)RoundDown((long)count));
        

        /// <summary>
        /// Gets the alignments size, round down to the nearest whole package.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public long RoundDown(long count) => count - GetOverflowAmount(count);
        

        /// <summary>
        /// Packages the count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public int GetPackageCount(int count) => unchecked ((int)GetPackageCount((long)count));
        
        /// <summary>
        /// Packages the count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public long GetPackageCount(long count) => count / PackageSize;
        
        /// <summary>
        /// Gets the number of "items" left over once packed
        /// </summary>
        /// <param name="count">The item count.</param>
        /// <returns></returns>
        public int GetOverflowAmount(int count) => unchecked ((int)GetOverflowAmount((long)count));

        /// <summary>
        /// Gets the overflow amount.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public long GetOverflowAmount(long count) => count % PackageSize;
        
        /// <summary>
        /// Gets the number of "items" pending to create a whole number of packages
        /// </summary>
        /// <param name="count">The item count.</param>
        /// <returns></returns>
        public int GetUnderflowAmount(int count) => unchecked ((int)GetUnderflowAmount((long)count));

        /// <summary>
        /// Gets the number of "items" pending to create a whole number of packages
        /// </summary>
        /// <param name="itemCount">The item count.</param>
        /// <returns></returns>
        public long GetUnderflowAmount(long itemCount)
        {
            var overflow = GetOverflowAmount(itemCount);
            if (overflow == 0)
                return 0;
            return UnitSize - overflow;
        }
    }
}
