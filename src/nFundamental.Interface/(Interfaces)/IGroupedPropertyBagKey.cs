namespace Fundamental.Interface
{
    public interface IGroupedPropertyBagKey : IPropertyBagKey
    {
        /// <summary>
        /// Gets the group category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        string Category { get; }

    }
}
