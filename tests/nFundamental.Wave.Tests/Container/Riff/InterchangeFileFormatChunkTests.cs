using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

using Fundamental.Core.Tests.Math;
using Fundamental.Core.Memory;
using Fundamental.Wave.Container.Iff;


namespace Fundamental.Core.Tests.Container.Riff
{
    [TestFixture]
    public class InterchangeFileFormatChunkTests
    {
        static readonly object[] TestParams =
        {
            new object[] {Endianness.Little},
            new object[] {Endianness.Big},
        };

        [Test, TestCaseSource(nameof(TestParams))]
        public void CanReadIffChunk(Endianness endianness)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // DATA written in ASCII
            var mmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };   
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);

            // Chunk Size written in little Endian bytes
            memoryStream.Write(EndianHelpers.ToEndianBytes(124, endianness));

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatChunk.FromStream(memoryStream, endianness);

            // -> ASSERT
            Assert.AreEqual("DATA",  fixture.ChunkId);
            Assert.AreEqual(124,     fixture.DataByteSize);
            Assert.AreEqual(8,       fixture.HeaderByteSize);
            Assert.AreEqual(124 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8,       fixture.DataLocation);
            Assert.AreEqual(0,       fixture.StartLocation);
            Assert.AreEqual(124 + 8, fixture.EndLocation);
            Assert.AreEqual(0,       fixture.PaddingBytes);
        }

        [Test, TestCaseSource(nameof(TestParams))]
        public void CanReadNonByteAlignedIffChunk(Endianness endianness)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // DATA written in ASCII
            var mmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);

            // Chunk Size written in little Endian bytes
            memoryStream.Write(EndianHelpers.ToEndianBytes(123, endianness));

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatChunk.FromStream(memoryStream, endianness);

            // -> ASSERT
            Assert.AreEqual("DATA",     fixture.ChunkId);
            Assert.AreEqual(1,          fixture.PaddingBytes);
            Assert.AreEqual(123,        fixture.DataByteSize);
            Assert.AreEqual(8,          fixture.HeaderByteSize);
            Assert.AreEqual(124 + 8,    fixture.TotalByteSize);
            Assert.AreEqual(8,          fixture.DataLocation);
            Assert.AreEqual(0,          fixture.StartLocation);
            Assert.AreEqual(124 + 8,    fixture.EndLocation); 
        }


        [Test, TestCaseSource(nameof(TestParams))]
        public void CanReadAtOffsettedPositionIffChunk(Endianness endianness)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write garbage
            var garbageBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            var offsetPosition = garbageBytes.Length;
            memoryStream.Write(garbageBytes);

            // DATA written in ASCII
            var mmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);

            // Chunk Size written in little Endian bytes
            memoryStream.Write(EndianHelpers.ToEndianBytes(54, endianness));

            memoryStream.Position = garbageBytes.Length;

            // -> ACT
            var fixture = InterchangeFileFormatChunk.FromStream(memoryStream, endianness);

            // -> ASSERT
            Assert.AreEqual("DATA",     fixture.ChunkId);
            Assert.AreEqual(54,         fixture.DataByteSize);
            Assert.AreEqual(8,          fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8,     fixture.TotalByteSize);
            Assert.AreEqual(0,          fixture.PaddingBytes);

            Assert.AreEqual(8 + offsetPosition,         fixture.DataLocation);
            Assert.AreEqual(0 + offsetPosition,         fixture.StartLocation);
            Assert.AreEqual(54 + 8 + offsetPosition,    fixture.EndLocation);
        }



        [Test, TestCaseSource(nameof(TestParams))]
        public void CanWriteIffChunk(Endianness endianness)
        {
            // -> ARRANGE:

            var expectedMmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunckSizeBytes = EndianHelpers.ToEndianBytes(32, endianness); 

            var fixture = InterchangeFileFormatChunk.Create
            (
                /* ChunkId */ "DATA",
                /* ContentByteSize */ 32
            );

            // -> ACT
            var memoryStream = new MemoryStream();
            fixture.Write(memoryStream, endianness);

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written Type Id bytes
            var typeIdBytes          = memoryStream.Read(4);
            var contentByteSizeBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedMmioBytes, typeIdBytes);
            Assert.AreEqual(expectedChunckSizeBytes, contentByteSizeBytes);
            Assert.AreEqual(8, streamPosition); 
        }

        [Test, TestCaseSource(nameof(TestParams))]
        public void CanWriteNonAlignedIffChunk(Endianness endianness)
        {
            // -> ARRANGE:
            var expectedMmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunckSizeBytes = EndianHelpers.ToEndianBytes(31, endianness); 

            var fixture = InterchangeFileFormatChunk.Create
            (
                /* ChunkId */ "DATA",
                /* ContentByteSize */ 31
            );

            // -> ACT
            var memoryStream = new MemoryStream();
            fixture.Write(memoryStream, endianness);

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written Type Id bytes
            var typeIdBytes = memoryStream.Read(4);
            var contentByteSizeBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedMmioBytes, typeIdBytes);
            Assert.AreEqual(expectedChunckSizeBytes, contentByteSizeBytes);
            Assert.AreEqual(8, streamPosition);
        }
    }
}
