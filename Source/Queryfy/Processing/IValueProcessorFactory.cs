namespace JanHafner.Queryfy.Processing
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IValueProcessorFactory"/> interface provides methods for retrieving <see cref="IValueProcessor"/> instances based on <see cref="Type"/> specific attributes.
    /// </summary>
    public interface IValueProcessorFactory
    {
        /// <summary>
        ///Gets an <see cref="IValueProcessor"/> instance.
        /// </summary>
        /// <param name="propertyType">The <see cref="Type"/> of the property.</param>
        /// <returns>An implementation of the <see cref="IValueProcessor"/> interface.</returns>
        [NotNull]
        IValueProcessor GetUrlValueProcessorForType([NotNull] Type propertyType);

        /// <summary>
        /// Gets an <see cref="IValueProcessor"/> instance.
        /// </summary>
        /// <param name="propertyType">The <see cref="Type"/> of the property.</param>
        /// <param name="valueProcessorType">The <see cref="Type"/> of the <see cref="IValueProcessor"/> to retrieve.</param>
        /// <returns>An implementation of the <see cref="IValueProcessor"/> interface.</returns>
        [NotNull]
        IValueProcessor GetUrlValueProcessor([NotNull] Type propertyType, [CanBeNull] Type valueProcessorType);
    }
}