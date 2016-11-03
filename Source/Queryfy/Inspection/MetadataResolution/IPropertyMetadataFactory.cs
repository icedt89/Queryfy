namespace JanHafner.Queryfy.Inspection.MetadataResolution
{
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IPropertyMetadataFactory"/> interface provides methods for retrieving metadata based on attributes of the supplied <see cref="PropertyInfo"/>.
    /// </summary>
    public interface IPropertyMetadataFactory
    {
        /// <summary>
        /// Gets the <see cref="IPropertyMetadata"/>
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <returns>The associated <see cref="IPropertyMetadata"/>.</returns>
        [NotNull]
        IPropertyMetadata GetPropertyMetadata([NotNull] PropertyInfo propertyInfo);
    }
}