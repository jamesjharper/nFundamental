using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Core.Memory;
using NUnit.Framework;

namespace Fundamental.Core.Tests.Memory
{
    [TestFixture]
    public class CursorStreamSegmentTests
    {

        [Test]
        public void CanEnumerateEmptyBuffer()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            var expetedOutput = new byte[] { };

            // -> ACT
            var enumerator = memoryStream.CursoredEnumerate(10).GetEnumerator();

            // -> ASSERT
            Assert.AreEqual(false, enumerator.MoveNext());

        }

        [Test]
        public void CanEnumerateOneWholeBuffer()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x2, 0x03, 0x04 };
            var memoryStream = new MemoryStream(inputStream);
            var expetedOutput = new byte[] { 0x01, 0x2, 0x03, 0x04 };

            // -> ACT
            var enumerator = memoryStream.CursoredEnumerate(4).GetEnumerator();

            // -> ASSERT
            Assert.AreEqual(true, enumerator.MoveNext());

            var segment = enumerator.Current.Read();
            Assert.AreEqual(expetedOutput, segment.Data);
            Assert.AreEqual(4, segment.Length);
            Assert.AreEqual(1, segment.Order);
            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(4, enumerator.Current.Length);
            Assert.AreEqual(0, enumerator.Current.Position);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }

        [Test]
        public void CanEnumerateTwoWholeBuffer()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x2, 0x03, 0x04 };
            var memoryStream = new MemoryStream(inputStream);

            // -> ACT
            var enumerator = memoryStream.CursoredEnumerate(2).GetEnumerator();

            // -> ASSERT

            // Read first segment
            Assert.AreEqual(true, enumerator.MoveNext());

            var segment1 = enumerator.Current.Read();
            Assert.AreEqual(inputStream[0], segment1.Data[0]);
            Assert.AreEqual(inputStream[1], segment1.Data[1]);

            Assert.AreEqual(2, segment1.Length);
            Assert.AreEqual(1, segment1.Order);
            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);
            Assert.AreEqual(0, enumerator.Current.Position);

            // Read Second segment
            Assert.AreEqual(true, enumerator.MoveNext());

            var segment2 = enumerator.Current.Read();
            Assert.AreEqual(inputStream[2], segment2.Data[0]);
            Assert.AreEqual(inputStream[3], segment2.Data[1]);

            Assert.AreEqual(2, segment2.Length);
            Assert.AreEqual(2, segment2.Order);
            Assert.AreEqual(2, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);
            Assert.AreEqual(2, enumerator.Current.Position);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }

        [Test]
        public void CanEnumerateHalfOfABuffer()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x2  };
            var memoryStream = new MemoryStream(inputStream);

            // -> ACT
            var enumerator = memoryStream.CursoredEnumerate(4).GetEnumerator();

            // -> ASSERT

            // Read first segment
            Assert.AreEqual(true, enumerator.MoveNext());

            var segment1 = enumerator.Current.Read();
            Assert.AreEqual(inputStream[0], segment1.Data[0]);
            Assert.AreEqual(inputStream[1], segment1.Data[1]);

            Assert.AreEqual(2, segment1.Length);
            Assert.AreEqual(1, segment1.Order);
            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);
            Assert.AreEqual(0, enumerator.Current.Position);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }

        [Test]
        public void CanEnumerateOneAndAHalfOfABufferBuffer()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x2, 0x03, 0x04, 0x05, 0x06 };
            var memoryStream = new MemoryStream(inputStream);

            // -> ACT
            var enumerator = memoryStream.CursoredEnumerate(4).GetEnumerator();

            // -> ASSERT

            // Read first segment
            Assert.AreEqual(true, enumerator.MoveNext());

            var segment1 = enumerator.Current.Read();
            Assert.AreEqual(inputStream[0], segment1.Data[0]);
            Assert.AreEqual(inputStream[1], segment1.Data[1]);
            Assert.AreEqual(inputStream[2], segment1.Data[2]);
            Assert.AreEqual(inputStream[3], segment1.Data[3]);

            Assert.AreEqual(4, segment1.Length);
            Assert.AreEqual(1, segment1.Order);
            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(4, enumerator.Current.Length);
            Assert.AreEqual(0, enumerator.Current.Position);

            // Read Second segment
            Assert.AreEqual(true, enumerator.MoveNext());

            var segment2 = enumerator.Current.Read();
            Assert.AreEqual(inputStream[4], segment2.Data[0]);
            Assert.AreEqual(inputStream[5], segment2.Data[1]);

            Assert.AreEqual(2, segment2.Length);
            Assert.AreEqual(2, segment2.Order);
            Assert.AreEqual(2, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);
            Assert.AreEqual(4, enumerator.Current.Position);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }

        [Test]
        public void CanEnumerateOutofOrder()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            var memoryStream = new MemoryStream(inputStream);

            var expectedSeg1 = new byte[] { 0x01, 0x02 };
            var expectedSeg2 = new byte[] { 0x03, 0x04 };
            var expectedSeg3 = new byte[] { 0x05, 0x06 };
            var expectedSeg4 = new byte[] { 0x07, 0x08 };

            // -> ACT
            var items = memoryStream.CursoredEnumerate(2).ToArray();

            // -> ASSERT
            Assert.AreEqual(4, items.Length);

            // read segment 3
            var seg3 = items[2].Read();
            Assert.AreEqual(expectedSeg3, seg3.Data);
            Assert.AreEqual(2, seg3.Length);
            Assert.AreEqual(3, seg3.Order);

            // read segment 1
            var seg1 = items[0].Read();
            Assert.AreEqual(expectedSeg1, seg1.Data);
            Assert.AreEqual(2, seg1.Length);
            Assert.AreEqual(1, seg1.Order);

            // read segment 2
            var seg2 = items[1].Read();
            Assert.AreEqual(expectedSeg2, seg2.Data);
            Assert.AreEqual(2, seg2.Length);
            Assert.AreEqual(2, seg2.Order);

            // read segment 4
            var seg4 = items[3].Read();
            Assert.AreEqual(expectedSeg4, seg4.Data);
            Assert.AreEqual(2, seg4.Length);
            Assert.AreEqual(4, seg4.Order);
        }

        [Test]
        public void CanWriteOverSegment()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            var memoryStream = new MemoryStream(inputStream);
            var expectedStream = new byte[] { 0x01, 0x02, 0x03, 0x04, 0xaa, 0xbb, 0x07, 0x08 };

            // -> ACT
            var items = memoryStream.CursoredEnumerate(2).Take(3).ToArray();
            items[2].Write(new byte[] { 0xaa, 0xbb });

            // -> ASSERT
            Assert.AreEqual(expectedStream, inputStream);
        }

        [Test]
        public void CanSwapSegment()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            var memoryStream = new MemoryStream(inputStream);
            var expectedStream = new byte[] { 0x01, 0x02, 0x05, 0x06, 0x03, 0x04, 0x07, 0x08 };

            // -> ACT
            var items = memoryStream.CursoredEnumerate(2).ToArray();
            CursorStreamSegment.Swap(items[1], items[2]);

            // -> ASSERT
            Assert.AreEqual(expectedStream, inputStream);
        }
    }
}
