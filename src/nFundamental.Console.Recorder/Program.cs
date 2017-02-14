using System;
using Fundamental.Interface;

namespace Fundamental.Console.Recorder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var factory = AudioInterfaceProvider.GetProvider();

                var deviceProvider = factory.Get<IDefaultDeviceProvider>();
                var deviceSourceFactory = factory.Get<IDeviceAudioSourceFactory>();
                var defaultDevice = deviceProvider.GetDefaultDevice(DeviceType.Capture);

                var device = deviceSourceFactory.GetAudioSource(defaultDevice);

                device.DataAvailable += OnDataAvailable;
                device.Start();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }

            System.Console.ReadLine();
        }

        private static void OnDataAvailable(object sender, Core.DataAvailableEventArgs e)
        {
            System.Console.WriteLine("Received " + e.ByteSize + " bytes");
        }
    }
}
