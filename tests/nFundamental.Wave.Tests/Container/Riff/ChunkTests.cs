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
            Assert.AreEqual(124,        fixture.Length);
            Assert.AreEqual(8,          fixture.HeaderByteSize);
            Assert.AreEqual(124 + 8,    fixture.TotalByteSize);
            Assert.AreEqual(8,          fixture.DataLocation);
            Assert.AreEqual(0,          fixture.StartLocation);
            Assert.AreEqual(124 + 8,    fixture.EndLocation);
            Assert.AreEqual(0,          fixture.PaddingBytes);
            Assert.AreEqual(false,      fixture.IsRf64);
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
            Assert.AreEqual("DATA",     fixture.ChunkId);
            Assert.AreEqual(1,          fixture.PaddingBytes);
            Assert.AreEqual(123,        fixture.DataByteSize);
            Assert.AreEqual(123,        fixture.Length);
            Assert.AreEqual(8,          fixture.HeaderByteSize);
            Assert.AreEqual(124 + 8,    fixture.TotalByteSize);
            Assert.AreEqual(8,          fixture.DataLocation);
            Assert.AreEqual(0,          fixture.StartLocation);
            Assert.AreEqual(124 + 8,    fixture.EndLocation);
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
            Assert.AreEqual(54,     fixture.Length);
            Assert.AreEqual(8,      fixture.HeaderByteSize);
            Assert.AreEqual(54 + 8, fixture.TotalByteSize);
            Assert.AreEqual(0,      fixture.PaddingBytes);

            Assert.AreEqual(8 + offsetPosition,         fixture.DataLocation);
            Assert.AreEqual(0 + offsetPosition,         fixture.StartLocation);
            Assert.AreEqual(54 + 8 + offsetPosition,    fixture.EndLocation);
      
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedContent = new byte[] {0x01, 0x02, 0x03, 0x04};

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedContent);
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            var buffer = new byte[4];
            var bytesRead = fixture.Read(buffer, 0, buffer.Length);

            // -> ASSERT
            Assert.AreEqual(expectedContent, buffer);
            Assert.AreEqual(expectedContent.Length, bytesRead);
            Assert.AreEqual(expectedContent.Length, fixture.Position);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanOverReadIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0xff};

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedContent, 0 , 4);
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            var buffer = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff };
            var bytesRead = fixture.Read(buffer, 0, buffer.Length);

            // -> ASSERT
            Assert.AreEqual(expectedContent, buffer);
            Assert.AreEqual(4, bytesRead);
            Assert.AreEqual(4, fixture.Position);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanUnderReadIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedContent = new byte[] { 0x01, 0x02, 0xff, 0xff, 0xff };

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);

            var buffer = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff };
            var bytesRead = fixture.Read(buffer, 0, 2);

            // -> ASSERT
            Assert.AreEqual(expectedContent, buffer);
            Assert.AreEqual(2, bytesRead);
            Assert.AreEqual(2, fixture.Position);
        }


        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadFromPositionIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedContent = new byte[] { 0x03, 0x04, 0xff,  0xff, 0xff };

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);
            fixture.Position = 2;

            var buffer = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff };
            var bytesRead = fixture.Read(buffer, 0, 2);

            // -> ASSERT
            Assert.AreEqual(expectedContent, buffer);
            Assert.AreEqual(2, bytesRead);
            Assert.AreEqual(4, fixture.Position);
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
            var fixture = Chunk.Create("DATA", memoryStream, iffStandard);
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
            var expectedChunkDataBytes = new byte[] {0x01, 0x02, 0x03, 0x04};

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = Chunk.Create("DATA", memoryStream, iffStandard);

            // Write a single byte. 
            fixture.Write(expectedChunkDataBytes);
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
            var expectedChunkDataBytes = new byte[] {0xff};

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = Chunk.Create("DATA", memoryStream, iffStandard);

            // Write a single byte. 
            fixture.Write(expectedChunkDataBytes);
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
            var expectedChunkDataBytes = new byte[] {0x01, 0x02, 0x03, 0x04};

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] {0xff, 0xff, 0xff, 0xff});
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);
            fixture.Write(expectedChunkDataBytes);

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
            var expectedChunkDataBytes = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06};

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] {0xff, 0xff, 0xff, 0xff});
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);
            fixture.Write(expectedChunkDataBytes);
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes   = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var chunkDataBytes = memoryStream.Read(6);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedChunkDataBytes, chunkDataBytes);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanAppendExistingIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunkSizeBytes = EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder);
            var expectedChunkDataBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06};

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);
            fixture.Position = 4;
            fixture.Write(new byte[] { 0x05, 0x06});
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes   = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var chunkDataBytes = memoryStream.Read(6);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedChunkDataBytes, chunkDataBytes);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanPartialOverwriteExistingIffChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x44, 0x41, 0x54, 0x41 };
            var expectedChunkSizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunkDataBytes = new byte[] { 0x01, 0x02, 0xff, 0xff };

            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x41 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0xff, 0xff, 0xff, 0xff });
            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream(memoryStream, iffStandard);
            fixture.Write(new byte[] { 0x01, 0x02 });
            fixture.Flush();

            // -> ASSERT
            memoryStream.Position = 0;

            var chunkIdBytes   = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var chunkDataBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedChunkIdBytes, chunkIdBytes);
            Assert.AreEqual(expectedChunkSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedChunkDataBytes, chunkDataBytes);
        }

    }
}
