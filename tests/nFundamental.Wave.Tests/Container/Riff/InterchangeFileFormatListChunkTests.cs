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
    public class InterchangeFileFormatListChunkTests
    {

        // Read

        [Test]
        public void CanReadEmptyLittleEndianRiffHeader()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Riff Header

            // RIFF written in ASCII
            var riffFileSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            memoryStream.Write(riffFileSignature, 0, riffFileSignature.Length);

            // Riff Size written in little Endian bytes
            var contentSizeBytes = EndianHelpers.ToLittleEndianBytes(0);
            memoryStream.Write(contentSizeBytes, 0, contentSizeBytes.Length);

            // mIwF written in ASCII
            var mmioBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);
            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatListChunk.ReadFromStream(memoryStream, Endianness.Little);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.TypeId);
            Assert.AreEqual("mIwF", fixture.SubTypeId);
            Assert.AreEqual(0, fixture.SubChunkByteSize);
            Assert.AreEqual(12, fixture.ChunkByteSize);
            Assert.AreEqual(12 , fixture.TotalByteSize);
        }

        [Test]
        public void CanReadEmptyBigEndianRiffHeader()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Riff Header

            // RIFF written in ASCII
            var riffFileSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            memoryStream.Write(riffFileSignature, 0, riffFileSignature.Length);

            // Riff Size written in little Endian bytes
            var contentSizeBytes = EndianHelpers.ToBigEndianBytes(0);
            memoryStream.Write(contentSizeBytes, 0, contentSizeBytes.Length);

            // mIwF written in ASCII
            var mmioBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };
            memoryStream.Write(mmioBytes, 0, mmioBytes.Length);
            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatListChunk.ReadFromStream(memoryStream, Endianness.Big);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.TypeId);
            Assert.AreEqual("mIwF", fixture.SubTypeId);
            Assert.AreEqual(0, fixture.SubChunkByteSize);
            Assert.AreEqual(12, fixture.ChunkByteSize);
            Assert.AreEqual(12, fixture.TotalByteSize);
        }

        [Test]
        public void CanReadLittleEndianRiffHeader()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Riff Header

            // RIFF written in ASCII
            var riffFileSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            memoryStream.Write(riffFileSignature, 0, riffFileSignature.Length);

            // Riff Size written in little Endian bytes
            var contentSizeBytes = EndianHelpers.ToLittleEndianBytes(5 + 3 + (8 * 2));
            memoryStream.Write(contentSizeBytes, 0, contentSizeBytes.Length);

            // mIwF written in ASCII
            var mmioBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };
            memoryStream.Write(mmioBytes);

            // Write Riff Chunk 1

            // DAT1 written in ASCII
            var c1MmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x31 };
            memoryStream.Write(c1MmioBytes);

            var c1SizeBytes = EndianHelpers.ToLittleEndianBytes(5);
            memoryStream.Write(c1SizeBytes);

            var c1Content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            memoryStream.Write(c1Content);

            // Write Riff Chunk 2

            // DAT2 written in ASCII
            var c2MmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x32 };
            memoryStream.Write(c2MmioBytes);

            var c2SizeBytes = EndianHelpers.ToLittleEndianBytes(3);
            memoryStream.Write(c2SizeBytes);

            var c2Content = new byte[] { 0x06, 0x07, 0x08 };
            memoryStream.Write(c2Content);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatListChunk.ReadFromStream(memoryStream, Endianness.Little);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.TypeId);
            Assert.AreEqual("mIwF", fixture.SubTypeId);
            Assert.AreEqual(24, fixture.SubChunkByteSize);
            Assert.AreEqual(12, fixture.ChunkByteSize);
            Assert.AreEqual(12 + 24, fixture.TotalByteSize);

            Assert.AreEqual(2, fixture.Chunks.Count);

            Assert.AreEqual("DAT1", fixture.Chunks[0].TypeId);
            Assert.AreEqual(5, fixture.Chunks[0].ContentByteSize);
            Assert.AreEqual(20, fixture.Chunks[0].Location);

            Assert.AreEqual("DAT2", fixture.Chunks[1].TypeId);
            Assert.AreEqual(3, fixture.Chunks[1].ContentByteSize);
            Assert.AreEqual(33, fixture.Chunks[1].Location);
        }

        [Test]
        public void CanReadBigEndianRiffHeader()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Riff Header

            // RIFF written in ASCII
            var riffFileSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            memoryStream.Write(riffFileSignature, 0, riffFileSignature.Length);

            // Riff Size written in little Endian bytes
            var contentSizeBytes = EndianHelpers.ToBigEndianBytes(5 + 3 + (8 * 2));
            memoryStream.Write(contentSizeBytes, 0, contentSizeBytes.Length);

            // mIwF written in ASCII
            var mmioBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };
            memoryStream.Write(mmioBytes);

            // Write Riff Chunk 1

            // DAT1 written in ASCII
            var c1MmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x31 };
            memoryStream.Write(c1MmioBytes);

            var c1SizeBytes = EndianHelpers.ToBigEndianBytes(5);
            memoryStream.Write(c1SizeBytes);

            var c1Content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            memoryStream.Write(c1Content);

            // Write Riff Chunk 2

            // DAT2 written in ASCII
            var c2MmioBytes = new byte[] { 0x44, 0x41, 0x54, 0x32 };
            memoryStream.Write(c2MmioBytes);

            var c2SizeBytes = EndianHelpers.ToBigEndianBytes(3);
            memoryStream.Write(c2SizeBytes);

            var c2Content = new byte[] { 0x06, 0x07, 0x08 };
            memoryStream.Write(c2Content);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatListChunk.ReadFromStream(memoryStream, Endianness.Big);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.TypeId);
            Assert.AreEqual("mIwF", fixture.SubTypeId);
            Assert.AreEqual(24, fixture.SubChunkByteSize);
            Assert.AreEqual(12, fixture.ChunkByteSize);
            Assert.AreEqual(12 + 24, fixture.TotalByteSize);

            Assert.AreEqual(2, fixture.Chunks.Count);

            Assert.AreEqual("DAT1", fixture.Chunks[0].TypeId);
            Assert.AreEqual(5, fixture.Chunks[0].ContentByteSize);
            Assert.AreEqual(20, fixture.Chunks[0].Location);

            Assert.AreEqual("DAT2", fixture.Chunks[1].TypeId);
            Assert.AreEqual(3, fixture.Chunks[1].ContentByteSize);
            Assert.AreEqual(33, fixture.Chunks[1].Location);
        }


        // Write

        [Test]
        public void CanWriteEmptyLittleEndianRiffHeader()
        {
            // -> ARRANGE:
            // RIFF written in ASCII
            var expectedTypeId = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.ToLittleEndianBytes(4); // Including the type

            // mIwF written in ASCII
            var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };
        
            var fixuture = new InterchangeFileFormatListChunk
            {
                TypeId = "RIFF",
                SubTypeId = "mIwF"
            };

            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, Endianness.Little);

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written Sig bytes
            var typeBytes = memoryStream.Read(4);

            var contentByteSizeBytes = memoryStream.Read(4);
            var subTypeBytes = memoryStream.Read(4);

            Assert.IsTrue(expectedTypeId.SequenceEqual(typeBytes));
            Assert.IsTrue(expectedChunckSizeBytes.SequenceEqual(contentByteSizeBytes));
            Assert.IsTrue(expectedSubTypeBytes.SequenceEqual(subTypeBytes));
            Assert.AreEqual(12, streamPosition);
        }

        [Test]
        public void CanWriteEmptyBigEndianRiffHeader()
        {
                        // -> ARRANGE:
            // RIFF written in ASCII
            var expectedTypeId = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.ToBigEndianBytes(4); // Including the type

            // mIwF written in ASCII
            var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };
        
            var fixuture = new InterchangeFileFormatListChunk
            {
                TypeId = "RIFF",
                SubTypeId = "mIwF"
            };

            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, Endianness.Big);

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written Sig bytes
            var typeBytes = memoryStream.Read(4);

            var contentByteSizeBytes = memoryStream.Read(4);
            var subTypeBytes = memoryStream.Read(4);

            Assert.IsTrue(expectedTypeId.SequenceEqual(typeBytes));
            Assert.IsTrue(expectedChunckSizeBytes.SequenceEqual(contentByteSizeBytes));
            Assert.IsTrue(expectedSubTypeBytes.SequenceEqual(subTypeBytes));
            Assert.AreEqual(12, streamPosition);
        }


        [Test]
        public void CanWriteNonEmptyLittleEndianRiffHeader()
        {
            // -> ARRANGE:
            // RIFF written in ASCII
            var expectedTypeBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.ToLittleEndianBytes(36 + 4); // Including the type

            // mIwF written in ASCII
            var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var fixuture = new InterchangeFileFormatListChunk
            {
                TypeId = "RIFF",
                SubTypeId = "mIwF"
            };

            fixuture.Chunks.Add(new InterchangeFileFormatChunk
            {
                TypeId = "DAT1",
                ContentByteSize = 10
            });

            fixuture.Chunks.Add(new InterchangeFileFormatChunk
            {
                TypeId = "DAT2",
                ContentByteSize = 10
            });

            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, Endianness.Little);

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written Sig bytes
            var typeBytes = memoryStream.Read(4);

            var contentByteSizeBytes = memoryStream.Read(4);
            var subTypeBytes = memoryStream.Read(4);

            Assert.IsTrue(expectedTypeBytes.SequenceEqual(typeBytes));
            Assert.IsTrue(expectedChunckSizeBytes.SequenceEqual(contentByteSizeBytes));
            Assert.IsTrue(expectedSubTypeBytes.SequenceEqual(subTypeBytes));

            Assert.AreEqual(20, fixuture.Chunks[0].Location);
            Assert.AreEqual(38, fixuture.Chunks[1].Location);
            Assert.AreEqual(48, streamPosition);
        }

        [Test]
        public void CanWriteNonEmptyBigEndianRiffHeader()
        {
            // -> ARRANGE:
            // RIFF written in ASCII
            var expectedTypeBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.ToBigEndianBytes(36 + 4); // Including the type

            // mIwF written in ASCII
            var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var fixuture = new InterchangeFileFormatListChunk
            {
                TypeId = "RIFF",
                SubTypeId = "mIwF"
            };

            fixuture.Chunks.Add(new InterchangeFileFormatChunk
            {
                TypeId = "DAT1",
                ContentByteSize = 10
            });

            fixuture.Chunks.Add(new InterchangeFileFormatChunk
            {
                TypeId = "DAT2",
                ContentByteSize = 10
            });

            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, Endianness.Big);

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written Sig bytes
            var typeBytes = memoryStream.Read(4);

            var contentByteSizeBytes = memoryStream.Read(4);
            var subTypeBytes = memoryStream.Read(4);

            Assert.IsTrue(expectedTypeBytes.SequenceEqual(typeBytes));
            Assert.IsTrue(expectedChunckSizeBytes.SequenceEqual(contentByteSizeBytes));
            Assert.IsTrue(expectedSubTypeBytes.SequenceEqual(subTypeBytes));

            Assert.AreEqual(20, fixuture.Chunks[0].Location);
            Assert.AreEqual(38, fixuture.Chunks[1].Location);
            Assert.AreEqual(48, streamPosition);
        }
    }
}
