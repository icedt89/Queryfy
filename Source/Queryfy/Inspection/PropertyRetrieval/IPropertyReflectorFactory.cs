namespace JanHafner.Queryfy.Inspection.PropertyRetrieval
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IPropertyReflectorFactory"/> interface provides methods for creating <see cref="IPropertyReflector"/> instances.
    /// </summary>
    [PublicAPI]
    public interface IPropertyReflectorFactory
    {
        /// <summary>
        /// Gets an instance of an <see cref="IPropertyReflector"/>.
        /// </summary>
        /// <param name="instanceType">The <see cref="Type"/> in which to choose the right <see cref="IPropertyReflector"/>.</param>
        /// <returns>An <see cref="IPropertyReflector"/>.</returns>
        [PublicAPI]
        [NotNull]
        [Pure]
        IPropertyReflector GetPropertyReflector([NotNull] Type instanceType);
    }
}