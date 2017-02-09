using System;
using System.Reflection;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiAudioClientInteropFactory : IWasapiAudioClientInteropFactory
    {
        /// <summary>
        /// The thread dispatch strategy
        /// </summary>
        private readonly IComThreadInterpoStrategy _comThreadInterpoStrategy;

        /// <summary>
        /// The wave format converter
        /// </summary>
        private readonly IAudioFormatConverter<WaveFormat> _waveFormatConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioClientInteropFactory"/> class.
        /// </summary>
        /// <param name="comThreadInterpoStrategy">The thread dispatch strategy.</param>
        /// <param name="waveFormatConverter">The wave format converter.</param>
        public WasapiAudioClientInteropFactory(IComThreadInterpoStrategy comThreadInterpoStrategy,
                                        IAudioFormatConverter<WaveFormat> waveFormatConverter)
        {
            _comThreadInterpoStrategy = comThreadInterpoStrategy;
            _waveFormatConverter = waveFormatConverter;
        }

        /// <summary>
        /// Create a new audio client COM instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        /// <exception cref="UnsupportedTokenTypeException"></exception>
        public IWasapiAudioClientInterop FactoryAudioClient(IDeviceToken deviceToken)
        {
            var token = deviceToken as WasapiDeviceToken;
            if (token != null)
                return FactoryAudioClient(token);

            var message = $"{nameof(WasapiAudioClientInteropFactory)} can only except tokens of type {nameof(WasapiDeviceToken)}";
            throw new UnsupportedTokenTypeException(message, typeof(WasapiDeviceToken));
        }

        /// <summary>
        /// Factories the audio client.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public IWasapiAudioClientInterop FactoryAudioClient(WasapiDeviceToken deviceToken)
        {
            var iAudioClient = Activate<IAudioClient>(deviceToken.MmDevice);
            return new WasapiAudioClientInterop(iAudioClient, _comThreadInterpoStrategy, _waveFormatConverter);
        }

        /// <summary>
        /// Activates a COM object from the specified device.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="immDevice">The WASAPI device object.</param>
        /// <returns></returns>
        /// <exception cref="Fundamental.Interface.Wasapi.DeviceNotAccessableException"></exception>
        private T Activate<T>(IMMDevice immDevice) where T : class
        {
            var interfaceId = typeof(T).GetTypeInfo().GUID;
            var result = Activate(immDevice, interfaceId);
            return ComObject.QuearyInterface<T>(result);
        }


        /// <summary>
        /// Activates a COM object from the specified device.
        /// </summary>
        /// <param name="immDevice">The device.</param>
        /// <param name="iid">The Interface Id.</param>
        /// <returns></returns>
        /// <exception cref="DeviceNotAccessableException"></exception>
        private object Activate(IMMDevice immDevice, Guid iid)
        {
            if (_comThreadInterpoStrategy.RequiresInvoke())
            {
                return _comThreadInterpoStrategy.InvokeOnTargetThread(new Func<IMMDevice, Guid, object>(Activate), immDevice, iid);
            }

            object result;
            var hr = immDevice.Activate(
                /* Com interface requested    */ iid,
                /* Com Server type            */ ClsCtx.LocalServer,
                /* Activation Flags           */ IntPtr.Zero,
                /* [out] resulting com object */ out result);


            if (hr == HResult.AUDCLNT_E_DEVICE_INVALIDATED)
                throw new DeviceNotAccessableException($"Failed to Activate WASAPI com object {iid}, as the target device is unplugged");

            hr.ThrowIfFailed();
            return result;
        }
    }
}
