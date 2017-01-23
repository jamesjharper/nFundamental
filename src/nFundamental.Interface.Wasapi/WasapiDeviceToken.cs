using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceToken : IDeviceToken
    {
        /// <summary>
        /// The device enumerator which is used for lazy loading the 
        /// IMMDevice instance
        /// </summary>
        private readonly IMMDeviceEnumerator _deviceEnumerator;

        /// <summary>
        /// The identifier (Might be null depending on how the instance was created)
        /// </summary>
        private string _id;

        /// <summary>
        /// The multimedia interface device. (Might be null depending on how the instance was created)
        /// </summary>
        private IMMDevice _mMDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceToken"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="deviceEnumerator"></param>
        public WasapiDeviceToken(string id, IMMDeviceEnumerator deviceEnumerator)
        {
            _deviceEnumerator = deviceEnumerator;
            _id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceToken"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceEnumerator">The device enumerator.</param>
        public WasapiDeviceToken(IMMDevice device, IMMDeviceEnumerator deviceEnumerator)
        {
            _deviceEnumerator = deviceEnumerator;
            _mMDevice = device;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceToken"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="device">The device.</param>
        public WasapiDeviceToken(string id, IMMDevice device)
        {
            _mMDevice = device;
            _id = id;
        }


        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id => _id ?? (_id = GetDeviceId());

        /// <summary>
        /// Gets or sets the multimedia interface device.
        /// </summary>
        /// <value>
        /// The mm device.
        /// </value>
        public IMMDevice MmDevice => _mMDevice ?? (_mMDevice = GetMmDevice());

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Id;

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var other = obj as WasapiDeviceToken;
            return other != null && Equals(other.Id, Id);
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // private methods

        private IMMDevice GetMmDevice()
        {
            IMMDevice device;
            _deviceEnumerator.GetDevice(_id, out device).ThrowIfFailed();
            return device;
        }

        private string GetDeviceId()
        {
            string deviceId;
            _mMDevice.GetId(out deviceId).ThrowIfFailed();
            return deviceId;
        }
    }
}
