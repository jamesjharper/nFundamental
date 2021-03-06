﻿using System;
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
        #region Test Fixtures

        public class TestDataChunk : ValueChunk
        {
            public byte[] Data { get; set; }

            protected override byte[] GetValueBytes()
            {
                return Data;
            }

            protected override void ReadValueBytes(byte[] data)
            {
                Data = data;
            }
        }

        public class TestGroupChunk : GroupChunk
        {
            protected override Chunk ParseLocalChunk(Chunk streamChunk)
            {
                if (streamChunk.ChunkId.StartsWith("SUB"))
                    return streamChunk.As<GroupChunk>();

                if (streamChunk.ChunkId.StartsWith("TST"))
                    return streamChunk.As<TestDataChunk>();
                return streamChunk;
            }
        }

        #endregion

        private static readonly object[] Standard32Bit = IffStandard.Standard32Bit;

        #region Read

        // Read Empty

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadEmpty(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Group Header
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
        }

        // Read non empty

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNonEmpty(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(30, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            // Write DAT2 sub chunk
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

            Assert.AreEqual(2, fixture.Count);

            Assert.AreEqual("DAT1", fixture[0].ChunkId);
            Assert.AreEqual(6,      fixture[0].DataByteSize);
            Assert.AreEqual(12,     fixture[0].DataLocation);

            Assert.AreEqual("DAT2", fixture[1].ChunkId);
            Assert.AreEqual(4,      fixture[1].DataByteSize);
            Assert.AreEqual(26,     fixture[1].DataLocation);
        }

        // Read non aligned

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNonAlignedNonEmpty(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(28, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT12 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(5, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0xff });
            
            // Write DAT2 sub chunk
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

            Assert.AreEqual(2,      fixture.Count);

            Assert.AreEqual("DAT1", fixture[0].ChunkId);
            Assert.AreEqual(5,      fixture[0].DataByteSize);
            Assert.AreEqual(12,     fixture[0].DataLocation);

            Assert.AreEqual("DAT2", fixture[1].ChunkId);
            Assert.AreEqual(3,      fixture[1].DataByteSize);
            Assert.AreEqual(26,     fixture[1].DataLocation);
        }

        // Read sub chunk

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadAllLocalChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            var expectedChunk2Bytes = new byte[] { 0x07, 0x08, 0x09, 0x0A };

            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(30, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(expectedChunk1Bytes);

            // Write DAT2 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedChunk2Bytes);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = GroupChunk.FromStream(memoryStream, iffStandard);

            var chunk1Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            var chunk2Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff };

            var chunk1BytesSize = fixture[0].Read(chunk1Bytes, 0 , chunk1Bytes.Length);
            var chunk2BytesSize = fixture[1].Read(chunk2Bytes, 0, chunk2Bytes.Length);

            // -> ASSERT
            Assert.AreEqual(expectedChunk1Bytes, chunk1Bytes);
            Assert.AreEqual(expectedChunk2Bytes, chunk2Bytes);

            Assert.AreEqual(6, chunk1BytesSize);
            Assert.AreEqual(4, chunk2BytesSize);

            Assert.AreEqual(6, fixture[0].Position);
            Assert.AreEqual(4, fixture[1].Position);

        }

        // Read past the end of sub chunk

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanOverreadReadLocalChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0xff, 0xff };
            var expectedChunk2Bytes = new byte[] { 0x07, 0x08, 0x09, 0x0A, 0xff, 0xff, 0xff, 0xff };

            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(30, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(expectedChunk1Bytes, 0, 6);

            // Write DAT2 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedChunk2Bytes, 0, 4);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = GroupChunk.FromStream(memoryStream, iffStandard);

            var chunk1Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            var chunk2Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };

            var chunk1BytesSize = fixture[0].Read(chunk1Bytes, 0, chunk1Bytes.Length);
            var chunk2BytesSize = fixture[1].Read(chunk2Bytes, 0, chunk2Bytes.Length);

            // -> ASSERT
            Assert.AreEqual(expectedChunk1Bytes, chunk1Bytes);
            Assert.AreEqual(expectedChunk2Bytes, chunk2Bytes); 
            
            Assert.AreEqual(6, chunk1BytesSize);
            Assert.AreEqual(4, chunk2BytesSize);

            Assert.AreEqual(6, fixture[0].Position);
            Assert.AreEqual(4, fixture[1].Position);
        }

        // Read portion of sub chunk

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanUnderReadLocalChunkContent(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            var expectedChunk2Bytes = new byte[] { 0x07, 0x08, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };

            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(30, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            // Write DAT2 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x07, 0x08, 0x09, 0x0A });

            memoryStream.Position = 0;

            // -> ACT
            var fixture = GroupChunk.FromStream(memoryStream, iffStandard);

            var chunk1Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            var chunk2Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };

            var chunk1BytesSize = fixture[0].Read(chunk1Bytes, 0, 2);
            var chunk2BytesSize = fixture[1].Read(chunk2Bytes, 0, 2);

            // -> ASSERT
            Assert.AreEqual(expectedChunk1Bytes, chunk1Bytes);
            Assert.AreEqual(expectedChunk2Bytes, chunk2Bytes);

            Assert.AreEqual(2, chunk1BytesSize);
            Assert.AreEqual(2, chunk2BytesSize);

            Assert.AreEqual(2, fixture[0].Position);
            Assert.AreEqual(2, fixture[1].Position);
        }

        // Read from position of sub chunk 

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadLocalChunkContentFromPosition(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunk1Bytes = new byte[] { 0x03, 0x04, 0x05, 0x06, 0xff, 0xff, 0xff, 0xff };
            var expectedChunk2Bytes = new byte[] { 0x09, 0x0A, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };

            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(30, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            // Write DAT2 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x07, 0x08, 0x09, 0x0A });

            memoryStream.Position = 0;

            // -> ACT
            var fixture = GroupChunk.FromStream(memoryStream, iffStandard);

            var chunk1Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            var chunk2Bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };

            fixture[0].Position = 2;
            fixture[1].Position = 2;

            var chunk1BytesSize = fixture[0].Read(chunk1Bytes, 0, chunk1Bytes.Length);
            var chunk2BytesSize = fixture[1].Read(chunk2Bytes, 0, chunk2Bytes.Length);

            // -> ASSERT
            Assert.AreEqual(expectedChunk1Bytes, chunk1Bytes);
            Assert.AreEqual(expectedChunk2Bytes, chunk2Bytes);

            Assert.AreEqual(4, chunk1BytesSize);
            Assert.AreEqual(2, chunk2BytesSize);

            Assert.AreEqual(6, fixture[0].Position);
            Assert.AreEqual(4, fixture[1].Position);
        }

        // Read nested sub chunk

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNestedLocalChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedSubChunkData = new byte[] {0x07, 0x08, 0x09, 0x0A};

            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(30, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            // Write TST1 sub group chunk
            memoryStream.Write(new byte[] { 0x54, 0x53, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedSubChunkData);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestGroupChunk>(memoryStream, iffStandard);

            // -> ASSERT
            Assert.AreEqual(expectedSubChunkData, fixture.At<TestDataChunk>(1).RawBytes);
        }

        // Read nested sub chunk group

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadNestedGroupChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedSubChunkData = new byte[] { 0x07, 0x08, 0x09, 0x0A };

            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(42, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            // Write SUB1 sub group chunk
            memoryStream.Write(new byte[] { 0x53, 0x55, 0x42, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(16, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6e, 0x53, 0x77, 0x6f });

            // Write SUB1.DAT2 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedSubChunkData);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestGroupChunk>(memoryStream, iffStandard);

            var chunkBytes = new byte[] { 0xff, 0xff, 0xff, 0xff };
            fixture.At<GroupChunk>(1)[0].Read(chunkBytes, 0, chunkBytes.Length);

            // -> ASSERT
            Assert.AreEqual(expectedSubChunkData, chunkBytes);
        }


        // Read subsequent chucks after nested sub chunk group

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanReadSubsequentChucksAfterNestedGroupChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedSubChunkData = new byte[] { 0x07, 0x08, 0x09, 0x0A };

            var memoryStream = new MemoryStream();

            // Write Group Header
            memoryStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
            memoryStream.Write(EndianHelpers.Int32Bytes(54, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6d, 0x49, 0x77, 0x46 });

            // Write DAT1 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(6, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            // Write SUB1 sub group chunk
            memoryStream.Write(new byte[] { 0x53, 0x55, 0x42, 0x31 });
            memoryStream.Write(EndianHelpers.Int32Bytes(16, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6e, 0x53, 0x77, 0x6f });

            // Write SUB1.DAT2 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x32 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(new byte[] { 0x6e, 0x53, 0x77, 0x6f });

            // Write DAT3 sub chunk
            memoryStream.Write(new byte[] { 0x44, 0x41, 0x54, 0x33 });
            memoryStream.Write(EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder));
            memoryStream.Write(expectedSubChunkData);

            memoryStream.Position = 0;

            // -> ACT
            var fixture = Chunk.FromStream<TestGroupChunk>(memoryStream, iffStandard);

            var chunkBytes = new byte[] { 0xff, 0xff, 0xff, 0xff };
            fixture[2].Read(chunkBytes, 0, chunkBytes.Length);

            // -> ASSERT
            Assert.AreEqual(expectedSubChunkData, chunkBytes);
        }

        #endregion

        #region Write 

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanWriteEmpty(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkId = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder); // Including the type
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            //  -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
               (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
               );

            // Write to stream
            fixture.Flush();

            //  -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written Sig bytes
            var chunkIdBytes = memoryStream.Read(4);
            var chunkSizeBytes = memoryStream.Read(4);
            var typeIdBytes = memoryStream.Read(4);

            Assert.AreEqual(expectedChunkId, chunkIdBytes);
            Assert.AreEqual(expectedChunckSizeBytes, chunkSizeBytes);
            Assert.AreEqual(expectedTypeIdBytes, typeIdBytes);
            Assert.AreEqual(12, streamPosition);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanWriteNonEmpty(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(28, iffStandard.ByteOrder);
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var expectedChunk1Id = new byte[] { 0x54, 0x53, 0x54, 0x31 };
            var expectedChunk1SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var expectedChunk2Id = new byte[] { 0x54, 0x53, 0x54, 0x32 };
            var expectedChunk2SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk2Bytes = new byte[] { 0x05, 0x06, 0x07, 0x08 };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
            (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
            );

            var dat1 = fixture.Add<TestDataChunk>("TST1");
            dat1.Data = expectedChunk1Bytes;

            var dat2 = fixture.Add<TestDataChunk>("TST2");
            dat2.Data = expectedChunk2Bytes;

            // Write to stream
            fixture.Flush();

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written bytes
            // Check group chunk
            Assert.AreEqual(expectedChunkIdBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunckSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTypeIdBytes, memoryStream.Read(4));

            // Check tst1 chunk
            Assert.AreEqual(expectedChunk1Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1Bytes, memoryStream.Read(4));

            // Check tst2 chunk
            Assert.AreEqual(expectedChunk2Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2Bytes, memoryStream.Read(4));

            Assert.AreEqual(20, fixture[0].DataLocation);
            Assert.AreEqual(32, fixture[1].DataLocation);
            Assert.AreEqual(36, streamPosition);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanWriteNestedChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(40, iffStandard.ByteOrder);
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var expectedTst1ChunkId = new byte[] { 0x54, 0x53, 0x54, 0x31 };
            var expectedTst1ChunkSizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedTst1ChunkBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var expectedSub1ChunkId = new byte[] { 0x53, 0x55, 0x42, 0x31 };
            var expectedSub1ChunkSizeBytes = EndianHelpers.Int32Bytes(16, iffStandard.ByteOrder);
            var expectedSub1ChunkTypeId = new byte[] { 0x4e, 0x45, 0x53, 0x54 };

            var expectedSub1Tst1ChunkId = new byte[] { 0x54, 0x53, 0x54, 0x32 };
            var expectedSub1Tst1ChunkSizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedSub1Tst1ChunkBytes = new byte[] { 0x05, 0x06, 0x07, 0x08 };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
            (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
            );

            var dat1 = fixture.Add<TestDataChunk>("TST1");
            dat1.Data = expectedTst1ChunkBytes;

            var sub1 = fixture.AddGroup<TestGroupChunk>("SUB1", "NEST");

            var sub1Dat1 = sub1.Add<TestDataChunk>("TST2");
            sub1Dat1.Data = expectedSub1Tst1ChunkBytes;

            // Write to stream
            fixture.Flush();

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written bytes
            // Check group chunk
            Assert.AreEqual(expectedChunkIdBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunckSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTypeIdBytes, memoryStream.Read(4));

            // Check tst1 chunk
            Assert.AreEqual(expectedTst1ChunkId, memoryStream.Read(4));
            Assert.AreEqual(expectedTst1ChunkSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTst1ChunkBytes, memoryStream.Read(4));

            // Check sub1 chunk
            Assert.AreEqual(expectedSub1ChunkId, memoryStream.Read(4));
            Assert.AreEqual(expectedSub1ChunkSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedSub1ChunkTypeId, memoryStream.Read(4));

            // Check sub1.tst1 chunk
            Assert.AreEqual(expectedSub1Tst1ChunkId, memoryStream.Read(4));
            Assert.AreEqual(expectedSub1Tst1ChunkSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedSub1Tst1ChunkBytes, memoryStream.Read(4));

            Assert.AreEqual(20, fixture[0].DataLocation);
            Assert.AreEqual(32, fixture[1].DataLocation);
            Assert.AreEqual(48, streamPosition);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanWriteNonAlignedNonEmpty(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(32, iffStandard.ByteOrder);
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var expectedChunk1Id = new byte[] { 0x54, 0x53, 0x54, 0x31 };
            var expectedChunk1SizeBytes = EndianHelpers.Int32Bytes(5, iffStandard.ByteOrder);
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

            var expectedChunk2Id = new byte[] { 0x54, 0x53, 0x54, 0x32 };
            var expectedChunk2SizeBytes = EndianHelpers.Int32Bytes(5, iffStandard.ByteOrder);
            var expectedChunk2Bytes = new byte[] { 0x06, 0x07, 0x08, 0x09, 0x0a };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
            (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
            );

            var dat1 = fixture.Add<TestDataChunk>("TST1");
            dat1.Data = expectedChunk1Bytes;

            var dat2 = fixture.Add<TestDataChunk>("TST2");
            dat2.Data = expectedChunk2Bytes;

            // Write to stream
            fixture.Flush();

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written bytes
            // Check group chunk
            Assert.AreEqual(expectedChunkIdBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunckSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTypeIdBytes, memoryStream.Read(4));

            // Check tst1 chunk
            Assert.AreEqual(expectedChunk1Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1Bytes, memoryStream.Read(5));
            memoryStream.Read(1); // Skip padding byte

            // Check tst2 chunk
            Assert.AreEqual(expectedChunk2Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2Bytes, memoryStream.Read(5));
            memoryStream.Read(1); // Skip padding byte

            Assert.AreEqual(20, fixture[0].DataLocation);
            Assert.AreEqual(34, fixture[1].DataLocation);
            Assert.AreEqual(40, streamPosition);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanAppendNonEmptyChunkToEmpty(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(28, iffStandard.ByteOrder);
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var expectedChunk1Id = new byte[] { 0x54, 0x53, 0x54, 0x31 };
            var expectedChunk1SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var expectedChunk2Id = new byte[] { 0x54, 0x53, 0x54, 0x32 };
            var expectedChunk2SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk2Bytes = new byte[] { 0x05, 0x06, 0x07, 0x08 };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
            (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
            );

            var dat1 = fixture.Add<TestDataChunk>("TST1");
            dat1.Data = expectedChunk1Bytes;

            // Write to stream
            fixture.Flush();

            var dat2 = fixture.Add<TestDataChunk>("TST2");
            dat2.Data = expectedChunk2Bytes;
            fixture.Flush();

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written bytes
            // Check group chunk
            Assert.AreEqual(expectedChunkIdBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunckSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTypeIdBytes, memoryStream.Read(4));

            // Check tst1 chunk
            Assert.AreEqual(expectedChunk1Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1Bytes, memoryStream.Read(4));

            // Check tst2 chunk
            Assert.AreEqual(expectedChunk2Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2Bytes, memoryStream.Read(4));

            Assert.AreEqual(20, fixture[0].DataLocation);
            Assert.AreEqual(32, fixture[1].DataLocation);
            Assert.AreEqual(36, streamPosition);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanAppendBytesToChunk(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(28, iffStandard.ByteOrder);
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var expectedChunk1Id = new byte[] { 0x54, 0x53, 0x54, 0x31 };
            var expectedChunk1SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var expectedChunk2Id = new byte[] { 0x54, 0x53, 0x54, 0x32 };
            var expectedChunk2SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk2Bytes = new byte[] { 0x05, 0x06, 0x07, 0x08 };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
            (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
            );

            var dat1 = fixture.Add<TestDataChunk>("TST1");
            dat1.Data = expectedChunk1Bytes;
            var dat2 = fixture.Add<TestDataChunk>("TST2");
            dat2.Data = new byte[] { 0xff, 0xff }; 

            // Write to stream
            fixture.Flush();

            // Append bytes
            dat2.Data = expectedChunk2Bytes;
            dat2.FlagHeaderForFlush();
            fixture.Flush();

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written bytes
            // Check group chunk
            Assert.AreEqual(expectedChunkIdBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunckSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTypeIdBytes, memoryStream.Read(4));

            // Check tst1 chunk
            Assert.AreEqual(expectedChunk1Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1Bytes, memoryStream.Read(4));

            // Check tst2 chunk
            Assert.AreEqual(expectedChunk2Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2Bytes, memoryStream.Read(4));

            Assert.AreEqual(20, fixture[0].DataLocation);
            Assert.AreEqual(32, fixture[1].DataLocation);
            Assert.AreEqual(36, streamPosition);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanAppendBytesToChunkStream(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(28, iffStandard.ByteOrder);
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var expectedChunk1Id = new byte[] { 0x54, 0x53, 0x54, 0x31 };
            var expectedChunk1SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var expectedChunk2Id = new byte[] { 0x44, 0x41, 0x54, 0x32 };
            var expectedChunk2SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk2Bytes = new byte[] { 0x05, 0x06, 0x07, 0x08 };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
            (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
            );

            var dat1 = fixture.Add<TestDataChunk>("TST1");
            dat1.Data = expectedChunk1Bytes;
            var dat2 = fixture.Add<Chunk>("DAT2");

            // Write to stream
            fixture.Flush();

            // Append bytes
            dat2.Write(expectedChunk2Bytes);
            fixture.Flush();

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written bytes
            // Check group chunk
            Assert.AreEqual(expectedChunkIdBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunckSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTypeIdBytes, memoryStream.Read(4));

            // Check tst1 chunk
            Assert.AreEqual(expectedChunk1Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1Bytes, memoryStream.Read(4));

            // Check tst2 chunk
            Assert.AreEqual(expectedChunk2Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2Bytes, memoryStream.Read(4));

            Assert.AreEqual(20, fixture[0].DataLocation);
            Assert.AreEqual(32, fixture[1].DataLocation);
            Assert.AreEqual(36, streamPosition);
        }

        [Test, TestCaseSource(nameof(Standard32Bit))]
        public void CanOverwriteBytesToChunkStream(IffStandard iffStandard)
        {
            // -> ARRANGE:
            var expectedChunkIdBytes = new byte[] { 0x52, 0x49, 0x46, 0x46 };
            var expectedChunckSizeBytes = EndianHelpers.Int32Bytes(28, iffStandard.ByteOrder);
            var expectedTypeIdBytes = new byte[] { 0x6d, 0x49, 0x77, 0x46 };

            var expectedChunk1Id = new byte[] { 0x54, 0x53, 0x54, 0x31 };
            var expectedChunk1SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk1Bytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var expectedChunk2Id = new byte[] { 0x44, 0x41, 0x54, 0x32 };
            var expectedChunk2SizeBytes = EndianHelpers.Int32Bytes(4, iffStandard.ByteOrder);
            var expectedChunk2Bytes = new byte[] { 0x05, 0x06, 0x07, 0x08 };

            // -> ACT
            var memoryStream = new MemoryStream();
            var fixture = GroupChunk.Create
            (
                   /* ChunkId  */ "RIFF",
                   /* TypeId   */ "mIwF",
                   /* Stream   */ memoryStream,
                   /* standard */ iffStandard
            );

            var dat1 = fixture.Add<TestDataChunk>("TST1");
            dat1.Data = expectedChunk1Bytes;
            var dat2 = fixture.Add<Chunk>("DAT2");
            dat2.Write(new byte[] { 0xff, 0xff, 0xff, 0xff });
            dat2.Position = 0;
            
            // Write to stream
            fixture.Flush();

            // Append bytes
            dat2.Write(expectedChunk2Bytes);
            fixture.Flush();

            // -> ASSERT
            var streamPosition = memoryStream.Position;
            memoryStream.Position = 0;

            // Read the written bytes
            // Check group chunk
            Assert.AreEqual(expectedChunkIdBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunckSizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedTypeIdBytes, memoryStream.Read(4));

            // Check tst1 chunk
            Assert.AreEqual(expectedChunk1Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk1Bytes, memoryStream.Read(4));

            // Check tst2 chunk
            Assert.AreEqual(expectedChunk2Id, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2SizeBytes, memoryStream.Read(4));
            Assert.AreEqual(expectedChunk2Bytes, memoryStream.Read(4));

            Assert.AreEqual(20, fixture[0].DataLocation);
            Assert.AreEqual(32, fixture[1].DataLocation);
            Assert.AreEqual(36, streamPosition);
        }

        #endregion
    }
}
