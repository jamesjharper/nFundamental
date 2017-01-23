using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using NSubstitute;
using NUnit.Framework;

namespace nFundamental.Interface.Wasapi.Tests
{
    [TestFixture]
    public class WasapiDevicePropertyBagTests
    {

        public IWasapiPropertyNameTranslator WasapiPropertyNameTranslator { get; set; }

        public IPropertyStore PropertyStoreTestFixture { get; set; }


        [SetUp]
        public void SetUp()
        {
            WasapiPropertyNameTranslator = Substitute.For<IWasapiPropertyNameTranslator>();
            PropertyStoreTestFixture = Substitute.For<IPropertyStore>();
        }

        private WasapiDevicePropertyBag GetTestFixture()
            => new WasapiDevicePropertyBag(PropertyStoreTestFixture, WasapiPropertyNameTranslator);


    }
}
