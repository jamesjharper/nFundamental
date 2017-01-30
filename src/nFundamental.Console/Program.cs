using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Interface;

#if NET46
using Fundamental.Interface.Wasapi;
#endif

namespace Fundamental.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if NET46
        var factory = new WasapiInterfaceProvider();

        var deviceEnumerator = factory.GetAudioInterface<IDeviceEnumerator>();
        var deviceInfoFactory = factory.GetAudioInterface<IDeviceInfoFactory>();

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
