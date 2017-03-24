using System;
using System.IO;
using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff.Headers
{
    public class HeaderMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderMetaData" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="dataStart">The data start.</param>
        /// <param name="dataLength">The data length.</param>
        /// <param name="requires64BitExtendedHeaders">if set to <c>true</c> requires 64 Bit Extended Headers.</param>
        /// <param name="iffStandard">The IFF standard.</param>
        /// <param name="source">The source.</param>
        /// <param name="header">The header.</param>
        public HeaderMetaData(long start, long dataStart, long dataLength, bool requires64BitExtendedHeaders, IffStandard iffStandard, Stream source, ChunkHeader header)
        {
            Header = header;
            Source = source;
            IffStandard = iffStandard;
            StartLocation = start;
            DataLocation = dataStart;
            DataByteSize = dataLength;
            IsRf64 = requires64BitExtendedHeaders && iffStandard.Supports64BitLookupHeaders;

            // 0xFFFFFFFF is the flag to say that this chunk's size exceeds a 32 bit int.
            if (DataByteSize == 0xFFFFFFFF &&  iffStandard.AddressSize == AddressSize.UInt32)
            {
                DataByteSize -= 1;
            }
        }

        /// <summary> The packing calculator </summary>
        public static readonly PackingCalculator Packing = PackingCalculator.Int16;

        /// <summary>  Gets a value indicating whether this instance is RF64. </summary>
        /// <value>
        ///   <c>true</c> if this instance is RF64; otherwise, <c>false</c>.
        /// </value>
        public bool IsRf64 { get; }

        /// <summary> Gets the size of the data byte. </summary>
        /// <value>
        /// The size of the data byte.
        /// </value>
        public long DataByteSize { get; }

        /// <summary> The size of the padded content byte. </summary>
        public long PaddedDataByteSize => Packing.RoundUp(DataByteSize);

        /// <summary> The size of the header byte. </summary>
        public long HeaderByteSize => DataLocation - StartLocation;

        /// <summary> Gets the data segment location in the base stream </summary>
        public long DataLocation { get; }

        /// <summary> Gets the start segment location in the base stream </summary>
        public long StartLocation { get; }

        /// <summary>
        /// Gets the end location.
        /// Note: EA IFF 85 Standard for Interchange Format Files states that
        /// chucks should be 16bit aligned 
        /// </summary>
        public Int64 EndLocation => DataLocation + PaddedDataByteSize;

        /// <summary> The IFF standard. </summary>
        public IffStandard IffStandard { get; }

        /// <summary> Gets the source stream.</summary>
        public Stream Source { get; }

        /// <summary> Gets the chunk header.</summary>
        public ChunkHeader Header { get; }

        /// <summary>
        /// Shifts the location.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns></returns>
        public HeaderMetaData ShiftLocation(long vector)
        {
            return new HeaderMetaData(StartLocation + vector, DataLocation + vector, DataByteSize, IsRf64, IffStandard, Source, Header);
        }

        /// <summary>
        /// Adjusts the length.
        /// </summary>
        /// <param name="newLength">The new length.</param>
        /// <returns></returns>
        public HeaderMetaData AdjustLength(long newLength)
        {
            return new HeaderMetaData(StartLocation, DataLocation, newLength, DataByteSize >= newLength, IffStandard, Source, Header);
        }
    }
}
