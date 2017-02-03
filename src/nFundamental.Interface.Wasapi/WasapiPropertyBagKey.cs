
using System;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiPropertyBagKey : IGrouppedPropertyBagKey
    {
        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Id { get; }

        /// <summary>
        /// Gets the item category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the group category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; }

        /// <summary>
        /// Gets the unified device property model property identifier.
        /// </summary>
        /// <value>
        /// The unified device property model property identifier.
        /// </value>
        public int UdmPropertyId { get; }

        /// <summary>
        /// Gets the unified device property model format identifier.
        /// </summary>
        /// <value>
        /// The unified device property model format identifier.
        /// </value>
        public Guid UdmFormatId { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is known by this API.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is known property; otherwise, <c>false</c>.
        /// </value>
        public bool IsKnownProperty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiPropertyBagKey" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="keyId">The key identifier.</param>
        /// <param name="category">The category.</param>
        /// <param name="udmPropertyId">The unified device property model property identifier.</param>
        /// <param name="udmFormatId">The unified device property model format identifier.</param>
        /// <param name="isRecognized">if set to <c>true</c> [is recognized].</param>
        public WasapiPropertyBagKey(string keyId, string name, string category, int udmPropertyId, Guid udmFormatId, bool isRecognized)
        {
            Category = category;
            UdmFormatId = udmFormatId;
            Name = name;
            UdmPropertyId = udmPropertyId;
            Id = keyId;
            IsKnownProperty = isRecognized;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is WasapiPropertyBagKey))
                return false;

            var other = (WasapiPropertyBagKey)obj;
            return Equals(other.UdmFormatId, UdmFormatId) &&
                  Equals(other.UdmPropertyId, UdmPropertyId);
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return UdmFormatId.GetHashCode() ^ UdmPropertyId;
        }
    }
}
