using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class DevicePeriod
    {
        /// <summary>
        /// A time value specifying the default interval between periodic processing passes by the audio engine. 
        /// </summary>
        /// <value>
        /// The process interval.
        /// </value>
        public TimeSpan Default { get; }

        /// <summary>
        /// A value specifying the minimum interval between periodic processing passes by the audio endpoint device.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public TimeSpan Minimum { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevicePeriod"/> class.
        /// </summary>
        /// <param name="defaultPeriod">The process interval.</param>
        /// <param name="minimumPeriod">The minimum interval.</param>
        public DevicePeriod(TimeSpan defaultPeriod, TimeSpan minimumPeriod)
        {
            Default = defaultPeriod;
            Minimum = minimumPeriod;
        }
    }
}
