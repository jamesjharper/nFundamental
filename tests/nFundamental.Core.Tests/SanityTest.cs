using NUnit.Framework;

namespace Fundamental.Core.Tests
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
