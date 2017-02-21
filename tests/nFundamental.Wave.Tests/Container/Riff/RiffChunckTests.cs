using System;
using System.IO;
using System.Linq;

using NUnit.Framework;
using Fundamental.Core.Tests.Math;
using Fundamental.Wave.Container.Riff;
using Fundamental.Core.Memory;



namespace Fundamental.Core.Tests.Container.Riff
{
    [TestFixture]
    public class RiffChunckTests
    {

        [Test]
        public void CanReadLittleEndianRiffChunk()
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
            var fixture = RiffChunk.ReadFromStream(memoryStream, Endianness.Little);

            // -> ASSERT
            Assert.AreEqual("DATA",  fixture.MmioId);
            Assert.AreEqual(123,     fixture.ContentByteSize);
            Assert.AreEqual(8,       fixture.HeaderByteSize);
            Assert.AreEqual(123 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8,       fixture.Location);
        }

        [Test]
        public void CanReadBigEndianRiffChunk()
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
            var fixture = RiffChunk.ReadFromStream(memoryStream, Endianness.Big);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.MmioId);
            Assert.AreEqual(54, fixture.ContentByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8, fixture.Location);
        }


        [Test]
        public void CanReadAtOffsetedPositionBigEndianRiffChunk()
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
            var fixture = RiffChunk.ReadFromStream(memoryStream, Endianness.Big);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.MmioId);
            Assert.AreEqual(54, fixture.ContentByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8 + garbageBytes.Length, fixture.Location);
        }


        [Test]
        public void CanWriteLittleEndianRiffChunk()
        {
            // -> ARRANGE:

            var expectedMmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunckSizeBytes = EndianHelpers.ToLittleEndianBytes(32);

            var fixuture = new RiffChunk();

            fixuture.MmioId = "DATA";
            fixuture.ContentByteSize = 32;

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
            Assert.AreEqual(8, streamPosition);
        }


        [Test]
        public void CanWriteBigEndianRiffChunk()
        {
            // -> ARRANGE:

            var expectedMmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunckSizeBytes = EndianHelpers.ToBigEndianBytes(31);

            var fixuture = new RiffChunk();

            fixuture.MmioId = "DATA";
            fixuture.ContentByteSize = 31;

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
            Assert.AreEqual(8, streamPosition);
        }
    }
}
