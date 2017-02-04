using System.Collections.Generic;
using System.Linq;
using Fundamental.Interface;

namespace Fundamental.Console.DeviceInfo
{

    public class DeviceDetailsVivisector
    {
        /// <summary>
        /// The device information
        /// </summary>
        private readonly IDeviceInfo _deviceInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDetailsVivisector"/> class.
        /// </summary>
        /// <param name="deviceInfo">The device information.</param>
        public DeviceDetailsVivisector(IDeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
        }


        /// <summary>
        /// Gets the device title.
        /// </summary>
        /// <returns></returns>
        public string GetDeviceTitle()
        {
            object deviceName;
            return _deviceInfo.Properties.TryGetValue("Device.FriendlyName", out deviceName) ? deviceName.ToString() : "Unknown";
        }


        /// <summary>
        /// Gets the non grouped device details.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeviceProperty> GetNonGroupedDeviceDetails()
        {
            return GetProperties().Select(x => new DeviceProperty
                {
                    Name = x.Key.Name,
                    Value = x.Value
                });
        }

        /// <summary>
        /// Gets the grouped device details.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGrouping<string, DeviceProperty>> GetGroupedDeviceDetails()
        {
            return GetGroupedProperties()
                .GroupBy(x => x.Key.Category,
                         x => new DeviceProperty()
                         {
                            Name = x.Key.Name,
                            Value = x.Value
                         });
        }

        // Private Methods

        private IEnumerable<KeyValuePair<IPropertyBagKey, object>> GetProperties()
        {
            return _deviceInfo.Properties
                 .Where(property => !(property.Key is IGroupedPropertyBagKey));
        }

        private IEnumerable<KeyValuePair<IGroupedPropertyBagKey, object>> GetGroupedProperties()
        {
            return _deviceInfo.Properties
                .Where(x => x.Key is IGroupedPropertyBagKey)
                .Select(x => new KeyValuePair<IGroupedPropertyBagKey, object>((IGroupedPropertyBagKey) x.Key, x.Value));
        }
    }
}
