using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundamental.Interface
{
    interface IAudioSourceFactory
    {

        /// <summary>
        /// Gets a information device instance.
        /// </summary>
        /// <param name="deviceToken">The device handle.</param>
        /// <returns></returns>
        IDeviceInfo GetAudioSource(IDeviceToken deviceToken);
    }
}
