using System.Collections.Generic;
using System.Linq;
using Fundamental.Interface;

namespace Fundamental.Console.DeviceInfo
{

    public class DeviceDetail
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class DeviceDetailsVivisector
    {
        private readonly IDeviceInfo _deviceInfo;

        public DeviceDetailsVivisector(IDeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
        }


        public string GetDeviceTitle()
        {
            object deviceName;
            return _deviceInfo.Properties.TryGetValue("Device.FriendlyName", out deviceName) ? deviceName.ToString() : "Unknown";
        }


        public IEnumerable<DeviceDetail> GetNonGrouppedDeviceDetails()
        {
            return GetProperties().Select(x => new DeviceDetail
                {
                    Name = x.Key.Name,
                    Value = x.Value
                });
        }

        public IEnumerable<IGrouping<string, DeviceDetail>> GetGrouppedDeviceDetails()
        {
            return GetGrouppedProperties()
                .GroupBy(x => x.Key.Category,
                         x => new DeviceDetail()
                         {
                            Name = x.Key.Name,
                            Value = x.Value
                         });
        }


        private IEnumerable<KeyValuePair<IPropertyBagKey, object>> GetProperties()
        {
            return _deviceInfo.Properties
                 .Where(property => !(property.Key is IGrouppedPropertyBagKey));
        }

        private IEnumerable<KeyValuePair<IGrouppedPropertyBagKey, object>> GetGrouppedProperties()
        {
            return _deviceInfo.Properties
                .Where(x => x.Key is IGrouppedPropertyBagKey)
                .Select(x => new KeyValuePair<IGrouppedPropertyBagKey, object>((IGrouppedPropertyBagKey) x.Key, x.Value));
        }
    }
}
