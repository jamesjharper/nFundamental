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
            System.Console.WriteLine(" -----------------");
            System.Console.WriteLine("  CAPTURE DEVICES ");
            System.Console.WriteLine(" -----------------");
            System.Console.WriteLine();

            var capatureDevices = deviceEnumerator.GetDevices(DeviceType.Capture);
            PrintDevices(capatureDevices, deviceInfoFactory);

            System.Console.WriteLine();
            System.Console.WriteLine();

            System.Console.WriteLine(" ----------------");
            System.Console.WriteLine("  RENDER DEVICES ");
            System.Console.WriteLine(" ----------------");
            System.Console.WriteLine();

            var renderDevices = deviceEnumerator.GetDevices(DeviceType.Render);
            PrintDevices(renderDevices, deviceInfoFactory);
        }

        private static void PrintDevices(IEnumerable<IDeviceToken> devices, IDeviceInfoFactory deviceInfoFactory)
        {
            foreach (var device in devices)
            {
                var deviceInfo = deviceInfoFactory.GetInfoDevice(device);
                var deviceDetailsVivisector = new DeviceDetailsVivisector(deviceInfo);
                PrintDeviceDetails(deviceDetailsVivisector);
            }
        }


        private static void PrintDeviceDetails(DeviceDetailsVivisector deviceInfo)
        {

            System.Console.WriteLine($" Device: {deviceInfo.GetDeviceTitle()}");

            foreach (var deviceDetail in deviceInfo.GetNonGroupedDeviceDetails())
            {
                System.Console.WriteLine($"    {deviceDetail.Name}: {deviceDetail.Value}");
            }

            foreach (var group in deviceInfo.GetGroupedDeviceDetails())
            {
                System.Console.WriteLine($"    {group.Key}");
                foreach (var deviceDetail in group)
                {
                    System.Console.WriteLine($"     - {deviceDetail.Name}: {deviceDetail.Value}");
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine();
        }
    }
}
