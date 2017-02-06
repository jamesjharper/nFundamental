using System;
using System.Collections.Generic;
using Fundamental.Core.AudioFormats;
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
                    PrintDeviceProperty(deviceDetail.Name, deviceDetail.Value);
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine();
        }

        private static void PrintDeviceProperty(string name, object value)
        {
            if (value is AudioFormat)
            {
                PrintDeviceProperty(name, (AudioFormat) value);
            }
            else
            {
                System.Console.WriteLine($"     - {name}: {value}");
            }
        }

        private static void PrintDeviceProperty(string name, AudioFormat value)
        {
            System.Console.WriteLine($"     - {name}:");

            foreach (var formatPropery in value)
            {
                System.Console.WriteLine($"       [{formatPropery.Key}]: {formatPropery.Value}");
            }
            System.Console.WriteLine();
        }
    }
}
