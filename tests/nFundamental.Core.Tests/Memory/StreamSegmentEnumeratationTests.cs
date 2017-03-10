using System.IO;
using NUnit.Framework;
using Fundamental.Core.Memory;

namespace Fundamental.Core.Tests.Memory
{

    [TestFixture]
    public class StreamSegmentEnumeratationTests
    {

        [Test]
        public void CanEnumerateEmptyBuffer()
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream();
            var expetedOutput = new byte[] {  };

            // -> ACT
            var enumerator = memoryStream.Enumerate(10).GetEnumerator();

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
            var enumerator = memoryStream.Enumerate(4).GetEnumerator();

            // -> ASSERT
            Assert.AreEqual(true, enumerator.MoveNext());
            Assert.AreEqual(expetedOutput, enumerator.Current.Data);
            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(4, enumerator.Current.Length);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }

        [Test]
        public void CanEnumerateTwoWholeBuffer()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x2, 0x03, 0x04 };
            var memoryStream = new MemoryStream(inputStream);
            var expetedOutput1 = new byte[] { 0x01, 0x2 };
            var expetedOutput2 = new byte[] { 0x03, 0x04 };

            // -> ACT
            var enumerator = memoryStream.Enumerate(2).GetEnumerator();

            // -> ASSERT

            // move next
            Assert.AreEqual(true, enumerator.MoveNext());
            Assert.AreEqual(expetedOutput1, enumerator.Current.Data);
            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);

            // move next
            Assert.AreEqual(true, enumerator.MoveNext());
            Assert.AreEqual(expetedOutput2, enumerator.Current.Data);
            Assert.AreEqual(2, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }


        [Test]
        public void CanEnumerateHalfOfABuffer()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x2 };
            var memoryStream = new MemoryStream(inputStream);
            var expetedOutput1 = new byte[] { 0x01, 0x2 };

            // -> ACT
            var enumerator = memoryStream.Enumerate(4).GetEnumerator();

            // -> ASSERT

            // move next
            Assert.AreEqual(true, enumerator.MoveNext());
            Assert.AreEqual(expetedOutput1[0], enumerator.Current.Data[0]);
            Assert.AreEqual(expetedOutput1[1], enumerator.Current.Data[1]);
            Assert.AreEqual(4, enumerator.Current.Data.Length);
            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }

        [Test]
        public void CanEnumerateOneAndAHalfOfABuffer()
        {
            // -> ARRANGE:
            var inputStream = new byte[] { 0x01, 0x2, 0x03, 0x04, 0x05, 0x06 };
            var memoryStream = new MemoryStream(inputStream);

            // -> ACT
            var enumerator = memoryStream.Enumerate(4).GetEnumerator();

            // -> ASSERT

            // move next
            Assert.AreEqual(true, enumerator.MoveNext());
            Assert.AreEqual(inputStream[0], enumerator.Current.Data[0]);
            Assert.AreEqual(inputStream[1], enumerator.Current.Data[1]);
            Assert.AreEqual(inputStream[2], enumerator.Current.Data[2]);
            Assert.AreEqual(inputStream[3], enumerator.Current.Data[3]);

            Assert.AreEqual(1, enumerator.Current.Order);
            Assert.AreEqual(4, enumerator.Current.Length);

            // move next
            Assert.AreEqual(true, enumerator.MoveNext());
            Assert.AreEqual(inputStream[4], enumerator.Current.Data[0]);
            Assert.AreEqual(inputStream[5], enumerator.Current.Data[1]);
            Assert.AreEqual(2, enumerator.Current.Order);
            Assert.AreEqual(2, enumerator.Current.Length);

            // move next
            Assert.AreEqual(false, enumerator.MoveNext());
        }
    }
}
