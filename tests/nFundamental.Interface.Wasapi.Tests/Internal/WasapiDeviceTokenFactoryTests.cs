using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;

namespace Fundamental.Interface.Wasapi.Tests.Internal
{
    [TestFixture]
    public class WasapiDeviceTokenFactoryTests
    {
        private WasapiDeviceTokenFactory GetTestFixture() => new WasapiDeviceTokenFactory(ImmDeviceEnumerator);

        private IMMDevice ImmDevice { get; set; }

        private IMMDeviceEnumerator ImmDeviceEnumerator { get; set; }


        [SetUp]
        public void SetUp()
        {
            ImmDevice = Substitute.For<IMMDevice>();
            ImmDeviceEnumerator = Substitute.For<IMMDeviceEnumerator>();
        }

        [Test]
        public void CanExtractTokenIdFromComObject()
        {
            // -> ARRANGE:
            var expectedId = "C67FFA3C-4A35-446E-ADB2-E39970D53C1D";
            var factory = GetTestFixture();

            // Expect the token object to resolve the id
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
            Assert.AreEqual(ImmDevice, token.MmDevice);
        }


        [Test]
        public void CanExtractTokenIdFormStringId()
        {
            // -> ARRANGE:
            var expectedId = "C67FFA3C-4A35-446E-ADB2-E39970D53C1D";
            var factory = GetTestFixture();

            // -> ACT
            var token = factory.GetToken(expectedId);

            // -> ASSERT
            Assert.AreEqual(expectedId, token.Id);
        }


        [Test]
        public void CanExtractTokenComObjectFromId()
        {
            // -> ARRANGE:
            var expectedId = "C67FFA3C-4A35-446E-ADB2-E39970D53C1D";
            var factory = GetTestFixture();

            // Expect the token object to resolve the id
            IMMDevice outImmDevice;
            ImmDeviceEnumerator.GetDevice(expectedId, out outImmDevice)
                .Returns(param =>
                {
                    param[1] = ImmDevice;
                    return HResult.S_OK;
                });

            // -> ACT
            var token = factory.GetToken(expectedId);
            var device = token.MmDevice;

            // -> ASSERT
            Assert.AreEqual(ImmDevice, device);
            Assert.AreEqual(expectedId, token.Id);
        }


    }
}
