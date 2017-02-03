using System;
using System.Collections.Generic;
using Fundamental.Interface;

namespace Fundamental.Console.DeviceInfo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var factory = AudioInterfaceProvider.GetProvider();

                var deviceEnumerator = factory.Get<IDeviceEnumerator>();
                var deviceInfoFactory = factory.Get<IDeviceInfoFactory>();

                PrintDevices(deviceEnumerator, deviceInfoFactory);

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Environment.Exit(-1);

            }

            System.Console.ReadLine();
        }


        private static void PrintDevices(IDeviceEnumerator deviceEnumerator, IDeviceInfoFactory deviceInfoFactory)
        {
            var allDevices = deviceEnumerator.GetDevices();
            foreach (var device in allDevices)
            {
                var deviceInfo = deviceInfoFactory.GetInfoDevice(device);
                PrintDeviceDetails(deviceInfo); 
            }
        }


        private static void PrintDeviceDetails(IDeviceInfo deviceInfo)
        {
            var deviceDetailsVivisector = new DeviceDetailsVivisector(deviceInfo);


            System.Console.WriteLine($"Device: {deviceDetailsVivisector.GetDeviceTitle()}");

            foreach (var deviceDetail in deviceDetailsVivisector.GetNonGrouppedDeviceDetails())
            {
                System.Console.WriteLine($"   {deviceDetail.Name}: {deviceDetail.Value}");
            }

            foreach (var group in deviceDetailsVivisector.GetGrouppedDeviceDetails())
            {
                System.Console.WriteLine($"   {group.Key}");
                foreach (var deviceDetail in group)
                {
                    System.Console.WriteLine($"    - {deviceDetail.Name}: {deviceDetail.Value}");
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine();
        }
    }
}
