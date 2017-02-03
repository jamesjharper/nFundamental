using Fundamental.Interface;
using Fundamental.Interface.Wasapi;
#if NET46

#endif

namespace nFundamental.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if NET46
        var factory = new WasapiInterfaceProvider();

        var deviceEnumerator = factory.Get<IDeviceEnumerator>();
        var deviceInfoFactory = factory.Get<IDeviceInfoFactory>();

        PrintDevices(deviceEnumerator, deviceInfoFactory);
#endif


        }


        private static void PrintDevices(IDeviceEnumerator deviceEnumerator, IDeviceInfoFactory deviceInfoFactory)
        {
            var allDevices = deviceEnumerator.GetDevices();
            foreach (var device in allDevices)
            {
                var deviceInfo = deviceInfoFactory.GetInfoDevice(device);
                foreach (var propertyValue in deviceInfo.Properties)
                {
                        System.Console.WriteLine(propertyValue);
                }
            }
        }
    }
}
