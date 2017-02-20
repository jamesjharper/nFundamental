using System;
using System.IO;
using Fundamental.Core.Tests.Math;
using MiscUtil.IO;
using Fundamental.Wave.Container.Riff;
using MiscUtil.Conversion;
using NUnit.Framework;

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
            var reader = new EndianBinaryReader(EndianBitConverter.Little, memoryStream);
            var fixture = RiffChunk.ReadFromStream(reader);

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
            var reader = new EndianBinaryReader(EndianBitConverter.Big, memoryStream);
            var fixture = RiffChunk.ReadFromStream(reader);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.MmioId);
            Assert.AreEqual(54, fixture.ContentByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8, fixture.Location);
        }


        [Test]
        public void CanReadAtOffsetedPositionLittleEndianRiffChunk()
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

            memoryStream.Position = 0;

            // -> ACT
            var reader = new EndianBinaryReader(EndianBitConverter.Big, memoryStream);
            var fixture = RiffChunk.ReadFromStream(reader);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.MmioId);
            Assert.AreEqual(54, fixture.ContentByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8, fixture.TotalByteSize);
            Assert.AreEqual(8, fixture.Location);
        }
    }
}
