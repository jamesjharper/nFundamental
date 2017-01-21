using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;

namespace nFundamental.Interface.Wasapi.Tests
{
    [TestFixture]
    public class WasapiDeviceTokenFactoryTests
    {

        private static WasapiDeviceTokenFactory GetTestFixture() => new WasapiDeviceTokenFactory();

        private IMMDevice ImmDevice { get; set; }


        [SetUp]
        public void SetUp()
        {
            ImmDevice = Substitute.For<IMMDevice>();
        }

        [Test]
        public void CanExtractTokenFormComObject()
        {
            // -> ARRANGE:
            var expectedId = "C67FFA3C-4A35-446E-ADB2-E39970D53C1D";
            var factory = GetTestFixture();

            string outString;
            ImmDevice.GetId(out outString)
                     .Returns(param =>
                     {
                        param[0] = expectedId;
                        return HResult.S_OK;
                     });

            // -> ACT
            var token = factory.GetToken(ImmDevice);

            // -> ASSERT
            Assert.AreEqual(expectedId, token.Id);
        }


        [Test]
        public void CanExtractTokenFormStringId()
        {
            // -> ARRANGE:
            var expectedId = "C67FFA3C-4A35-446E-ADB2-E39970D53C1D";
            var factory = GetTestFixture();

            // -> ACT
            var token = factory.GetToken(expectedId);

            // -> ASSERT
            Assert.AreEqual(expectedId, token.Id);
        }
    }
}
