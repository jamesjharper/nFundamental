namespace Fundamental.Interface
{
    public interface IPropertyBagKey
    {
        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets the item category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        string Name { get; }
    }
}
