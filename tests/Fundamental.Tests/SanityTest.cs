using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Fundamental.Tests
{
    [TestFixture]
    public class SanityTest
    {
        [Test]
        public void AmISane()
        {
            Assert.AreEqual(true, true);
        }
    }
}
