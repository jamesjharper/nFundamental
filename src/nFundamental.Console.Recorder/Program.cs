using System;
using Fundamental.Core;
using Fundamental.Interface;

namespace Fundamental.Console.Recorder
{
    public class Program
    {
        private static byte[] _buffer = new byte[90000];

        private static readonly object rwLock = new object();
        private static int bufferPos = 0;
        private static IHardwareAudioSink _renderDevice;
        private static IHardwareAudioSource _captureDevice;


        public static void Main(string[] args)
        {
            try
            {
                var factory = AudioInterfaceProvider.GetProvider();

                var deviceProvider = factory.Get<IDefaultDeviceProvider>();

                var deviceSourceFactory = factory.Get<IDeviceAudioSourceFactory>();
                var deviceSinkFactory = factory.Get<IDeviceAudioSinkFactory>();

                var captureDeviceToken = deviceProvider.GetDefaultDevice(DeviceType.Capture);
                var renderDeviceToken = deviceProvider.GetDefaultDevice(DeviceType.Render);

                _captureDevice = deviceSourceFactory.GetAudioSource(captureDeviceToken);
                _renderDevice = deviceSinkFactory.GetAudioSink(renderDeviceToken);

                var _captureFormat = (_captureDevice as IFormatGetable)?.GetFormat();
                var _renderFormat = (_renderDevice as IFormatGetable)?.GetFormat();

                _captureDevice.DataAvailable += OnDataAvailable;
                _renderDevice.DataRequested += OnDataRequested;

                _captureDevice.Start();
                _renderDevice.Start();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }

            System.Console.ReadLine();
        }

        private static void OnDataRequested(object sender, Core.DataRequestedEventArgs e)
        {
           // lock (rwLock)
            {
                var written =_renderDevice.Write(_buffer, 0, bufferPos);

                Array.Copy(_buffer, written, _buffer, 0, _buffer.Length - bufferPos);
                System.Console.WriteLine("out " + e.ByteSize + " bytes");
                bufferPos = 0;
            }
   
        }

        private static void OnDataAvailable(object sender, Core.DataAvailableEventArgs e)
        {
            //lock (rwLock)
            {
                bufferPos += _captureDevice.Read(_buffer, bufferPos, _buffer.Length - bufferPos);
                System.Console.WriteLine("In  " + e.ByteSize + " bytes");
            }
        }
    }
}
