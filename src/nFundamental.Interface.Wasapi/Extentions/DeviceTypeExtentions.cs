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
                case DeviceType.Capture:
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
            var hasCapature = deviceType.Contains(DeviceType.Capture);

            if (hasRender && hasCapature)
                return DataFlow.All;

            if (hasRender)
                return DataFlow.Render;

            if (hasCapature)
                return DataFlow.Capture;

            throw new ArgumentException("No recognized device types given", nameof(deviceType));
        }

        /// <summary>
        /// Converts a WASAPI data flow to fundamental device type.
        /// </summary>
        /// <param name="deviceType">Type of the device.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">deviceType - null</exception>
        public static DeviceType ConvertToFundamentalDeviceState(this DataFlow deviceType)
        {
            if ((deviceType & DataFlow.Capture) != 0)
                return DeviceType.Capture;

            if ((deviceType & DataFlow.Render)!= 0)
                return DeviceType.Render;

            throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null);
        }
    }
}
