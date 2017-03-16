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
    public class ChunkTests
    {
        private static readonly object[] Standard32Bit = IffStandard.Standard32Bit;

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(124, iffStandard.ByteOrder));

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("DATA",     fixture.ChunkId);
            Assert.AreEqual(124,        fixture.DataByteSize);
            Assert.AreEqual(8,          fixture.HeaderByteSize);
            Assert.AreEqual(8,          fixture.DataLocation);
            Assert.AreEqual(0,          fixture.StartLocation);
            Assert.AreEqual(124 + 8,    fixture.EndLocation);
            Assert.AreEqual(false,      fixture.IsRf64);
        }

      
        private static readonly object[] Standard32ExtendableBitOnlyFormats = new [] { IffStandard.Rf64 };

        [Test, TestCaseSource(nameof(Standard32ExtendableBitOnlyFormats))]
        public void CanRead32BitMaxSizeExtendableBitIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(0xFFFFFFFF, iffStandard.ByteOrder));

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.ChunkId);
            Assert.AreEqual(0xFFFFFFFFL - 1, fixture.DataByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(8, fixture.DataLocation);
            Assert.AreEqual(0, fixture.StartLocation);
            Assert.AreEqual(0xFFFFFFFFL - 1 + 8, fixture.EndLocation);
            Assert.AreEqual(iffStandard.Has64BitLookupChunk, fixture.IsRf64);
        }

        private static readonly object[] Standard32BitOnlyFormats = new [] { IffStandard.Iff, IffStandard.Riff };

        [Test, TestCaseSource(nameof(Standard32BitOnlyFormats))]
        public void CanRead32BitMaxSizeNonExtendableBitIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(0xFFFFFFFF, iffStandard.ByteOrder));

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.ChunkId);
            Assert.AreEqual(0xFFFFFFFFL, fixture.DataByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(8, fixture.DataLocation);
            Assert.AreEqual(0, fixture.StartLocation);
            Assert.AreEqual(0xFFFFFFFFL  + 1 + 8, fixture.EndLocation);
            Assert.AreEqual(iffStandard.Has64BitLookupChunk, fixture.IsRf64);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNonByteAlignedIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(123, iffStandard.ByteOrder));

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.ChunkId);
            Assert.AreEqual(123, fixture.DataByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(8, fixture.DataLocation);
            Assert.AreEqual(0, fixture.StartLocation);
            Assert.AreEqual(124 + 8, fixture.EndLocation);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadAtOffsettedPositionIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write garbage
            var garbageBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            var offsetPosition = garbageBytes.Length;
            memoryStream.Write(garbageBytes);
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(54, iffStandard.ByteOrder));

            memoryStream.Position = garbageBytes.Length;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.ChunkId);
            Assert.AreEqual(54,     fixture.DataByteSize);
            Assert.AreEqual(8,      fixture.HeaderByteSize);

            Assert.AreEqual(8 + offsetPosition,         fixture.DataLocation);
            Assert.AreEqual(0 + offsetPosition,         fixture.StartLocation);
            Assert.AreEqual(54 + 8 + offsetPosition,    fixture.EndLocation);

        }


        // Write

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanCreateEmptyIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunkSizeBytes = EndianHelpers.Int32Bytes(0, iffStandard.ByteOrder);

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = Chunk.ToStream("DATA", memoryStream, iffStandard);
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
        }
    }
}
