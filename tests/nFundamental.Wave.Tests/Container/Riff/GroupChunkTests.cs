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
    public class GroupChunkTests
    {
        private static readonly object[] Standard32Bit = IffStandard.Standard32Bit;

        // Read

        // Read Empty

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadEmptyIffListHeader(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            memoryStream.Position = 0;

            // -> ACT
            var fixture = GroupChunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.ChunkId);
            Assert.AreEqual("mIwF", fixture.TypeId);
            Assert.AreEqual(4,      fixture.DataByteSize);
            Assert.AreEqual(12,     fixture.TotalByteSize);
        }

        // Read non empty

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNonEmptyIffListHeader(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(30, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write Riff Chunk 1

            // DAT1 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            // Write Riff Chunk 2

            // DAT2 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x07, 0x08, 0x09, 0x0A });

            memoryStream.Position = 0;

            // -> ACT
            var fixture = GroupChunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.ChunkId);
            Assert.AreEqual("mIwF", fixture.TypeId);
            Assert.AreEqual(30,     fixture.DataByteSize);
            Assert.AreEqual(8,      fixture.HeaderByteSize);
            Assert.AreEqual(8,      fixture.DataLocation);
            Assert.AreEqual(30 + 8, fixture.TotalByteSize);

            Assert.AreEqual(2, fixture.Count);

            Assert.AreEqual("DAT1", fixture[0].ChunkId);
            Assert.AreEqual(6,      fixture[0].DataByteSize);
            Assert.AreEqual(12,     fixture[0].DataLocation);

            Assert.AreEqual("DAT2", fixture[1].ChunkId);
            Assert.AreEqual(4,      fixture[1].DataByteSize);
            Assert.AreEqual(26,     fixture[1].DataLocation);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNonAlignedNonEmptyIffListHeader(IffStandard iffStandard)
        {
                        // -> ARRANGE:
            var memoryStream = new MemoryStream();
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(28, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write Riff Chunk 1

            // DAT1 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(5, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0xff });

            // Write Riff Chunk 2

            // DAT2 written in ASCII
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(3, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x07, 0x08, 0x09, 0xff });

            memoryStream.Position = 0;

            // -> ACT
            var fixture = GroupChunk.FromStream(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual("RIFF", fixture.ChunkId);
            Assert.AreEqual("mIwF", fixture.TypeId);
            Assert.AreEqual(28,     fixture.DataByteSize);
            Assert.AreEqual(8,      fixture.HeaderByteSize);
            Assert.AreEqual(8,      fixture.DataLocation);
            Assert.AreEqual(28 + 8, fixture.TotalByteSize);

            Assert.AreEqual(2,      fixture.Count);

            Assert.AreEqual("DAT1", fixture[0].ChunkId);
            Assert.AreEqual(5,      fixture[0].DataByteSize);
            Assert.AreEqual(12,     fixture[0].DataLocation);

            Assert.AreEqual("DAT2", fixture[1].ChunkId);
            Assert.AreEqual(3,      fixture[1].DataByteSize);
            Assert.AreEqual(26,     fixture[1].DataLocation);
        }

        // Read with delegates

        //[Test, TestCaseSource(nameof(TestParams))]
        //public void CanDelegateReadNonEmptyIffListHeader(Endianness endianness)
        //{
        //     -> ARRANGE:
        //    var memoryStream = new MemoryStream();

        //     Write Iff List

        //     RIFF written in ASCII
        //    memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });

        //     Iff Size written in little Endian bytes
        //    memoryStream.Write(EndianHelpers.Int32Bytes(6 + 4 + (8 * 2) + 4, endianness));

        //     mIwF written in ASCII
        //    memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

        //     Write Riff Chunk 1

        //     DAT1 written in ASCII
        //    memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
        //    memoryStream.Write(EndianHelpers.Int32Bytes(6, endianness));
        //    memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

        //     Write Riff Chunk 2

        //     DAT2 written in ASCII
        //    memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
        //    memoryStream.Write(EndianHelpers.Int32Bytes(4, endianness));
        //    memoryStream.Write(new byte[] { 0x07, 0x08, 0x09, 0x0A });

        //    memoryStream.Position = 0;

        //     -> ACT

        //    var wasCalledForChunk1 = false;
        //    var wasCalledForChunk2 = false;
        //    var wasCalledForUnexpected = false;

        //    var streamLocationAtChunk1 = 0L;
        //    var streamLocationAtChunk2 = 0L;

        //    var callback = new Action<Chunk, Stream>((iff, stream) =>
        //    {
        //        var currentLocation = stream.Position;
        //         Change the position to something invalid just to test the rest of the file is still read correctly
        //        stream.Position = 0;

        //        if (iff.ChunkId == "DAT1")
        //        {
        //            wasCalledForChunk1 = true;
        //            streamLocationAtChunk1 = currentLocation;
        //            return;
        //        }

        //        if (iff.ChunkId == "DAT2")
        //        {
        //            wasCalledForChunk2 = true;
        //            streamLocationAtChunk2 = currentLocation;
        //            return;
        //        }

        //        wasCalledForUnexpected = true;
        //    });


        //    var fixture = InterchangeFileFormatGroupChunk.FromStream(memoryStream, endianness, callback);

        //     -> ASSERT

        //     Asset callbacks
        //    Assert.IsFalse(wasCalledForUnexpected);
        //    Assert.IsTrue(wasCalledForChunk1);
        //    Assert.IsTrue(wasCalledForChunk2);

        //    Assert.AreEqual(20, streamLocationAtChunk1);
        //    Assert.AreEqual(34, streamLocationAtChunk2);

        //     Assert the rest loaded correctly 
        //    Assert.AreEqual("RIFF", fixture.ChunkId);
        //    Assert.AreEqual("mIwF", fixture.TypeId);
        //    Assert.AreEqual(26, fixture.DataByteSize);
        //    Assert.AreEqual(12, fixture.HeaderByteSize);
        //    Assert.AreEqual(12 + 26, fixture.TotalByteSize);

        //    Assert.AreEqual(2, fixture.Count);

        //    Assert.AreEqual("DAT1", fixture[0].ChunkId);
        //    Assert.AreEqual(6, fixture[0].DataByteSize);
        //    Assert.AreEqual(20, fixture[0].DataLocation);

        //    Assert.AreEqual("DAT2", fixture[1].ChunkId);
        //    Assert.AreEqual(4, fixture[1].DataByteSize);
        //    Assert.AreEqual(34, fixture[1].DataLocation);
        //}


        // Write

        //[Test, TestCaseSource(nameof(TestParams))]
        //public void CanWriteEmptyIffHeader(Endianness endianness)
        //{
        //     -> ARRANGE:
        //     RIFF written in ASCII
        //    var expectedTypeId = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        //    var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(4, endianness); // Including the type

        //     mIwF written in ASCII
        //    var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

        //    var fixture = InterchangeFileFormatGroupChunk.Create
        //    (
        //        /* ChunkId */ "RIFF",
        //        /* TypeId */  "mIwF"
        //    );

        //     -> ACT
        //    var memoryStream = new MemoryStream();
        //    fixture.Write(memoryStream, endianness);

        //     -> ASSERT
        //    var streamPosition = memoryStream.Position;
        //    memoryStream.Position = 0;

        //     Read the written Sig bytes
        //    var typeBytes = memoryStream.Read(4);

        //    var contentByteSizeBytes = memoryStream.Read(4);
        //    var subTypeBytes = memoryStream.Read(4);

        //    Assert.AreEqual(expectedTypeId, typeBytes);
        //    Assert.AreEqual(expectedChunckSizeBytes, contentByteSizeBytes);
        //    Assert.AreEqual(expectedSubTypeBytes, subTypeBytes);
        //    Assert.AreEqual(12, streamPosition);
        //}


        //[Test, TestCaseSource(nameof(TestParams))]
        //public void CanWriteNonEmptyIffHeader(Endianness endianness)
        //{
        //     -> ARRANGE:
        //     RIFF written in ASCII
        //    var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        //    var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(36 + 4, endianness); // Including the type

        //     mIwF written in ASCII
        //    var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

        //    var fixture = InterchangeFileFormatGroupChunk.Create
        //    (
        //        /* ChunkId */ "RIFF",
        //        /* TypeId */  "mIwF",
        //        Chunk.Create
        //        (
        //            /* ChunkId */ "DAT1",
        //            /* ContentByteSize */ 9
        //        ),
        //        Chunk.Create
        //        (
        //            /* ChunkId */ "DAT2",
        //            /* ContentByteSize */ 9
        //        )
        //    );

        //     -> ACT
        //    var memoryStream = new MemoryStream();
        //    fixture.Write(memoryStream, endianness);

        //     -> ASSERT
        //    var streamPosition = memoryStream.Position;
        //    memoryStream.Position = 0;

        //     Read the written Sig bytes
        //    var typeBytes = memoryStream.Read(4);

        //    var contentByteSizeBytes = memoryStream.Read(4);
        //    var subTypeBytes = memoryStream.Read(4);

        //    Assert.AreEqual(expectedChunkIdBytes, typeBytes);
        //    Assert.AreEqual(expectedChunckSizeBytes, contentByteSizeBytes);
        //    Assert.AreEqual(expectedTypeIdBytes, subTypeBytes);

        //    Assert.AreEqual(20, fixture[0].DataLocation);
        //    Assert.AreEqual(38, fixture[1].DataLocation);
        //    Assert.AreEqual(48, streamPosition);
        //}

        //[Test, TestCaseSource(nameof(TestParams))]
        //public void CanWriteNonAlignedNonEmptyIffHeader(Endianness endianness)
        //{
        //     -> ARRANGE:
        //     RIFF written in ASCII
        //    var expectedTypeBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        //    var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(36 + 4, endianness); // Including the type

        //     mIwF written in ASCII
        //    var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

        //    var fixture = InterchangeFileFormatGroupChunk.Create
        //    (
        //        /* ChunkId */ "RIFF",
        //        /* TypeId */  "mIwF",
        //        Chunk.Create
        //        (
        //            /* ChunkId */ "DAT1",
        //            /* ContentByteSize */ 9
        //        ),
        //        Chunk.Create
        //        (
        //            /* ChunkId */ "DAT2",
        //            /* ContentByteSize */ 9
        //        )
        //    );

        //     -> ACT
        //    var memoryStream = new MemoryStream();
        //    fixture.Write(memoryStream, endianness);

        //     -> ASSERT
        //    var streamPosition = memoryStream.Position;
        //    memoryStream.Position = 0;

        //     Read the written Sig bytes
        //    var typeBytes = memoryStream.Read(4);

        //    var contentByteSizeBytes = memoryStream.Read(4);
        //    var subTypeBytes = memoryStream.Read(4);

        //    Assert.AreEqual(expectedTypeBytes, typeBytes);
        //    Assert.AreEqual(expectedChunckSizeBytes, contentByteSizeBytes);
        //    Assert.AreEqual(expectedSubTypeBytes, subTypeBytes);

        //    Assert.AreEqual(20, fixture[0].DataLocation);
        //    Assert.AreEqual(38, fixture[1].DataLocation);
        //    Assert.AreEqual(48, streamPosition);
        //}


        //[Test, TestCaseSource(nameof(TestParams))]
        //public void CanWriteNonEmptyIffHeaderUsingDel(Endianness endianness)
        //{
        //     -> ARRANGE:
        //     RIFF written in ASCII
        //    var expectedTypeBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        //    var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(70, endianness); // Including the type

        //     mIwF written in ASCII
        //    var expectedSubTypeBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

        //    var fixture = InterchangeFileFormatGroupChunk.Create
        //    (
        //        /* ChunkId */ "RIFF",
        //        /* TypeId */  "mIwF",
        //        Chunk.Create
        //        (
        //            /* ChunkId */ "DAT1",
        //            /* ContentByteSize */ 10
        //        ),
        //        Chunk.Create
        //        (
        //            /* ChunkId */ "DAT2",
        //            /* ContentByteSize */ 10
        //        )
        //    );

        //     -> ACT

        //    var wasCalledForChunk1 = false;
        //    var wasCalledForChunk2 = false;
        //    var wasCalledForUnexpected = false;

        //    var streamLocationAtChunk1 = 0L;
        //    var streamLocationAtChunk2 = 0L;

        //    var callback = new Action<Chunk, Stream>((iff, stream) =>
        //    {
        //        var currentLocation = stream.Position;
        //         Change the position to something invalid just to test the rest of the file is still read correctly
        //        stream.Position = 0;

        //        if (iff.ChunkId == "DAT1")
        //        {
        //            wasCalledForChunk1 = true;
        //            iff.DataByteSize = 20;
        //            streamLocationAtChunk1 = currentLocation;
        //            return;
        //        }

        //        if (iff.ChunkId == "DAT2")
        //        {
        //            wasCalledForChunk2 = true;
        //            iff.DataByteSize = 30;
        //            streamLocationAtChunk2 = currentLocation;
        //            return;
        //        }

        //        wasCalledForUnexpected = true;
        //    });

        //     -> ACT
        //    var memoryStream = new MemoryStream();
        //    fixture.Write(memoryStream, endianness, callback);

        //     -> ASSERT
        //    var streamPosition = memoryStream.Position;
        //    memoryStream.Position = 0;

        //     Asset callbacks
        //    Assert.IsFalse(wasCalledForUnexpected);
        //    Assert.IsTrue(wasCalledForChunk1);
        //    Assert.IsTrue(wasCalledForChunk2);

        //    Assert.AreEqual(20, streamLocationAtChunk1);
        //    Assert.AreEqual(48, streamLocationAtChunk2);

        //     Read the written Sig bytes
        //    var typeBytes = memoryStream.Read(4);

        //    var contentByteSizeBytes = memoryStream.Read(4);
        //    var subTypeBytes = memoryStream.Read(4);

        //    Assert.AreEqual(expectedTypeBytes, typeBytes);
        //    Assert.AreEqual(expectedChunckSizeBytes, contentByteSizeBytes);
        //    Assert.AreEqual(expectedSubTypeBytes, subTypeBytes);

        //    Assert.AreEqual(20, fixture[0].DataLocation);
        //    Assert.AreEqual(48, fixture[1].DataLocation);
        //    Assert.AreEqual(78, streamPosition);
        //}



        //[Test, TestCaseSource(nameof(TestParams))]
        //public void CanAppendNonEmptyChunkToEmptyIffHeader(Endianness endianness)
        //{
        //     -> ARRANGE:
        //     RIFF written in ASCII
        //    var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        //    var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(8 + 4 + 20, endianness); // Including the type

        //     mIwF written in ASCII
        //    var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

        //    var fixture = InterchangeFileFormatGroupChunk.Create
        //    (
        //        /* ChunkId */ "RIFF",
        //        /* TypeId */  "mIwF"
        //    );

        //     -> ACT

        //    var wasCalledForChunk1 = false;
        //    var wasCalledForUnexpected = false;

        //    var streamLocationAtChunk1 = 0L;

        //    var callback = new Action<Chunk, Stream>((iff, stream) =>
        //    {
        //        var currentLocation = stream.Position;
        //         Change the position to something invalid just to test the rest of the file is still read correctly
        //        stream.Position = 0;

        //        if (iff.ChunkId == "DAT1")
        //        {
        //            wasCalledForChunk1 = true;
        //            iff.DataByteSize = 20;
        //            streamLocationAtChunk1 = currentLocation;
        //            return;
        //        }

        //        wasCalledForUnexpected = true;
        //    });


        //    var memoryStream = new MemoryStream();
        //    fixture.Write(memoryStream, endianness);

        //    fixture.Append(memoryStream, endianness, "DAT1", callback);

        //     -> ASSERT
        //    var streamPosition = memoryStream.Position;
        //    memoryStream.Position = 0;

        //     Asset callbacks
        //    Assert.IsFalse(wasCalledForUnexpected);
        //    Assert.IsTrue(wasCalledForChunk1);

        //    Assert.AreEqual(20, streamLocationAtChunk1);

        //     Read the written Sig bytes
        //    var typeBytes = memoryStream.Read(4);

        //    var contentByteSizeBytes = memoryStream.Read(4);
        //    var subTypeBytes = memoryStream.Read(4);

        //    Assert.AreEqual(expectedChunkIdBytes, typeBytes);
        //    Assert.AreEqual(expectedChunckSizeBytes, contentByteSizeBytes);
        //    Assert.AreEqual(expectedTypeIdBytes, subTypeBytes);

        //    Assert.AreEqual(20, fixture[0].DataLocation);
        //    Assert.AreEqual(40, streamPosition);
        //}
    }
}
