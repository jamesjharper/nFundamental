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
        static readonly object[] TestParams =
        {
            new object[] {Endianness.Little},
            new object[] {Endianness.Big},
        };
        // Read

        // Read Empty

        [Test, TestCaseSource(nameof(TestParams))]
        public void CanReadEmptyIffListHeader(Endianness endianness)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Riff Header

            // RIFF written in ASCII
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });

            // Riff Size written in little Endian bytes
            memoryStream.Write(EndianHelpers.ToEndianBytes(0, endianness));

            // mIwF written in ASCII
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatListChunk.ReadFromStream(memoryStream, endianness);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.TypeId);
            Assert.AreEqual("mIwF", fixture.SubTypeId);
            Assert.AreEqual(0, fixture.SubChunkByteSize);
            Assert.AreEqual(12, fixture.HeaderByteSize);
            Assert.AreEqual(12 , fixture.TotalByteSize);
        }

        // Read non empty


        [Test, TestCaseSource(nameof(TestParams))]
        public void CanReadNonEmptyIffListHeader(Endianness endianness)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Riff Header

            // RIFF written in ASCII
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });

            // Riff Size written in little Endian bytes
            memoryStream.Write(EndianHelpers.ToEndianBytes(5 + 3 + (8 * 2), endianness));

            // mIwF written in ASCII
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write Riff Chunk 1

            // DAT1 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.ToEndianBytes(5, endianness));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 });

            // Write Riff Chunk 2

            // DAT2 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.ToEndianBytes(3, endianness));
            memoryStream.Write(new byte[] { 0x06, 0x07, 0x08 });

            memoryStream.Position = 0;

            // -> ACT
            var fixture = InterchangeFileFormatListChunk.ReadFromStream(memoryStream, endianness);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.TypeId);
            Assert.AreEqual("mIwF", fixture.SubTypeId);
            Assert.AreEqual(24, fixture.SubChunkByteSize);
            Assert.AreEqual(12, fixture.HeaderByteSize);
            Assert.AreEqual(12 + 24, fixture.TotalByteSize);

            Assert.AreEqual(2, fixture.Chunks.Count);

            Assert.AreEqual("DAT1", fixture.Chunks[0].TypeId);
            Assert.AreEqual(5, fixture.Chunks[0].ContentByteSize);
            Assert.AreEqual(20, fixture.Chunks[0].Location);

            Assert.AreEqual("DAT2", fixture.Chunks[1].TypeId);
            Assert.AreEqual(3, fixture.Chunks[1].ContentByteSize);
            Assert.AreEqual(33, fixture.Chunks[1].Location);
        }

        // Read with delegates

        [Test, TestCaseSource(nameof(TestParams))]
        public void CanDelegateReadNonEmptyIffListHeader(Endianness endianness)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Riff Header

            // RIFF written in ASCII
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });

            // Riff Size written in little Endian bytes
            memoryStream.Write(EndianHelpers.ToEndianBytes(5 + 3 + (8 * 2), endianness));

            // mIwF written in ASCII
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write Riff Chunk 1

            // DAT1 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.ToEndianBytes(5, endianness));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 });

            // Write Riff Chunk 2

            // DAT2 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.ToEndianBytes(3, endianness));
            memoryStream.Write(new byte[] { 0x06, 0x07, 0x08 });

            memoryStream.Position = 0;

            // -> ACT

            var wasCalledForChunk1 = false;
            var wasCalledForChunk2 = false;
            var wasCalledForUnexpected = false;

            var streamLocationAtChunk1 = 0L;
            var streamLocationAtChunk2 = 0L;

            var callback = new Action<InterchangeFileFormatChunk, Stream>((iff, stream) =>
            {
                var currentLocation = stream.Position;
                // Change the position to something invalid just to test the rest of the file is still read correctly
                stream.Position = 0;

                if (iff.TypeId == "DAT1")
                {
                    wasCalledForChunk1 = true;
                    streamLocationAtChunk1 = currentLocation;
                    return;
                }

                if (iff.TypeId == "DAT2")
                {
                    wasCalledForChunk2 = true;
                    streamLocationAtChunk2 = currentLocation;
                    return;
                }

                wasCalledForUnexpected = true;
            });


            var fixture = InterchangeFileFormatListChunk.ReadFromStream(memoryStream, endianness, callback);

            // -> ASSERT

            // Asset callbacks
            Assert.IsFalse(wasCalledForUnexpected);
            Assert.IsTrue(wasCalledForChunk1);
            Assert.IsTrue(wasCalledForChunk2);

            Assert.AreEqual(20, streamLocationAtChunk1);
            Assert.AreEqual(33, streamLocationAtChunk2);

            // Assert the rest loaded correctly 
            Assert.AreEqual("RIFF", fixture.TypeId);
            Assert.AreEqual("mIwF", fixture.SubTypeId);
            Assert.AreEqual(24, fixture.SubChunkByteSize);
            Assert.AreEqual(12, fixture.HeaderByteSize);
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

        [Test, TestCaseSource(nameof(TestParams))]
        public void CanWriteEmptyIffHeader(Endianness endianness)
        {
            // -> ARRANGE:
            // RIFF written in ASCII
            var expectedTypeId = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.ToEndianBytes(4, endianness); // Including the type

            // mIwF written in ASCII
            var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };
        
            var fixuture = new InterchangeFileFormatListChunk
            {
                TypeId = "RIFF",
                SubTypeId = "mIwF"
            };

            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, endianness);

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


        [Test, TestCaseSource(nameof(TestParams))]
        public void CanWriteNonEmptyIffHeader(Endianness endianness)
        {
            // -> ARRANGE:
            // RIFF written in ASCII
            var expectedTypeBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.ToEndianBytes(36 + 4, endianness); // Including the type

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
            fixuture.Write(memoryStream, endianness);

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


        [Test, TestCaseSource(nameof(TestParams))]
        public void CanWriteNonEmptyIffHeaderUsingDel(Endianness endianness)
        {
            // -> ARRANGE:
            // RIFF written in ASCII
            var expectedTypeBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.ToEndianBytes(70, endianness); // Including the type

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

            var wasCalledForChunk1 = false;
            var wasCalledForChunk2 = false;
            var wasCalledForUnexpected = false;

            var streamLocationAtChunk1 = 0L;
            var streamLocationAtChunk2 = 0L;

            var callback = new Action<InterchangeFileFormatChunk, Stream>((iff, stream) =>
            {
                var currentLocation = stream.Position;
                // Change the position to something invalid just to test the rest of the file is still read correctly
                stream.Position = 0;

                if (iff.TypeId == "DAT1")
                {
                    wasCalledForChunk1 = true;
                    iff.ContentByteSize = 20;
                    streamLocationAtChunk1 = currentLocation;
                    return;
                }

                if (iff.TypeId == "DAT2")
                {
                    wasCalledForChunk2 = true;
                    iff.ContentByteSize = 30;
                    streamLocationAtChunk2 = currentLocation;
                    return;
                }

                wasCalledForUnexpected = true;
            });

            // -> ACT
            var memoryStream = new MemoryStream();
            fixuture.Write(memoryStream, endianness, callback);

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Asset callbacks
            Assert.IsFalse(wasCalledForUnexpected);
            Assert.IsTrue(wasCalledForChunk1);
            Assert.IsTrue(wasCalledForChunk2);

            Assert.AreEqual(20, streamLocationAtChunk1);
            Assert.AreEqual(48, streamLocationAtChunk2);

            // Read the written Sig bytes
            var typeBytes = memoryStream.Read(4);

            var contentByteSizeBytes = memoryStream.Read(4);
            var subTypeBytes = memoryStream.Read(4);

            Assert.IsTrue(expectedTypeBytes.SequenceEqual(typeBytes));
            Assert.IsTrue(expectedChunckSizeBytes.SequenceEqual(contentByteSizeBytes));
            Assert.IsTrue(expectedSubTypeBytes.SequenceEqual(subTypeBytes));

            Assert.AreEqual(20, fixuture.Chunks[0].Location);
            Assert.AreEqual(48, fixuture.Chunks[1].Location);
            Assert.AreEqual(78, streamPosition);
        }
    }
}
