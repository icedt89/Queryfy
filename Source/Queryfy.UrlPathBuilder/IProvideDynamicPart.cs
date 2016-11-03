namespace JanHafner.Queryfy.UrlPathBuilder
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IProvideDynamicPart"/> interface provides a common way of retrieving a dynamic value for an <see cref="Uri"/>-Part.
    /// </summary>
    public interface IProvideDynamicPart
    {
        /// <summary>
        /// Gets the value for the dynamic <see cref="Uri"/>-Part.
        /// </summary>
        [NotNull]
        String DynamicUriPartName { get; }
    }
}