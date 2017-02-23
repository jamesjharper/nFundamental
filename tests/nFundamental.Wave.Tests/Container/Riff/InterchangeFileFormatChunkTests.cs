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

        [Test]
        public void CanReadLittleEndianRiffSubChunk()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // DATA written in ASCII
            var mmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };   
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);

            // Chunk Size written in little Endian bytes
            var chunckSizeBytes = EndianHelpers.ToLittleEndianBytes(123);
            memoryStream.Write(chunckSizeBytes, 0, chunckSizeBytes.Length);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatChunk.ReadFromStream(memoryStream, Endianness.Little);

            // -> ASSERT
            Assert.AreEqual("DATA",  fixture.TypeId);
            Assert.AreEqual(123,     fixture.ContentByteSize);
            Assert.AreEqual(8,       fixture.HeaderByteSize);
            Assert.AreEqual(123 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8,       fixture.Location);
        }

        [Test]
        public void CanReadBigEndianRiffSubChunk()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // DATA written in ASCII
            var mmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);

            // Chunk Size written in little Endian bytes
            var chunckSizeBytes = EndianHelpers.ToBigEndianBytes(54);
            memoryStream.Write(chunckSizeBytes, 0, chunckSizeBytes.Length);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatChunk.ReadFromStream(memoryStream, Endianness.Big);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.TypeId);
            Assert.AreEqual(54, fixture.ContentByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8, fixture.Location);
        }


        [Test]
        public void CanReadAtOffsetedPositionBigEndianRiffSubChunk()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write garbage
            var garbageBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            memoryStream.Write(garbageBytes, 0, garbageBytes.Length);


            // DATA written in ASCII
            var mmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);

            // Chunk Size written in little Endian bytes
            var chunckSizeBytes = EndianHelpers.ToBigEndianBytes(54);
            memoryStream.Write(chunckSizeBytes, 0, chunckSizeBytes.Length);

            memoryStream.Position = garbageBytes.Length;

            // -> ACT
            var fixture = InterchangeFileFormatChunk.ReadFromStream(memoryStream, Endianness.Big);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.TypeId);
            Assert.AreEqual(54, fixture.ContentByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8 + garbageBytes.Length, fixture.Location);
        }


        [Test]
        public void CanWriteLittleEndianRiffSubChunk()
        {
            // -> ARRANGE:

            var expectedMmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunckSizeBytes = EndianHelpers.ToLittleEndianBytes(32);

            var fixuture = new InterchangeFileFormatChunk
            {
                TypeId = "DATA",
                ContentByteSize = 32
            };


            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, Endianness.Little);


            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written MIMO bytes
            var mmioBytes = memoryStream.Read(4);
            var contentByteSizeBytes = memoryStream.Read(4);


            Assert.IsTrue(expectedMmioBytes.SequenceEqual(mmioBytes));
            Assert.IsTrue(expectedChunckSizeBytes.SequenceEqual(contentByteSizeBytes));
            Assert.AreEqual(40, streamPosition); // Expect the cursor to be at the end of the chuck data
        }


        [Test]
        public void CanWriteBigEndianRiffSubChunk()
        {
            // -> ARRANGE:

            var expectedMmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunckSizeBytes = EndianHelpers.ToBigEndianBytes(31);

            var fixuture = new InterchangeFileFormatChunk
            {
                TypeId = "DATA",
                ContentByteSize = 31
            };


            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, Endianness.Big);


            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written MIMO bytes
            var mmioBytes = memoryStream.Read(4);
            var contentByteSizeBytes = memoryStream.Read(4);


            Assert.IsTrue(expectedMmioBytes.SequenceEqual(mmioBytes));
            Assert.IsTrue(expectedChunckSizeBytes.SequenceEqual(contentByteSizeBytes));
            Assert.AreEqual(39, streamPosition); // Expect the cursor to be at the end of the chuck data
        }
    }
}
