using System;
using System.Collections.Generic;
using System.Linq;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using Fundamental.Interface.Wasapi;
using NSubstitute;
using NUnit.Framework;

namespace Fundamental.Interface.Wasapi.Tests
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


        // Handling unknown property names

        [Test]
        public void CanNotTryGetUnknownProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = PropertyKey.NullKey;
        
            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // -> ACT
            object resultObject;
            var couldNotGetValue = !fixture.TryGetValue("key3", out resultObject);

            // -> ASSERT
            Assert.Null(resultObject);
            Assert.That(couldNotGetValue);
        }

        [Test]
        public void CanNotGetUnknownProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = PropertyKey.NullKey;

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // -> ACT
            var resultObject = fixture["key3"];

            // -> ASSERT
            Assert.Null(resultObject);
        }

        [Test]
        public void CanNotContainUnknownProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = PropertyKey.NullKey;

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // -> ACT
            var doesNotContain = !fixture.ContainsKey("key3");

            // -> ASSERT
            Assert.That(doesNotContain);
        }

        // Handling missing properties

        [Test]
        public void CanNotTryGetMissingProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // Expect that we call the underlying property store with the returned key
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.Empty;
                    return HResult.S_OK;
                });

            // -> ACT
            object resultObject;
            var couldNotGetValue = !fixture.TryGetValue("key3", out resultObject);

            // -> ASSERT
            Assert.Null(resultObject);
            Assert.That(couldNotGetValue);
        }

        [Test]
        public void CanNotGetMissingProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // Expect that we call the underlying property store with the returned key
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.Empty;
                    return HResult.S_OK;
                });

            // -> ACT
            var resultObject = fixture["key3"];

            // -> ASSERT
            Assert.Null(resultObject);
        }

        [Test]
        public void DoesNotContainMissingProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // Expect that we call the underlying property store with the returned key
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.Empty;
                    return HResult.S_OK;
                });

            // -> ACT
            var doesNotContain = !fixture.ContainsKey("key3");

            // -> ASSERT
            Assert.That(doesNotContain);
        }

        // Handling valid properties

        [Test]
        public void CanTryGetValidProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2); 

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // Expect that we call the underlying property store with the returned key
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(1); // valid data
                    return HResult.S_OK;
                });

            // -> ACT
            object resultObject;
            var couldGetValue = fixture.TryGetValue("key3", out resultObject);

            // -> ASSERT
            Assert.That(couldGetValue);
            Assert.AreEqual(1, resultObject);
        }


        [Test]
        public void CanGetValidProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2); 

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // Expect that we call the underlying property store with the returned key
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data
                    return HResult.S_OK;
                });

            // -> ACT
            var resultObject = fixture["key3"];

            // -> ASSERT
            Assert.AreEqual(234, resultObject);
        }


        [Test]
        public void ContainsValidProperty()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2); 

            // Expect a call to the property name translator to get the WASAPI property key
            WasapiPropertyNameTranslator
                .ResolvePropertyKey("key3")
                .Returns(returnedPropertyKey);


            // Expect that we call the underlying property store with the returned key
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data
                    return HResult.S_OK;
                });

            // -> ACT
            var containsKey = fixture.ContainsKey("key3");

            // -> ASSERT
            Assert.That(containsKey);
        }

        // Empty Enumeration

        [Test]
        public void CanGetEmptyListOfValues()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                 .Returns((param) =>
                 {
                     param[0] = 0; // count of zero
                     return HResult.S_OK;
                 });


            // -> ACT
            var valueList = fixture.Values.ToList();

            // -> ASSERT
            Assert.IsEmpty(valueList);
        }

        [Test]
        public void CanGetEmptyListOfKeys()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                 .Returns((param) =>
                 {
                     param[0] = 0; // count of zero
                     return HResult.S_OK;
                 });


            // -> ACT
            var valueList = fixture.Keys.ToList();

            // -> ASSERT
            Assert.IsEmpty(valueList);
        }

        [Test]
        public void CanGetEmptyListOfKeyValuePairs()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                 .Returns((param) =>
                 {
                     param[0] = 0; // count of zero
                     return HResult.S_OK;
                 });


            // -> ACT
            var valueList = fixture.ToList();

            // -> ASSERT
            Assert.IsEmpty(valueList);
        }

        // Single Value Enumeration

        [Test]
        public void CanGetListOfaSingleValue()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey1 = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                .Returns((param) =>
                {
                    param[0] = 1; // count of one
                    return HResult.S_OK;
                });

            // Expect a call to get the first value in the list
            PropertyKey outPropertyKey;
            PropertyStoreTestFixture
                .GetAt(0, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey1;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey1, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized )
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey1)
                .Returns("key9");

            // -> ACT
            var valueList = fixture.Values.ToList();

            // -> ASSERT
            Assert.AreEqual(1, valueList.Count);
            Assert.Contains(234, valueList);
        }

        [Test]
        public void CanGetListOfaSingleKey()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey1 = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                .Returns((param) =>
                {
                    param[0] = 1; // count of zero
                    return HResult.S_OK;
                });

            // Expect a call to get the first value in the list
            PropertyKey outPropertyKey;
            PropertyStoreTestFixture
                .GetAt(0, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey1;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey1, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized )
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey1)
                .Returns("key9");

            // -> ACT
            var keyList = fixture.Keys.ToList();

            // -> ASSERT
            Assert.AreEqual(1, keyList.Count);
            Assert.Contains("key9", keyList);
        }


        [Test]
        public void CanGetListOfaSingleKeyValuePair()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey1 = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                .Returns((param) =>
                {
                    param[0] = 1; // count of zero
                    return HResult.S_OK;
                });

            // Expect a call to get the first value in the list
            PropertyKey outPropertyKey;
            PropertyStoreTestFixture
                .GetAt(0, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey1;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey1, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized )
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey1)
                .Returns("key9");

            // -> ACT
            var keyList = fixture.ToList();

            // -> ASSERT
            Assert.AreEqual(1, keyList.Count);
            Assert.Contains(new KeyValuePair<string, object>("key9", 234), keyList);
        }

        // Many Value Enumeration

        [Test]
        public void CanGetListOfManyValues()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey1 = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);
            var returnedPropertyKey2 = new PropertyKey(new Guid("40999244-8186-4DB6-90C7-78BBDD85BD9B"), 4);

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                .Returns((param) =>
                {
                    param[0] = 2; // count of 2
                    return HResult.S_OK;
                });
            
            // Value 1:
            //
            // Expect a call to get the first value in the list
            PropertyKey outPropertyKey;
            PropertyStoreTestFixture
                .GetAt(0, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey1;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey1, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized )
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey1)
                .Returns("key9");

            // Value 2:
            //
            // Expect a call to get the first value in the list
            PropertyStoreTestFixture
                .GetAt(1, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey2;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey2, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(1); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized )
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey2)
                .Returns("key2");

            // -> ACT
            var valueList = fixture.Values.ToList();

            // -> ASSERT
            Assert.AreEqual(2, valueList.Count);
            Assert.Contains(234, valueList);
            Assert.Contains(1, valueList);
        }

        [Test]
        public void CanGetListOfManyKeys()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey1 = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);
            var returnedPropertyKey2 = new PropertyKey(new Guid("40999244-8186-4DB6-90C7-78BBDD85BD9B"), 4);

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                .Returns((param) =>
                {
                    param[0] = 2; // count of 2
                    return HResult.S_OK;
                });

            // Value 1:
            //
            // Expect a call to get the first value in the list
            PropertyKey outPropertyKey;
            PropertyStoreTestFixture
                .GetAt(0, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey1;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey1, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized )
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey1)
                .Returns("key9");

            // Value 2:
            //
            // Expect a call to get the first value in the list
            PropertyStoreTestFixture
                .GetAt(1, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey2;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey2, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(1); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized)
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey2)
                .Returns("key2");

            // -> ACT
            var valueList = fixture.Keys.ToList();

            // -> ASSERT
            Assert.AreEqual(2, valueList.Count);
            Assert.Contains("key9", valueList);
            Assert.Contains("key2", valueList);
        }

        [Test]
        public void CanGetListOfManyKeyValuePairs()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var returnedPropertyKey1 = new PropertyKey(new Guid("071576F4-388C-465C-A31E-52A46DE367C6"), 2);
            var returnedPropertyKey2 = new PropertyKey(new Guid("40999244-8186-4DB6-90C7-78BBDD85BD9B"), 4);

            // Expect a call to get the current property count
            int outCount;
            PropertyStoreTestFixture
                .GetCount(out outCount)
                .Returns((param) =>
                {
                    param[0] = 2; // count of 2
                    return HResult.S_OK;
                });

            // Value 1:
            //
            // Expect a call to get the first value in the list
            PropertyKey outPropertyKey;
            PropertyStoreTestFixture
                .GetAt(0, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey1;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropVariant outPropVariant;
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey1, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(234); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized )
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey1)
                .Returns("key9");

            // Value 2:
            //
            // Expect a call to get the first value in the list
            PropertyStoreTestFixture
                .GetAt(1, out outPropertyKey)
                .Returns((param) =>
                {
                    param[1] = returnedPropertyKey2;
                    return HResult.S_OK;
                });

            // Expect call to resolve the key in to a value
            PropertyStoreTestFixture
                .GetValue(returnedPropertyKey2, out outPropVariant)
                .Returns((param) =>
                {
                    param[1] = PropVariant.FromObject(1); // valid data;
                    return HResult.S_OK;
                });

            // Expect a call to resolve the property key (so we know if the key is recognized)
            WasapiPropertyNameTranslator
                .ResolvePropertyName(returnedPropertyKey2)
                .Returns("key2");

            // -> ACT
            var valueList = fixture.ToList();

            // -> ASSERT
            Assert.AreEqual(2, valueList.Count);
            Assert.Contains(new KeyValuePair<string, object>("key9", 234), valueList);
            Assert.Contains(new KeyValuePair<string, object>("key2", 1), valueList);
        }
    }
}
