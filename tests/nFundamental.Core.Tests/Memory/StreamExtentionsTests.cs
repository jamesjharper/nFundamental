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


       
        private static readonly object[] CanSwapBytesDownMiddelTestCases =
        {
            // 2 element
            // -------------------------
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02 }, 
                /* Output      */ new byte[] { 0x02, 0x01 },
                /* shift       */ 1,
                /* Buffer Size */ 1
            },

            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02 }, 
                /* Output      */ new byte[] { 0x02, 0x01 },
                /* shift       */ 1,
                /* Buffer Size */ 2
            },

            // 4 element
            // -------------------------
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x01, 0x02 },
                /* shift       */ 2,
                /* Buffer Size */ 1
            },

            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x01, 0x02 },
                /* shift       */ 2,
                /* Buffer Size */ 2
            },

            // buffer size 3
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x01, 0x02 },
                /* shift       */ 2,
                /* Buffer Size */ 3
            },

            // buffer size 4
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x01, 0x02 },
                /* shift       */ 2,
                /* Buffer Size */ 4
            },

            // buffer size 5
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x01, 0x02 },
                /* shift       */ 2,
                /* Buffer Size */ 5
            },

            // 6 element
            // -------------------------
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x04, 0x05, 0x06, 0x01, 0x02, 0x03 }, 
                /* shift       */ 3,
                /* Buffer Size */ 1
            },
            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x04, 0x05, 0x06, 0x01, 0x02, 0x03 }, 
                /* shift       */ 3,
                /* Buffer Size */ 2
            },
            // buffer size 10
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x04, 0x05, 0x06, 0x01, 0x02, 0x03 }, 
                /* shift       */ 3,
                /* Buffer Size */ 10
           },
        };

        [Test, TestCaseSource(nameof(CanSwapBytesDownMiddelTestCases))]
        public void CanSwapBytesDownMiddel(byte[] input, byte[] output, int shift, int bufferSize)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream(input);

            // -> ACT
            memoryStream.Rotate(shift, input.Length, bufferSize);

            // -> ASSERT
            Assert.AreEqual(output, input);

        }

        private static readonly object[] CanSwapBytesNonLeaningTestCases =
       {
            // 2 element
            // -------------------------
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02 }, 
                /* Output      */ new byte[] { 0x01, 0x02 },
                /* shift       */ 0,
                /* Buffer Size */ 1
            },

            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02 }, 
                /* Output      */ new byte[] { 0x01, 0x02 }, 
                /* shift       */ 0,
                /* Buffer Size */ 2
            },

            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02 }, 
                /* Output      */ new byte[] { 0x01, 0x02 }, 
                /* shift       */ 2,
                /* Buffer Size */ 2
            }
        };

        [Test, TestCaseSource(nameof(CanSwapBytesNonLeaningTestCases))]
        public void CanSwapBytesNonLeaning(byte[] input, byte[] output, int shift, int bufferSize)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream(input);

            // -> ACT
            memoryStream.Rotate(shift, input.Length, bufferSize);

            // -> ASSERT
            Assert.AreEqual(output, input);

        }


        private static readonly object[] CanSwapBytesLeftLeaningTestCases =
        {           
            // 4 element
            // -------------------------
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x04, 0x01, 0x02, 0x03 }, 
                /* shift       */ 1,
                /* Buffer Size */ 1
            },

            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x04, 0x01, 0x02, 0x03 }, 
                /* shift       */ 1,
                /* Buffer Size */ 2
            },

            // buffer size 3
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x04, 0x01, 0x02, 0x03 }, 
                /* shift       */ 1,
                /* Buffer Size */ 3
            },


            // buffer size 5
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x04, 0x01, 0x02, 0x03 }, 
                /* shift       */ 1,
                /* Buffer Size */ 5
            },

            // 6 element
            // -------------------------
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x05, 0x06, 0x01, 0x02, 0x03, 0x04 }, 
                /* shift       */ 2,
                /* Buffer Size */ 1
            },
                        // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x06, 0x01, 0x02, 0x03, 0x04, 0x05 },
                /* shift       */ 1,
                /* Buffer Size */ 1
            },
            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x05, 0x06, 0x01, 0x02, 0x03, 0x04 }, 
                /* shift       */ 2,
                /* Buffer Size */ 2
            },
            // buffer size 10
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x05, 0x06, 0x01, 0x02, 0x03, 0x04 }, 
                /* shift       */ 2,
                /* Buffer Size */ 10
           },
        };

        [Test, TestCaseSource(nameof(CanSwapBytesLeftLeaningTestCases))]
        public void CanSwapBytesLeftLeaning(byte[] input, byte[] output, int shift, int bufferSize)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream(input);

            // -> ACT
            memoryStream.Rotate(shift, input.Length, bufferSize);

            // -> ASSERT
            Assert.AreEqual(output, input);

        }

        private static readonly object[] CanSwapBytesRightLeaningTestCases =
        {            
            // 4 element
            // -------------------------
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x02, 0x03, 0x04, 0x01 }, 
                /* shift       */ 3,
                /* Buffer Size */ 1
            },

            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x02, 0x03, 0x04, 0x01 }, 
                /* shift       */ 3,
                /* Buffer Size */ 2
            },

            // buffer size 3
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x02, 0x03, 0x04, 0x01 },  
                /* shift       */ 3,
                /* Buffer Size */ 3
            },


            // buffer size 5
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04 }, 
                /* Output      */ new byte[] { 0x02, 0x03, 0x04, 0x01 }, 
                /* shift       */ 3,
                /* Buffer Size */ 5
            },

            // 6 element
            // -------------------------
            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x05, 0x06, 0x01, 0x02 }, 
                /* shift       */ 4,
                /* Buffer Size */ 1
            },
            // buffer size 1
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x02, 0x03, 0x04, 0x05, 0x06, 0x01  },  
                /* shift       */ 5,
                /* Buffer Size */ 1
            },
            // buffer size 2
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x05, 0x06, 0x01, 0x02 }, 
                /* shift       */ 4,
                /* Buffer Size */ 2
            },
            // buffer size 10
            new object[]
            {
                /* Input       */ new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, 
                /* Output      */ new byte[] { 0x03, 0x04, 0x05, 0x06, 0x01, 0x02 }, 
                /* shift       */ 4,
                /* Buffer Size */ 10
           },
        };

        [Test, TestCaseSource(nameof(CanSwapBytesRightLeaningTestCases))]
        public void CanSwapBytesRightLeaning(byte[] input, byte[] output, int shift, int bufferSize)
        {
            // -> ARRANGE:
            var memoryStream = new MemoryStream(input);

            // -> ACT
            memoryStream.Rotate(shift, input.Length, bufferSize);

            // -> ASSERT
            Assert.AreEqual(output, input);

        }
    }
}
