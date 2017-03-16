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
    public class ValueChunkTests
    {
        #region Test Fixtures

        public class TestDataChunk : ValueChunk
        {
            private byte[] _data;

            public byte[] Data
            {
                get { return _data; }
                set
                {
                    _data = value;
                    FlagHeaderForFlush();
                }
            }

            protected override byte[] GetValueBytes()
            {
                return Data;
            }

            protected override void ReadValueBytes(byte[] data)
            {
                Data = data;
            }
        }


        #endregion

        private static readonly object[] Standard32Bit = IffStandard.Standard32Bit;

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadValueIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(124, iffStandard.ByteOrder));

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestDataChunk>(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.ChunkId);
            Assert.AreEqual(124,    fixture.DataByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(8, fixture.DataLocation);
            Assert.AreEqual(0, fixture.StartLocation);
            Assert.AreEqual(124 + 8, fixture.EndLocation);
            Assert.AreEqual(false, fixture.IsRf64);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedContent = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedContent);
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestDataChunk>(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual(expectedContent, fixture.Data);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadAtOffsettedPositionIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write garbage
            var garbageBytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff };
            var expectedContent = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var offsetPosition = garbageBytes.Length;
            memoryStream.Write(garbageBytes);
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedContent);

            memoryStream.Position = garbageBytes.Length;

            // -> ACT
            var fixture = Chunk.FromStream<TestDataChunk>(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("DATA", fixture.ChunkId);
            Assert.AreEqual(4, fixture.DataByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);

            Assert.AreEqual(expectedContent, fixture.Data);
            Assert.AreEqual(8 + offsetPosition, fixture.DataLocation);
            Assert.AreEqual(0 + offsetPosition, fixture.StartLocation);
            Assert.AreEqual(12 + offsetPosition, fixture.EndLocation);

        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNonByteAlignedIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(5, iffStandard.ByteOrder));
            memoryStream.Write(expectedContent);
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestDataChunk>(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual(expectedContent, fixture.Data);

            Assert.AreEqual("DATA", fixture.ChunkId);
            Assert.AreEqual(5, fixture.DataByteSize);
            Assert.AreEqual(8, fixture.HeaderByteSize);
            Assert.AreEqual(8, fixture.DataLocation);
            Assert.AreEqual(0, fixture.StartLocation);
            Assert.AreEqual(6 + 8, fixture.EndLocation);
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
            var fixture = ValueChunk.ToStream<TestDataChunk>("DATA", memoryStream, iffStandard);
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanCreateAlignedIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunkSizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunkDataBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = ValueChunk.ToStream<TestDataChunk>("DATA", memoryStream, iffStandard);

            // Write a single byte. 
            fixture.Data = expectedChunkDataBytes;
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var chunkDataBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedChunkDataBytes, chunkDataBytes);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanCreateNonAlignedIffChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunkSizeBytes = EndianHelpers.Int32Bytes(1, iffStandard.ByteOrder);
            var expectedChunkDataBytes = new byte[] { 0xff };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = ValueChunk.ToStream<TestDataChunk>("DATA", memoryStream, iffStandard);

            // Write a single byte. 
            fixture.Data = expectedChunkDataBytes;
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var chunkDataBytes = memoryStream.Read(1);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedChunkDataBytes, chunkDataBytes);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanOverwriteAllExistingIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunkSizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunkDataBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0xff, 0xff, 0xff, 0xff });
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestDataChunk>(memoryStream, iffStandard);
            fixture.Data = expectedChunkDataBytes;
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var chunkDataBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedChunkDataBytes, chunkDataBytes);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanExtendExistingIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunkSizeBytes = EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder);
            var expectedChunkDataBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0xff, 0xff, 0xff, 0xff });
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestDataChunk>(memoryStream, iffStandard);
            fixture.Data = expectedChunkDataBytes;
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var chunkDataBytes = memoryStream.Read(6);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedChunkDataBytes, chunkDataBytes);
        }
    }
}
