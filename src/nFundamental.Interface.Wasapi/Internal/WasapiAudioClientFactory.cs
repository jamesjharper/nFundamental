using System;
using System.Reflection;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiAudioClientFactory : IWasapiAudioClientFactory
    {
        /// <summary>
        /// Create a new audio client COM instance.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public IAudioClient FactoryAudioClient(WasapiDeviceToken token)
        {
            var immDevice = token.MmDevice;
            return Activate<IAudioClient>(immDevice);
        }


        /// <summary>
        /// Activates a COM object from the specified device.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="immDevice">The WASAPI device object.</param>
        /// <returns></returns>
        /// <exception cref="Fundamental.Interface.Wasapi.DeviceNotAccessableException"></exception>
        private static T Activate<T>(IMMDevice immDevice) where T : class
        {
            var interfaceId = typeof(T).GetTypeInfo().GUID;
            object result;
            var hr = immDevice.Activate( 
                /* Com interface requested    */ interfaceId,
                /* Com Server type            */ ClsCtx.LocalServer,
                /* Activation Flags           */ IntPtr.Zero,
                /* [out] resulting com object */ out result);


            if(hr == HResult.AUDCLNT_E_DEVICE_INVALIDATED)
                throw new DeviceNotAccessableException($"Failed to Activate WASAPI com object {typeof(T).Name}, as the target device is unplugged");

            hr.ThrowIfFailed();
            return ComObject.QuearyInterface<T>(result);
        }
    }
}
