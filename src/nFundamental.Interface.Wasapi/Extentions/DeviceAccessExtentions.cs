
using System;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi.Extentions
{
    internal static class DeviceAccessExtentions
    {
        /// <summary>
        /// Converts to WASAPI audio client share mode.
        /// </summary>
        /// <param name="deviceAccess">The device access.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">deviceAccess - null</exception>
        public static AudioClientShareMode ConvertToWasapiAudioClientShareMode(this DeviceAccess deviceAccess)
        {
            switch (deviceAccess)
            {
                case DeviceAccess.Shared:
                    return AudioClientShareMode.Shared;
                case DeviceAccess.Exclusive:
                    return AudioClientShareMode.Exclusive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deviceAccess), deviceAccess, null);
            }
        }


        /// <summary>
        /// Converts to fundamental device access.
        /// </summary>
        /// <param name="audioClientShareMode">The audio client share mode.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">audioClientShareMode - null</exception>
        public static DeviceAccess ConvertToFundamentalDeviceAccess(this AudioClientShareMode audioClientShareMode)
        {
            switch (audioClientShareMode)
            {
                case AudioClientShareMode.Shared:
                    return DeviceAccess.Shared;
                case AudioClientShareMode.Exclusive:
                    return DeviceAccess.Exclusive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioClientShareMode), audioClientShareMode, null);
            }
        }
    }
}