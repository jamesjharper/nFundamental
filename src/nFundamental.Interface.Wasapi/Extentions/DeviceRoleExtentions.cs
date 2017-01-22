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
        public static Role ConvertToWasapiDeviceRole(this DeviceRole deviceRole)
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


        /// <summary>
        /// Converts WASAPI Role enum to fundamental device role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">role - null</exception>
        public static DeviceRole ConvertToFundamentalDeviceRole(this Role role)
        {
            switch (role)
            {
                case Role.Console:
                    return DeviceRole.Console;

                case Role.Multimedia:
                    return DeviceRole.Multimedia;

                case Role.Communications:
                    return DeviceRole.Communications;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }
    }
}
