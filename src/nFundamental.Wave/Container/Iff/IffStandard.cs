using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{

    public enum AddressSize
    {
        UInt16,
        UInt32,
        UInt64
    }

    public class IffStandard
    {

        // Publish Standards

        /// <summary>
        /// RF64 is a BWF-compatible multichannel file format enabling file sizes to exceed 4 GB. 
        /// It has been specified by the European Broadcasting Union.
        /// </summary>
        public static IffStandard Rf64 { get; } = new IffStandard(Endianness.Little, true, AddressSize.UInt32);
        

        /// <summary>
        /// Interchange File Format (IFF), is a generic container file format originally introduced by the 
        /// Electronic Arts company in 1985 (in cooperation with Commodore/Amiga) in order to facilitate transfer
        /// of data between software produced by different companies.
        /// </summary>
        public static IffStandard Iff { get; } = new IffStandard(Endianness.Big, false, AddressSize.UInt32);

        /// <summary>
        /// The Resource Interchange File Format (RIFF) is a generic file container format for storing data in tagged chunks.
        /// It is primarily used to store multimedia such as sound and video, though it may also be used to store any arbitrary data.
        /// </summary>
        public static IffStandard Riff { get; } = new IffStandard(Endianness.Little, false, AddressSize.UInt32);

        // Experimental standards for exclusive use with this software

        // Native 64bit support for Iff files

        /// <summary>
        /// Experimental extension of the IFF standard, where address sizes are represented in 64bit uints 
        /// </summary>
        public static IffStandard Iff64 { get; } = new IffStandard(Endianness.Big, false, AddressSize.UInt64);

        /// <summary>
        /// Experimental extension of the RIFF standard, where address sizes are represented in 64bit uints 
        /// </summary>
        public static IffStandard Riff64 { get; } = new IffStandard(Endianness.Little, false, AddressSize.UInt64);

        /// <summary>
        /// Known 32bit IIF extended standards
        /// </summary>
        public static IffStandard[] Standard32Bit = { Rf64, Iff, Riff };

        /// <summary>
        /// Known 64bit IIF extended standards
        /// </summary>
        public static IffStandard[] Standard64Bit = { Iff64, Riff64 };

        /// <summary>
        /// Known IIF extended standards
        /// </summary>
        public static IffStandard[] Standards =  { Rf64, Iff, Riff, Iff64, Riff64 };

        /// <summary>
        /// Gets the byte order.
        /// </summary>
        /// <value>
        /// The byte order.
        /// </value>
        public Endianness ByteOrder { get; private set; } 

        /// <summary>
        /// Gets a value indicating whether this instance has external size written out side of this segment.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has external size; otherwise, <c>false</c>.
        /// </value>
        public bool Has64BitLookupChunk { get; private set; }

        /// <summary>
        /// Gets the size of the address.
        /// </summary>
        /// <value>
        /// The size of the address.
        /// </value>
        public AddressSize AddressSize { get; private set; } 

        /// <summary>
        /// Initializes a new instance of the <see cref="IffStandard"/> class.
        /// </summary>
        /// <param name="byteOrder">The byte order.</param>
        /// <param name="has64BitLookupChunk">if set to <c>true</c> [RF64].</param>
        /// <param name="addressSize">Size of the address.</param>
        public IffStandard(Endianness byteOrder, 
                                    bool has64BitLookupChunk, 
                                    AddressSize addressSize)
        {
            ByteOrder = byteOrder;
            Has64BitLookupChunk = has64BitLookupChunk;
            AddressSize = addressSize;
        }
    }
}
