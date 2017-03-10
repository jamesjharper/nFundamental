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
    public class StreamExtentionsTests
    {

        private static readonly object[] BufferSizes = 
        {
            1
        };

        [Test, TestCaseSource(nameof(BufferSizes))]
        public void CanSwapBytesDownMiddel(int bufferSize)
        {
            // -> ARRANGE:
            var inputStream = new byte[] {0x01, 0x2, 0x03, 0x04};
            var memoryStream = new MemoryStream(inputStream);
            var expetedOutput = new byte[] { 0x03, 0x4, 0x01, 0x02 };

            // -> ACT
            memoryStream.Swap(0, 2, 2, bufferSize);

            // -> ASSERT
            Assert.AreEqual(expetedOutput, inputStream);

        }

        //[Test, TestCaseSource(nameof(BufferSizes))]
        //public void CanSwapBytesLeftLeaning(int bufferSize)
        //{
        //    // -> ARRANGE:
        //    var inputStream = new byte[] { 0x01, 0x2, 0x03, 0x04 };
        //    var memoryStream = new MemoryStream(inputStream);
        //    var expetedOutput = new byte[] { 0x04, 0x1, 0x02, 0x03 };

        //    // -> ACT
        //    memoryStream.Swap(0, 1, 3, bufferSize);

        //    // -> ASSERT
        //    Assert.AreEqual(expetedOutput, inputStream);
        //}
    }
}
