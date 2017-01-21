using System.Collections.Generic;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Extentions
{
    public static class IMMDeviceCollectionExtentions
    {

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <param name="deviceCollection">The device collection.</param>
        /// <returns></returns>
        public static IEnumerable<IMMDevice> GetEnumerable(this IMMDeviceCollection deviceCollection)
        {
            int count;
            deviceCollection.GetCount(out count).ThrowIfFailed();

            for (var i = 0; i < count; i++)
            {
                IMMDevice mmDevice;
                deviceCollection.Item(i, out mmDevice).ThrowIfFailed();
                yield return mmDevice;
            }
        }
    }
}
