using System;
using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi.Extentions
{
    internal static class DeviceRoleExtentions
    {
        /// <summary>
        /// Converts DeviceRole enum to WASAPI Role enum.
        /// </summary>
        /// <param name="deviceRole">The device role.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">deviceRole - null</exception>
        public static Role ConvertToWasapiDataFlow(this DeviceRole deviceRole)
        {
            switch (deviceRole)
            {
                case DeviceRole.Console:
                    return Role.Console;

                case DeviceRole.Multimedia:
                    return Role.Multimedia;

                case DeviceRole.Communications:
                    return Role.Communications;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deviceRole), deviceRole, null);
            }
        }
    }
}
