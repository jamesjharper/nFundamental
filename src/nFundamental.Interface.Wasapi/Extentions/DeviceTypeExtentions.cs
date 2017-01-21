using System;
using System.Linq;
using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Extentions
{
    internal static class DeviceTypeExtentions
    {
        /// <summary>
        /// Converts DeviceType enum to WASAPI DataFlow enum.
        /// </summary>
        /// <param name="deviceType">Type of the device.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">deviceType - null</exception>
        public static DataFlow ConvertToWasapiDataFlow(this DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Capature:
                    return DataFlow.Capture;
                case DeviceType.Render:
                    return DataFlow.Render;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null);
            }
        }

        /// <summary>
        /// Converts DeviceType enum to WASAPI DataFlow enum.
        /// </summary>
        /// <param name="deviceType">Type of the device.</param>
        /// <returns></returns>
        public static DataFlow ConvertToWasapiDeviceState(this DeviceType[] deviceType)
        {
            var hasRender = deviceType.Contains(DeviceType.Render);
            var hasCapature = deviceType.Contains(DeviceType.Capature);

            if (hasRender && hasCapature)
                return DataFlow.All;

            if (hasRender)
                return DataFlow.Render;

            if (hasCapature)
                return DataFlow.Capture;

            throw new ArgumentException("No recognized device types given", nameof(deviceType));
        }
    }
}
